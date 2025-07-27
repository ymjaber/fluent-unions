using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Result;

/// <summary>
/// Analyzer that detects unsafe access to Result.Value property without proper success checks.
/// </summary>
/// <remarks>
/// This analyzer helps prevent runtime InvalidOperationException errors by ensuring that 
/// Result.Value is only accessed after verifying that IsSuccess is true. The analyzer
/// recognizes several safe patterns including:
/// - Access within if (result.IsSuccess) blocks
/// - Access within the success branch of conditional expressions
/// - Access after calling ThrowIfFailure()
/// - Access within Match method callbacks
/// 
/// Example of unsafe code that triggers this diagnostic:
/// <code>
/// Result&lt;int&gt; result = GetResult();
/// int value = result.Value; // Warning: accessing Value without checking IsSuccess
/// </code>
/// 
/// Example of safe code:
/// <code>
/// Result&lt;int&gt; result = GetResult();
/// if (result.IsSuccess)
/// {
///     int value = result.Value; // Safe: checked IsSuccess first
/// }
/// </code>
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultValueAccessAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for unsafe Result.Value access.
    /// </summary>
    /// <remarks>
    /// This rule produces warning FU0101 when Result.Value is accessed without proper checks.
    /// The warning can be suppressed if the developer is certain the access is safe.
    /// </remarks>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.ResultValueAccessWithoutCheck,
        title: "Avoid accessing Result.Value without checking IsSuccess",
        messageFormat: "Result.Value is accessed without checking IsSuccess. This may throw an InvalidOperationException.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Result.Value should only be accessed after verifying IsSuccess is true to avoid runtime exceptions.");

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
    /// Analyzes member access expressions to detect unsafe Result.Value access.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the member access expression to analyze.</param>
    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;
        
        // Check if accessing "Value" property
        if (memberAccess.Name.Identifier.Text != "Value")
            return;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(memberAccess);
        if (symbolInfo.Symbol is not IPropertySymbol propertySymbol)
            return;

        // Check if it's Result<T>.Value
        if (!SymbolHelpers.IsValueResultType(propertySymbol.ContainingType))
            return;

        // Check if we're in a safe context (after IsSuccess check)
        if (IsInSafeContext(memberAccess, context.SemanticModel))
            return;

        var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether a Result.Value access is in a safe context where IsSuccess has been checked.
    /// </summary>
    /// <param name="valueAccess">The member access expression for Result.Value.</param>
    /// <param name="semanticModel">The semantic model for the compilation.</param>
    /// <returns>True if the access is in a safe context; otherwise, false.</returns>
    /// <remarks>
    /// This method checks various patterns to determine safety:
    /// - If statements with IsSuccess conditions
    /// - Conditional expressions with IsSuccess checks
    /// - Inside Match method callbacks
    /// - After ThrowIfFailure method calls
    /// - After IsFailure checks that exit the control flow (throw, return, etc.)
    /// </remarks>
    private static bool IsInSafeContext(MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var currentNode = valueAccess.Parent;
        
        while (currentNode != null)
        {
            // Check if we're inside an if statement that checks IsSuccess
            if (currentNode is IfStatementSyntax ifStatement)
            {
                if (HasIsSuccessCheck(ifStatement.Condition, valueAccess.Expression, semanticModel))
                    return true;
            }
            
            // Check if we're inside a conditional expression that checks IsSuccess
            if (currentNode is ConditionalExpressionSyntax conditional)
            {
                if (HasIsSuccessCheck(conditional.Condition, valueAccess.Expression, semanticModel))
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
            
            // Check if we're after a ThrowIfFailure call
            if (IsAfterThrowIfFailureCall(valueAccess, semanticModel))
                return true;
            
            // Check if we're after an IsFailure check that exits the method
            if (IsAfterFailureExit(valueAccess, semanticModel))
                return true;
            
            // Don't look beyond method boundaries
            if (currentNode is MethodDeclarationSyntax or LocalFunctionStatementSyntax or LambdaExpressionSyntax)
                break;
                
            currentNode = currentNode.Parent;
        }
        
        return false;
    }

    /// <summary>
    /// Checks if an expression contains an IsSuccess property check for the given result expression.
    /// </summary>
    /// <param name="condition">The condition expression to examine.</param>
    /// <param name="resultExpression">The result expression to match against.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the condition checks IsSuccess on the same result instance; otherwise, false.</returns>
    private static bool HasIsSuccessCheck(ExpressionSyntax condition, ExpressionSyntax resultExpression, SemanticModel semanticModel)
    {
        // Handle direct IsSuccess property access
        if (condition is MemberAccessExpressionSyntax memberAccess &&
            memberAccess.Name.Identifier.Text == "IsSuccess")
        {
            return AreExpressionsEquivalent(memberAccess.Expression, resultExpression, semanticModel);
        }
        
        // Handle logical AND conditions
        if (condition is BinaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalAndExpression } binaryExpr)
        {
            return HasIsSuccessCheck(binaryExpr.Left, resultExpression, semanticModel) ||
                   HasIsSuccessCheck(binaryExpr.Right, resultExpression, semanticModel);
        }
        
        return false;
    }

    /// <summary>
    /// Determines if a Result.Value access occurs after a ThrowIfFailure method call on the same result instance.
    /// </summary>
    /// <param name="valueAccess">The member access expression for Result.Value.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the access is after a ThrowIfFailure call; otherwise, false.</returns>
    /// <remarks>
    /// ThrowIfFailure ensures the result is successful or throws an exception, making subsequent Value access safe.
    /// </remarks>
    private static bool IsAfterThrowIfFailureCall(MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var block = valueAccess.FirstAncestorOrSelf<BlockSyntax>();
        if (block == null)
            return false;

        var valueAccessPosition = valueAccess.SpanStart;
        
        // Look for ThrowIfFailure calls before this access
        foreach (var invocation in block.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (invocation.SpanStart >= valueAccessPosition)
                continue;

            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                memberAccess.Name.Identifier.Text == "ThrowIfFailure")
            {
                var methodSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
                if (methodSymbol?.ContainingType != null && 
                    SymbolHelpers.IsResultType(methodSymbol.ContainingType))
                {
                    // Check if the same result instance
                    if (AreExpressionsEquivalent(memberAccess.Expression, valueAccess.Expression, semanticModel))
                        return true;
                }
            }
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
    /// Match method provides safe pattern matching on Result, making Value access within its callbacks safe.
    /// </remarks>
    private static bool IsMatchMethodCall(InvocationExpressionSyntax invocation, SemanticModel semanticModel)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return false;
            
        if (memberAccess.Name.Identifier.Text != "Match")
            return false;
            
        var methodSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        return methodSymbol?.ContainingType != null && 
               SymbolHelpers.IsValueResultType(methodSymbol.ContainingType);
    }

    /// <summary>
    /// Determines if a Result.Value access occurs after an IsFailure check that exits the current control flow.
    /// </summary>
    /// <param name="valueAccess">The member access expression for Result.Value.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the access is after an IsFailure check with control flow exit; otherwise, false.</returns>
    /// <remarks>
    /// This method recognizes patterns like:
    /// if (result.IsFailure) throw new Exception();
    /// var value = result.Value; // Safe because failure case exits
    /// </remarks>
    private static bool IsAfterFailureExit(MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var block = valueAccess.FirstAncestorOrSelf<BlockSyntax>();
        if (block == null)
        {
            // Check if we're in a top-level program
            var compilation = valueAccess.FirstAncestorOrSelf<CompilationUnitSyntax>();
            if (compilation != null)
            {
                return CheckForFailureExitInStatements(compilation.Members.OfType<GlobalStatementSyntax>()
                    .SelectMany(g => g.Statement.DescendantNodesAndSelf().OfType<StatementSyntax>()), 
                    valueAccess, semanticModel);
            }
            return false;
        }

        return CheckForFailureExitInStatements(block.Statements, valueAccess, semanticModel);
    }

    /// <summary>
    /// Checks if there's an IsFailure check with control flow exit before the value access in a sequence of statements.
    /// </summary>
    private static bool CheckForFailureExitInStatements(IEnumerable<StatementSyntax> statements, 
        MemberAccessExpressionSyntax valueAccess, SemanticModel semanticModel)
    {
        var valueAccessPosition = valueAccess.SpanStart;
        
        foreach (var statement in statements)
        {
            if (statement.SpanStart >= valueAccessPosition)
                break;

            // Check for if (result.IsFailure) with exit statement
            if (statement is IfStatementSyntax ifStatement)
            {
                if (HasIsFailureCheck(ifStatement.Condition, valueAccess.Expression, semanticModel))
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
    /// Checks if an expression contains an IsFailure property check for the given result expression.
    /// </summary>
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