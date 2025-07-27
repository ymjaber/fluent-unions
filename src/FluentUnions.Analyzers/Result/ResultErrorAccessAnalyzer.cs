using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Result;

/// <summary>
/// Analyzer that detects unsafe access to Result.Error property without proper failure checks.
/// </summary>
/// <remarks>
/// This analyzer helps prevent runtime InvalidOperationException errors by ensuring that 
/// Result.Error is only accessed after verifying that IsFailure is true (or IsSuccess is false). 
/// The analyzer recognizes several safe patterns including:
/// - Access within if (result.IsFailure) blocks
/// - Access within if (!result.IsSuccess) blocks
/// - Access within the failure branch of conditional expressions
/// - Access within Match method failure callbacks (second argument)
/// 
/// Example of unsafe code that triggers this diagnostic:
/// <code>
/// Result&lt;int&gt; result = GetResult();
/// string error = result.Error; // Warning: accessing Error without checking IsFailure
/// </code>
/// 
/// Example of safe code:
/// <code>
/// Result&lt;int&gt; result = GetResult();
/// if (result.IsFailure)
/// {
///     string error = result.Error; // Safe: checked IsFailure first
/// }
/// 
/// // Or using IsSuccess negation:
/// if (!result.IsSuccess)
/// {
///     string error = result.Error; // Safe: verified failure state
/// }
/// </code>
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultErrorAccessAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for unsafe Result.Error access.
    /// </summary>
    /// <remarks>
    /// This rule produces warning FU0102 when Result.Error is accessed without proper checks.
    /// The warning can be suppressed if the developer is certain the access is safe.
    /// </remarks>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.ResultErrorAccessWithoutCheck,
        title: "Avoid accessing Result.Error without checking IsFailure",
        messageFormat: "Result.Error is accessed without checking IsFailure. This may throw an InvalidOperationException.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Result.Error should only be accessed after verifying IsFailure is true to avoid runtime exceptions.");

    /// <summary>
    /// Gets the collection of diagnostics supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering actions to be performed during compilation.
    /// </summary>
    /// <param name="context">The analysis context used to register analyzer actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    /// <summary>
    /// Analyzes member access expressions to detect unsafe Result.Error access.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the member access expression to analyze.</param>
    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        
        // Check if accessing "Error" property
        if (memberAccess.Name.Identifier.Text != "Error")
            return;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
        if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
            return;

        // Check if it's Result.Error or Result<T>.Error
        if (!SymbolHelpers.IsResultType(propertySymbol.ContainingType))
            return;

        // Check if we're in a safe context (after IsFailure check)
        if (IsInSafeContext(memberAccess, context.SemanticModel))
            return;

        var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether a Result.Error access is in a safe context where IsFailure has been checked.
    /// </summary>
    /// <param name="errorAccess">The member access expression for Result.Error.</param>
    /// <param name="semanticModel">The semantic model for the compilation.</param>
    /// <returns>True if the access is in a safe context; otherwise, false.</returns>
    /// <remarks>
    /// This method checks various patterns to determine safety:
    /// - If statements with IsFailure conditions
    /// - If statements with negated IsSuccess conditions
    /// - Conditional expressions with failure checks
    /// - Inside Match method failure callbacks (second argument)
    /// - After IsSuccess checks that exit the control flow (throw, return, etc.)
    /// </remarks>
    private static bool IsInSafeContext(MemberAccessExpressionSyntax errorAccess, SemanticModel semanticModel)
    {
        var currentNode = errorAccess.Parent;
        
        while (currentNode != null)
        {
            // Check if we're inside an if statement that checks IsFailure
            if (currentNode is IfStatementSyntax ifStatement)
            {
                if (HasIsFailureCheck(ifStatement.Condition, errorAccess.Expression, semanticModel))
                    return true;
            }
            
            // Check if we're inside a conditional expression that checks IsFailure
            if (currentNode is ConditionalExpressionSyntax conditional)
            {
                if (HasIsFailureCheck(conditional.Condition, errorAccess.Expression, semanticModel))
                {
                    // Make sure we're in the true branch
                    var parent = errorAccess.Parent;
                    while (parent != conditional && parent != null)
                    {
                        if (parent == conditional.WhenFalse)
                            return false;
                        parent = parent.Parent;
                    }
                    return true;
                }
            }
            
            // Check if we're inside a Match method call (in the failure handler)
            if (currentNode is ArgumentSyntax arg && 
                arg.Parent is ArgumentListSyntax argList &&
                argList.Parent is InvocationExpressionSyntax invocation)
            {
                if (IsMatchMethodCall(invocation, semanticModel))
                {
                    // Check if we're in the failure handler (usually second argument)
                    var argIndex = argList.Arguments.IndexOf(arg);
                    if (argIndex == 1)
                        return true;
                }
            }
            
            // Check if we're after an IsSuccess check that exits the method
            if (IsAfterSuccessExit(errorAccess, semanticModel))
                return true;
            
            // Don't look beyond method boundaries
            if (currentNode is MethodDeclarationSyntax or LocalFunctionStatementSyntax or LambdaExpressionSyntax)
                break;
                
            currentNode = currentNode.Parent;
        }
        
        return false;
    }

    /// <summary>
    /// Checks if an expression contains an IsFailure property check or negated IsSuccess check for the given result expression.
    /// </summary>
    /// <param name="condition">The condition expression to examine.</param>
    /// <param name="resultExpression">The result expression to match against.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the condition checks failure state on the same result instance; otherwise, false.</returns>
    /// <remarks>
    /// This method recognizes both direct IsFailure checks and negated IsSuccess checks (!result.IsSuccess).
    /// </remarks>
    private static bool HasIsFailureCheck(ExpressionSyntax condition, ExpressionSyntax resultExpression, SemanticModel semanticModel)
    {
        // Handle direct IsFailure property access
        if (condition is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "IsFailure")
        {
            return AreExpressionsEquivalent(memberAccess.Expression, resultExpression, semanticModel);
        }
        
        // Handle negated IsSuccess (!result.IsSuccess)
        if (condition is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalNotExpression } notExpr &&
            notExpr.Operand is MemberAccessExpressionSyntax innerMemberAccess &&
            innerMemberAccess.Name.Identifier.Text == "IsSuccess")
        {
            return AreExpressionsEquivalent(innerMemberAccess.Expression, resultExpression, semanticModel);
        }
        
        // Handle logical AND conditions
        if (condition is BinaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalAndExpression } binaryExpr)
        {
            return HasIsFailureCheck(binaryExpr.Left, resultExpression, semanticModel) ||
                   HasIsFailureCheck(binaryExpr.Right, resultExpression, semanticModel);
        }
        
        return false;
    }

    /// <summary>
    /// Determines if two expressions refer to the same symbol (variable, property, etc.).
    /// </summary>
    /// <param name="expr1">The first expression to compare.</param>
    /// <param name="expr2">The second expression to compare.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if both expressions refer to the same symbol; otherwise, false.</returns>
    private static bool AreExpressionsEquivalent(ExpressionSyntax expr1, ExpressionSyntax expr2, SemanticModel semanticModel)
    {
        var symbol1 = semanticModel.GetSymbolInfo(expr1).Symbol;
        var symbol2 = semanticModel.GetSymbolInfo(expr2).Symbol;
        
        return symbol1 != null && SymbolEqualityComparer.Default.Equals(symbol1, symbol2);
    }

    /// <summary>
    /// Checks if an invocation is a call to the Match method on a Result type.
    /// </summary>
    /// <param name="invocation">The invocation expression to check.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the invocation is a Result.Match method call; otherwise, false.</returns>
    /// <remarks>
    /// In Match method calls, the Error property can be safely accessed within the failure handler (typically the second argument).
    /// </remarks>
    private static bool IsMatchMethodCall(InvocationExpressionSyntax invocation, SemanticModel semanticModel)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return false;
            
        if (memberAccess.Name.Identifier.Text != "Match")
            return false;
            
        var methodSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        return methodSymbol?.ContainingType != null && 
               SymbolHelpers.IsResultType(methodSymbol.ContainingType);
    }

    /// <summary>
    /// Determines if a Result.Error access occurs after an IsSuccess check that exits the current control flow.
    /// </summary>
    /// <param name="errorAccess">The member access expression for Result.Error.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the access is after an IsSuccess check with control flow exit; otherwise, false.</returns>
    /// <remarks>
    /// This method recognizes patterns like:
    /// if (result.IsSuccess) return;
    /// var error = result.Error; // Safe because success case exits
    /// </remarks>
    private static bool IsAfterSuccessExit(MemberAccessExpressionSyntax errorAccess, SemanticModel semanticModel)
    {
        var block = errorAccess.FirstAncestorOrSelf<BlockSyntax>();
        if (block == null)
        {
            // Check if we're in a top-level program
            var compilation = errorAccess.FirstAncestorOrSelf<CompilationUnitSyntax>();
            if (compilation != null)
            {
                return CheckForSuccessExitInStatements(compilation.Members.OfType<GlobalStatementSyntax>()
                    .SelectMany(g => g.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>()), 
                    errorAccess, semanticModel);
            }
            return false;
        }

        return CheckForSuccessExitInStatements(block.Statements, errorAccess, semanticModel);
    }

    /// <summary>
    /// Checks if there's an IsSuccess check with control flow exit before the error access in a sequence of statements.
    /// </summary>
    private static bool CheckForSuccessExitInStatements(IEnumerable<StatementSyntax> statements, 
        MemberAccessExpressionSyntax errorAccess, SemanticModel semanticModel)
    {
        var errorAccessPosition = errorAccess.SpanStart;
        
        foreach (var statement in statements)
        {
            if (statement.SpanStart >= errorAccessPosition)
                break;

            // Check for if (result.IsSuccess) with exit statement
            if (statement is IfStatementSyntax ifStatement)
            {
                if (HasIsSuccessCheck(ifStatement.Condition, errorAccess.Expression, semanticModel))
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
    /// Checks if an expression contains an IsSuccess property check for the given result expression.
    /// </summary>
    private static bool HasIsSuccessCheck(ExpressionSyntax condition, ExpressionSyntax resultExpression, SemanticModel semanticModel)
    {
        // Handle direct IsSuccess property access
        if (condition is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "IsSuccess")
        {
            return AreExpressionsEquivalent(memberAccess.Expression, resultExpression, semanticModel);
        }
        
        // Handle negated IsFailure (!result.IsFailure)
        if (condition is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalNotExpression } notExpr &&
            notExpr.Operand is MemberAccessExpressionSyntax innerMemberAccess &&
            innerMemberAccess.Name.Identifier.Text == "IsFailure")
        {
            return AreExpressionsEquivalent(innerMemberAccess.Expression, resultExpression, semanticModel);
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