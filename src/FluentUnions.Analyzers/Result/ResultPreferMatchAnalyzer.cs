using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Result;

/// <summary>
/// Analyzer that suggests using the Match method instead of if/else patterns for Result handling.
/// </summary>
/// <remarks>
/// This analyzer promotes a more functional programming style by detecting if/else patterns
/// that check IsSuccess or IsFailure properties and suggesting the use of the Match method instead.
/// The Match method provides a more idiomatic way to handle both success and failure cases
/// with better type safety and readability.
/// 
/// Example of code that triggers this diagnostic:
/// <code>
/// if (result.IsSuccess)
/// {
///     Console.WriteLine($"Success: {result.Value}");
/// }
/// else
/// {
///     Console.WriteLine($"Error: {result.Error}");
/// }
/// </code>
/// 
/// Suggested improvement:
/// <code>
/// result.Match(
///     success: value => Console.WriteLine($"Success: {value}"),
///     failure: error => Console.WriteLine($"Error: {error}")
/// );
/// </code>
/// 
/// The analyzer only triggers when:
/// - The if condition checks IsSuccess or IsFailure (or their negations)
/// - There is an else clause
/// - At least one branch accesses Result.Value or Result.Error
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultPreferMatchAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for suggesting Match over if/else patterns.
    /// </summary>
    /// <remarks>
    /// This rule produces info-level diagnostic FU0103 to suggest a more idiomatic approach.
    /// It's an informational suggestion rather than a warning, as if/else patterns are still valid.
    /// </remarks>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.ResultPreferMatchOverIfElse,
        title: "Prefer Match over if/else for Result handling",
        messageFormat: "Consider using Match method instead of if/else pattern for {0}",
        category: Categories.Design,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "The Match method provides a more idiomatic and functional approach to handling Result values.");

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
        context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
    }

    /// <summary>
    /// Analyzes if statements to detect if/else patterns that could be replaced with Match.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the if statement to analyze.</param>
    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
    {
        var ifStatement = (IfStatementSyntax)context.Node;

        // Check if the condition is checking IsSuccess or IsFailure
        var resultInfo = GetResultCheckInfo(ifStatement.Condition, context.SemanticModel);
        if (resultInfo == null)
            return;

        // Check if there's an else clause
        if (ifStatement.Else == null)
            return;

        // Both branches should handle the Result
        if (!BothBranchesHandleResult(ifStatement, ifStatement.Else, resultInfo.Expression, context.SemanticModel))
            return;

        var resultType = GetResultTypeString(resultInfo.Expression, context.SemanticModel);
        var diagnostic = Diagnostic.Create(
            Rule,
            ifStatement.GetLocation(),
            resultType);

        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Extracts information about Result property checks from a condition expression.
    /// </summary>
    /// <param name="condition">The condition expression to analyze.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>Information about the Result check if found; otherwise, null.</returns>
    /// <remarks>
    /// This method handles both direct property access (result.IsSuccess, result.IsFailure)
    /// and negated forms (!result.IsSuccess, !result.IsFailure).
    /// </remarks>
    private static ResultCheckInfo? GetResultCheckInfo(ExpressionSyntax condition, SemanticModel semanticModel)
    {
        if (condition is MemberAccessExpressionSyntax memberAccess)
        {
            var propertyName = memberAccess.Name.Identifier.Text;
            if (propertyName == "IsSuccess" || propertyName == "IsFailure")
            {
                var symbol = semanticModel.GetSymbolInfo(memberAccess).Symbol as IPropertySymbol;
                if (symbol != null && SymbolHelpers.IsResultType(symbol.ContainingType))
                {
                    return new ResultCheckInfo
                    {
                        Expression = memberAccess.Expression,
                        IsCheckingIsSuccess = propertyName == "IsSuccess"
                    };
                }
            }
        }

        // Handle negation: !result.IsSuccess or !result.IsFailure
        if (condition is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalNotExpression } notExpr)
        {
            var innerInfo = GetResultCheckInfo(notExpr.Operand, semanticModel);
            if (innerInfo != null)
            {
                return new ResultCheckInfo
                {
                    Expression = innerInfo.Expression,
                    IsCheckingIsSuccess = !innerInfo.IsCheckingIsSuccess
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Determines if both branches of an if/else statement handle the Result value.
    /// </summary>
    /// <param name="ifStatement">The if statement to analyze.</param>
    /// <param name="elseClause">The else clause to analyze.</param>
    /// <param name="resultExpression">The Result expression being checked.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if at least one branch accesses Result.Value or Result.Error; otherwise, false.</returns>
    private static bool BothBranchesHandleResult(IfStatementSyntax ifStatement, ElseClauseSyntax elseClause,
        ExpressionSyntax resultExpression, SemanticModel semanticModel)
    {
        // Check if the true branch accesses Value or Error
        var trueBranchAccesses = ifStatement.Statement.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(ma => IsAccessingResult(ma, resultExpression, semanticModel));

        // Check if the false branch accesses Value or Error
        var falseBranchAccesses = elseClause.Statement.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(ma => IsAccessingResult(ma, resultExpression, semanticModel));

        return trueBranchAccesses || falseBranchAccesses;
    }

    /// <summary>
    /// Checks if a member access expression is accessing Value or Error properties of a specific Result instance.
    /// </summary>
    /// <param name="memberAccess">The member access expression to check.</param>
    /// <param name="resultExpression">The Result expression to match against.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>True if the member access is accessing Value or Error on the same Result instance; otherwise, false.</returns>
    private static bool IsAccessingResult(MemberAccessExpressionSyntax memberAccess,
        ExpressionSyntax resultExpression, SemanticModel semanticModel)
    {
        var memberName = memberAccess.Name.Identifier.Text;
        if (memberName != "Value" && memberName != "Error")
            return false;

        var accessedSymbol = semanticModel.GetSymbolInfo(memberAccess.Expression).Symbol;
        var resultSymbol = semanticModel.GetSymbolInfo(resultExpression).Symbol;

        return accessedSymbol != null && SymbolEqualityComparer.Default.Equals(accessedSymbol, resultSymbol);
    }

    /// <summary>
    /// Gets a display string for the Result type of an expression.
    /// </summary>
    /// <param name="expression">The expression whose type to get.</param>
    /// <param name="semanticModel">The semantic model for type information.</param>
    /// <returns>A string representation of the Result type (e.g., "Result&lt;int&gt;" or "Result").</returns>
    private static string GetResultTypeString(ExpressionSyntax expression, SemanticModel semanticModel)
    {
        var type = semanticModel.GetTypeInfo(expression).Type;
        if (type is INamedTypeSymbol namedType)
        {
            if (SymbolHelpers.IsValueResultType(namedType))
            {
                var typeArg = namedType.TypeArguments.FirstOrDefault();
                return $"Result<{typeArg?.ToDisplayString() ?? "T"}>";
            }
            else if (SymbolHelpers.IsUnitResultType(namedType))
            {
                return "Result";
            }
        }
        return "Result";
    }

    /// <summary>
    /// Holds information about a Result property check in a condition.
    /// </summary>
    private class ResultCheckInfo
    {
        /// <summary>
        /// Gets or sets the expression that represents the Result instance being checked.
        /// </summary>
        public ExpressionSyntax Expression { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the check is for IsSuccess (true) or IsFailure (false).
        /// </summary>
        public bool IsCheckingIsSuccess { get; set; }
    }
}
