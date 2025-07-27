using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Common;

/// <summary>
/// Analyzer that detects misuse of builder types (EnsureBuilder and FilterBuilder) that should be
/// completed with validation or filter methods.
/// </summary>
/// <remarks>
/// This analyzer helps prevent incorrect usage of builder pattern types by detecting when they are:
/// - Assigned to variables without completion
/// - Returned from methods without proper completion
/// - Used in expression-bodied void methods
/// - Passed as arguments without completion
/// - Accessed via properties (.Ensure or .Filter) without calling a terminal method
/// 
/// The builders should always be completed with:
/// - Satisfies() method call
/// - Predefined validation/filter methods (NotEmpty, GreaterThan, etc.)
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BuilderMisuseAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor EnsureBuilderRule = new(
        id: DiagnosticIds.EnsureBuilderMisuse,
        title: "EnsureBuilder should be completed with a validation method",
        messageFormat: "EnsureBuilder is not properly completed. Call a validation method like Satisfies(), NotEmpty(), GreaterThan(), etc. to produce a Result.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "EnsureBuilder should be completed with a validation method to produce a Result.");

    private static readonly DiagnosticDescriptor FilterBuilderRule = new(
        id: DiagnosticIds.FilterBuilderMisuse,
        title: "FilterBuilder should be completed with a filter method",
        messageFormat: "FilterBuilder is not properly completed. Call a filter method like Satisfies(), NotEmpty(), GreaterThan(), etc. to produce an Option.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "FilterBuilder should be completed with a filter method to produce an Option.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        ImmutableArray.Create(EnsureBuilderRule, FilterBuilderRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        // Check property/member access that might return builders (e.g., result.Ensure, option.Filter)
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        
        // Check if this is accessing .Ensure or .Filter property
        var memberName = memberAccess.Name.Identifier.Text;
        if (memberName != "Ensure" && memberName != "Filter")
            return;
            
        // Get the type info to check if it returns a builder
        var typeInfo = context.SemanticModel.GetTypeInfo(memberAccess);
        var returnType = typeInfo.Type;
        
        if (returnType == null)
            return;
            
        // Check if this returns a builder type
        var (isBuilder, builderType) = GetBuilderType(returnType);
        if (!isBuilder)
            return;
            
        // Check if the builder is properly used (i.e., a method is called on it)
        if (!IsBuilderProperlyUsed(memberAccess, context.SemanticModel, builderType))
        {
            var diagnostic = builderType == BuilderType.Ensure
                ? Diagnostic.Create(EnsureBuilderRule, memberAccess.GetLocation())
                : Diagnostic.Create(FilterBuilderRule, memberAccess.GetLocation());
                
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

    private static bool IsBuilderProperlyUsed(MemberAccessExpressionSyntax builderAccess, 
        SemanticModel semanticModel, BuilderType builderType)
    {
        var parent = builderAccess.Parent;
        
        // Check if it's immediately followed by a method call
        if (parent is MemberAccessExpressionSyntax parentMemberAccess && 
            parentMemberAccess.Expression == builderAccess)
        {
            // The builder is being used to access a member - this is good if the member is a method that will be invoked
            var grandParent = parentMemberAccess.Parent;
            if (grandParent is InvocationExpressionSyntax)
            {
                // A method is being called on the builder - this completes it
                return true;
            }
        }
        
        // Check specific misuse patterns
        return !IsMisused(builderAccess, semanticModel, builderType);
    }


    private static bool IsMisused(MemberAccessExpressionSyntax builderAccess, SemanticModel semanticModel, BuilderType builderType)
    {
        var parent = builderAccess.Parent;
        
        // Assignment to variable
        if (parent is EqualsValueClauseSyntax)
            return true;
            
        // Assignment expression
        if (parent is AssignmentExpressionSyntax)
            return true;
            
        // Return statement
        if (parent is ReturnStatementSyntax)
            return true;
        
        // Expression-bodied member (especially void methods)
        if (parent is ArrowExpressionClauseSyntax arrowClause)
        {
            var memberDeclaration = arrowClause.Parent;
            
            // Always consider it misuse in expression-bodied members
            // unless it's immediately followed by a method call
            return true;
        }
        
        // Passed as argument
        if (parent is ArgumentSyntax)
            return true;
        
        // Expression statement (builder accessed but no method called)
        if (parent is ExpressionStatementSyntax)
            return true;
            
        // Await expression
        if (parent is AwaitExpressionSyntax)
            return true;
            
        // Used in conditional expressions
        if (parent is ConditionalExpressionSyntax or 
            BinaryExpressionSyntax or 
            PrefixUnaryExpressionSyntax or
            PostfixUnaryExpressionSyntax)
            return true;
        
        // Cast or conversion
        if (parent is CastExpressionSyntax or ParenthesizedExpressionSyntax)
            return true;
        
        // Lambda expression
        if (parent is SimpleLambdaExpressionSyntax or ParenthesizedLambdaExpressionSyntax)
            return true;
        
        return false;
    }


    private enum BuilderType
    {
        None,
        Ensure,
        Filter
    }
}