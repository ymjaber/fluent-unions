using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Common;

/// <summary>
/// Analyzer that detects misuse of builder types (EnsureBuilder and FilterBuilder) that should be
/// completed with terminal operations or implicit conversions.
/// </summary>
/// <remarks>
/// This analyzer helps prevent incorrect usage of builder pattern types by detecting when they are:
/// - Assigned to variables without completion
/// - Returned from methods without proper conversion
/// - Used in expression-bodied void methods
/// - Passed as arguments without conversion
/// 
/// The builders should always be completed with:
/// - Build() method call
/// - Map/Bind/BindAll methods (for EnsureBuilder)
/// - Implicit conversion to Result/Option
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BuilderMisuseAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor EnsureBuilderRule = new(
        id: DiagnosticIds.EnsureBuilderMisuse,
        title: "EnsureBuilder should be completed with Build() or terminal operation",
        messageFormat: "EnsureBuilder is not properly completed. Use Build(), Map(), Bind(), BindAll(), or ensure implicit conversion to Result.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "EnsureBuilder should be completed with a terminal operation to produce a Result.");

    private static readonly DiagnosticDescriptor FilterBuilderRule = new(
        id: DiagnosticIds.FilterBuilderMisuse,
        title: "FilterBuilder should be completed with Build() or implicit conversion",
        messageFormat: "FilterBuilder is not properly completed. Use Build() or ensure implicit conversion to Option.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "FilterBuilder should be completed with Build() or implicitly converted to Option.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        ImmutableArray.Create(EnsureBuilderRule, FilterBuilderRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        // Check method invocations that might return builders
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);
        
        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
            return;
            
        var returnType = methodSymbol.ReturnType;
        if (returnType == null)
            return;
            
        // Check if this returns a builder type
        var (isBuilder, builderType) = GetBuilderType(returnType);
        if (!isBuilder)
            return;
            
        // Check if the builder is properly used
        if (!IsBuilderProperlyUsed(invocation, context.SemanticModel, returnType))
        {
            var diagnostic = builderType == BuilderType.Ensure
                ? Diagnostic.Create(EnsureBuilderRule, invocation.GetLocation())
                : Diagnostic.Create(FilterBuilderRule, invocation.GetLocation());
                
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static (bool isBuilder, BuilderType builderType) GetBuilderType(ITypeSymbol type)
    {
        if (SymbolHelpers.IsEnsureBuilderType(type))
            return (true, BuilderType.Ensure);
            
        if (SymbolHelpers.IsFilterBuilderType(type))
            return (true, BuilderType.Filter);
            
        return (false, BuilderType.None);
    }

    private static bool IsBuilderProperlyUsed(InvocationExpressionSyntax builderInvocation, 
        SemanticModel semanticModel, ITypeSymbol builderType)
    {
        var parent = builderInvocation.Parent;
        
        // Check if it's immediately followed by a terminal operation
        if (parent is MemberAccessExpressionSyntax memberAccess && 
            memberAccess.Expression == builderInvocation)
        {
            var memberName = memberAccess.Name.Identifier.Text;
            if (IsTerminalOperation(memberName, builderType))
                return true;
        }
        
        // Check if it's being implicitly converted
        if (IsImplicitlyConverted(builderInvocation, semanticModel, builderType))
            return true;
            
        // Check specific misuse patterns
        return !IsMisused(builderInvocation, semanticModel);
    }

    private static bool IsTerminalOperation(string methodName, ITypeSymbol builderType)
    {
        // Build() is terminal for both builders
        if (methodName == "Build")
            return true;
            
        // Additional terminal operations for EnsureBuilder
        if (builderType.Name == "EnsureBuilder")
        {
            return methodName switch
            {
                "Map" => true,
                "Bind" => true,
                "BindAll" => true,
                _ => false
            };
        }
        
        return false;
    }

    private static bool IsImplicitlyConverted(ExpressionSyntax expression, SemanticModel semanticModel, 
        ITypeSymbol builderType)
    {
        var parent = expression.Parent;
        
        // Get the expected type at this position
        var typeInfo = semanticModel.GetTypeInfo(expression);
        var convertedType = typeInfo.ConvertedType;
        
        if (convertedType == null)
            return false;
            
        // Check if converting to Result (for EnsureBuilder) or Option (for FilterBuilder)
        if (builderType.Name == "EnsureBuilder")
        {
            return IsResultType(convertedType);
        }
        else if (builderType.Name == "FilterBuilder")
        {
            return IsOptionType(convertedType);
        }
        
        return false;
    }

    private static bool IsMisused(InvocationExpressionSyntax builderInvocation, SemanticModel semanticModel)
    {
        var parent = builderInvocation.Parent;
        
        // Assignment to variable
        if (parent is EqualsValueClauseSyntax)
            return true;
            
        // Assignment expression
        if (parent is AssignmentExpressionSyntax)
            return true;
            
        // Return statement (unless the method returns Result/Option)
        if (parent is ReturnStatementSyntax)
        {
            var enclosingMethod = GetEnclosingMethod(parent);
            if (enclosingMethod != null)
            {
                var methodSymbol = semanticModel.GetDeclaredSymbol(enclosingMethod);
                if (methodSymbol?.ReturnType != null)
                {
                    var builderType = semanticModel.GetTypeInfo(builderInvocation).Type;
                    if (builderType?.Name == "EnsureBuilder")
                        return !IsResultType(methodSymbol.ReturnType);
                    else if (builderType?.Name == "FilterBuilder")
                        return !IsOptionType(methodSymbol.ReturnType);
                }
            }
        }
        
        // Expression-bodied member (especially void methods)
        if (parent is ArrowExpressionClauseSyntax arrowClause)
        {
            var memberDeclaration = arrowClause.Parent;
            
            // Check for void methods
            if (memberDeclaration is MethodDeclarationSyntax methodDecl)
            {
                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDecl);
                if (methodSymbol?.ReturnsVoid == true)
                    return true;
                    
                // Check if return type matches
                if (methodSymbol?.ReturnType != null)
                {
                    var builderType = semanticModel.GetTypeInfo(builderInvocation).Type;
                    if (builderType?.Name == "EnsureBuilder")
                        return !IsResultType(methodSymbol.ReturnType);
                    else if (builderType?.Name == "FilterBuilder")
                        return !IsOptionType(methodSymbol.ReturnType);
                }
            }
        }
        
        // Passed as argument (unless parameter expects Result/Option)
        if (parent is ArgumentSyntax argument)
        {
            var parameter = GetCorrespondingParameter(argument, semanticModel);
            if (parameter?.Type != null)
            {
                var builderType = semanticModel.GetTypeInfo(builderInvocation).Type;
                if (builderType?.Name == "EnsureBuilder")
                    return !IsResultType(parameter.Type);
                else if (builderType?.Name == "FilterBuilder")
                    return !IsOptionType(parameter.Type);
            }
        }
        
        // Expression statement (builder invoked but result ignored)
        if (parent is ExpressionStatementSyntax)
            return true;
        
        return false;
    }

    private static bool IsResultType(ITypeSymbol type) => SymbolHelpers.IsResultType(type);

    private static bool IsOptionType(ITypeSymbol type) => SymbolHelpers.IsOptionType(type);

    private static MethodDeclarationSyntax? GetEnclosingMethod(SyntaxNode node)
    {
        var current = node.Parent;
        while (current != null)
        {
            if (current is MethodDeclarationSyntax method)
                return method;
            current = current.Parent;
        }
        return null;
    }

    private static IParameterSymbol? GetCorrespondingParameter(ArgumentSyntax argument, SemanticModel semanticModel)
    {
        if (argument.Parent?.Parent is InvocationExpressionSyntax invocation)
        {
            var symbolInfo = semanticModel.GetSymbolInfo(invocation);
            if (symbolInfo.Symbol is IMethodSymbol method)
            {
                var argumentList = (ArgumentListSyntax)argument.Parent;
                var index = argumentList.Arguments.IndexOf(argument);
                
                if (index >= 0 && index < method.Parameters.Length)
                    return method.Parameters[index];
            }
        }
        return null;
    }

    private enum BuilderType
    {
        None,
        Ensure,
        Filter
    }
}