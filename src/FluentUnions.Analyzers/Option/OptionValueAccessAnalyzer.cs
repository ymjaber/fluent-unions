using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that detects potentially unsafe access to Option.Value property without proper null checks.
/// </summary>
/// <remarks>
/// This analyzer helps prevent runtime InvalidOperationException by detecting when code accesses
/// the Value property of an Option without first verifying that the option contains a value.
/// The analyzer considers access safe when:
/// - It's inside an if statement that checks IsSome
/// - It's in the true branch of a conditional expression that checks IsSome
/// - It's inside a Match method call
/// - It's preceded by a logical AND condition that includes an IsSome check
/// 
/// This analyzer is crucial for maintaining runtime safety when working with Option types.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OptionValueAccessAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for the unsafe value access rule.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.OptionValueAccessWithoutCheck,
        title: "Avoid accessing Option.Value without checking IsSome",
        messageFormat: "Option.Value is accessed without checking IsSome. This may throw an InvalidOperationException.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Option.Value should only be accessed after verifying IsSome is true to avoid runtime exceptions.");

    /// <summary>
    /// Gets the list of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions for member access expressions.
    /// </summary>
    /// <param name="context">The analysis context used to register actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    /// <summary>
    /// Analyzes member access expressions to detect unsafe access to Option.Value.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        
        // Check if accessing "Value" property
        if (memberAccess.Name.Identifier.Text != "Value")
            return;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
        if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
            return;

        // Check if it's Option<T>.Value
        if (!SymbolHelpers.IsOptionType(propertySymbol.ContainingType))
            return;

        // Check if we're in a safe context (after IsSome check)
        if (IsInSafeContext(memberAccess, context.SemanticModel))
            return;

        var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether the Value access occurs in a safe context where IsSome has been checked.
    /// </summary>
    /// <param name="valueAccess">The Value property access expression.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if the access is in a safe context; otherwise, false.</returns>
    private static bool IsInSafeContext(MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var currentNode = valueAccess.Parent;
        
        while (currentNode != null)
        {
            // Check if we're inside an if statement that checks IsSome
            if (currentNode is IfStatementSyntax ifStatement)
            {
                if (HasIsSomeCheck(ifStatement.Condition, valueAccess.Expression, semanticModel))
                    return true;
            }
            
            // Check if we're inside a conditional expression that checks IsSome
            if (currentNode is ConditionalExpressionSyntax conditional)
            {
                if (HasIsSomeCheck(conditional.Condition, valueAccess.Expression, semanticModel))
                {
                    // Make sure we're in the true branch
                    var parent = valueAccess.Parent;
                    while (parent != conditional && parent != null)
                    {
                        if (parent == conditional.WhenFalse)
                            return false;
                        parent = parent.Parent;
                    }
                    return true;
                }
            }
            
            // Check if we're inside a Match method call
            if (currentNode is ArgumentSyntax { Parent: ArgumentListSyntax { Parent: InvocationExpressionSyntax invocation } })
            {
                if (IsMatchMethodCall(invocation, semanticModel))
                    return true;
            }
            
            // Check if we're after an IsNone check that exits the method
            if (IsAfterNoneExit(valueAccess, semanticModel))
                return true;
            
            // Don't look beyond method boundaries
            if (currentNode is MethodDeclarationSyntax or LocalFunctionStatementSyntax or LambdaExpressionSyntax)
                break;
                
            currentNode = currentNode.Parent;
        }
        
        return false;
    }

    /// <summary>
    /// Checks if the given condition expression contains an IsSome check for the specified option.
    /// </summary>
    /// <param name="condition">The condition expression to analyze.</param>
    /// <param name="optionExpression">The option expression to look for.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if the condition checks IsSome for the option; otherwise, false.</returns>
    private static bool HasIsSomeCheck(ExpressionSyntax condition, ExpressionSyntax optionExpression, SemanticModel semanticModel)
    {
        // Handle direct IsSome property access
        if (condition is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "IsSome")
        {
            return AreExpressionsEquivalent(memberAccess.Expression, optionExpression, semanticModel);
        }
        
        // Handle logical AND conditions
        if (condition is BinaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalAndExpression } binaryExpr)
        {
            return HasIsSomeCheck(binaryExpr.Left, optionExpression, semanticModel) ||
                   HasIsSomeCheck(binaryExpr.Right, optionExpression, semanticModel);
        }
        
        return false;
    }

    /// <summary>
    /// Determines whether two expressions refer to the same symbol.
    /// </summary>
    /// <param name="expr1">The first expression.</param>
    /// <param name="expr2">The second expression.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if both expressions refer to the same symbol; otherwise, false.</returns>
    private static bool AreExpressionsEquivalent(ExpressionSyntax expr1, ExpressionSyntax expr2, SemanticModel semanticModel)
    {
        var symbol1 = semanticModel.GetSymbolInfo(expr1).Symbol;
        var symbol2 = semanticModel.GetSymbolInfo(expr2).Symbol;
        
        return symbol1 != null && SymbolEqualityComparer.Default.Equals(symbol1, symbol2);
    }

    /// <summary>
    /// Determines whether the invocation is a call to the Option.Match method.
    /// </summary>
    /// <param name="invocation">The invocation expression to check.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if the invocation is an Option.Match call; otherwise, false.</returns>
    private static bool IsMatchMethodCall(InvocationExpressionSyntax invocation, SemanticModel semanticModel)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return false;
            
        if (memberAccess.Name.Identifier.Text != "Match")
            return false;
            
        var methodSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        return methodSymbol?.ContainingType != null && 
               SymbolHelpers.IsOptionType(methodSymbol.ContainingType);
    }

    /// <summary>
    /// Determines if an Option.Value access occurs after an IsNone check that exits the current control flow.
    /// </summary>
    /// <param name="valueAccess">The member access expression for Option.Value.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the access is after an IsNone check with control flow exit; otherwise, false.</returns>
    /// <remarks>
    /// This method recognizes patterns like:
    /// if (option.IsNone) throw new Exception();
    /// var value = option.Value; // Safe because none case exits
    /// </remarks>
    private static bool IsAfterNoneExit(MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var block = valueAccess.FirstAncestorOrSelf<BlockSyntax>();
        if (block == null)
        {
            // Check if we're in a top-level program
            var compilation = valueAccess.FirstAncestorOrSelf<CompilationUnitSyntax>();
            if (compilation != null)
            {
                return CheckForNoneExitInStatements(compilation.Members.OfType<GlobalStatementSyntax>()
                    .SelectMany(g => g.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>()), 
                    valueAccess, semanticModel);
            }
            return false;
        }

        return CheckForNoneExitInStatements(block.Statements, valueAccess, semanticModel);
    }

    /// <summary>
    /// Checks if there's an IsNone check with control flow exit before the value access in a sequence of statements.
    /// </summary>
    private static bool CheckForNoneExitInStatements(IEnumerable<StatementSyntax> statements, 
        MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var valueAccessPosition = valueAccess.SpanStart;
        
        foreach (var statement in statements)
        {
            if (statement.SpanStart >= valueAccessPosition)
                break;

            // Check for if (option.IsNone) with exit statement
            if (statement is IfStatementSyntax ifStatement)
            {
                if (HasIsNoneCheck(ifStatement.Condition, valueAccess.Expression, semanticModel))
                {
                    // Check if the if body contains an exit statement
                    if (ContainsExitStatement(ifStatement.Statement))
                    {
                        // Check that there's no else clause or the else also exits
                        if (ifStatement.Else == null || ContainsExitStatement(ifStatement.Else.Statement))
                            return true;
                    }
                }
            }
        }
        
        return false;
    }

    /// <summary>
    /// Checks if an expression contains an IsNone property check for the given option expression.
    /// </summary>
    private static bool HasIsNoneCheck(ExpressionSyntax condition, ExpressionSyntax optionExpression, SemanticModel semanticModel)
    {
        // Handle direct IsNone property access
        if (condition is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "IsNone")
        {
            return AreExpressionsEquivalent(memberAccess.Expression, optionExpression, semanticModel);
        }
        
        // Handle negated IsSome (!option.IsSome)
        if (condition is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalNotExpression } notExpr &&
            notExpr.Operand is MemberAccessExpressionSyntax innerMemberAccess &&
            innerMemberAccess.Name.Identifier.Text == "IsSome")
        {
            return AreExpressionsEquivalent(innerMemberAccess.Expression, optionExpression, semanticModel);
        }
        
        return false;
    }

    /// <summary>
    /// Determines if a statement contains a control flow exit (throw, return, break, continue).
    /// </summary>
    private static bool ContainsExitStatement(StatementSyntax statement)
    {
        return statement switch
        {
            ThrowStatementSyntax => true,
            ReturnStatementSyntax => true,
            BreakStatementSyntax => true,
            ContinueStatementSyntax => true,
            BlockSyntax block => block.Statements.Any(ContainsExitStatement),
            _ => false
        };
    }
}