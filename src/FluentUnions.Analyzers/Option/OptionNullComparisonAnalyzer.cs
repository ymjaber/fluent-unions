using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that detects null comparisons with Option&lt;T&gt; types and suggests using IsNone or IsSome instead.
/// </summary>
/// <remarks>
/// Since Option&lt;T&gt; is a value type (struct), it can never be null. This analyzer detects common mistakes
/// where developers treat Option like a reference type and compare it with null. The analyzer will trigger on:
/// - Direct null comparisons: option == null, option != null
/// - Pattern matching with null: option is null
/// 
/// The analyzer suggests using the appropriate Option properties:
/// - Use IsNone to check if the option has no value
/// - Use IsSome to check if the option has a value
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OptionNullComparisonAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for the null comparison rule.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.OptionNullComparison,
        title: "Option<T> should not be compared with null",
        messageFormat: "Option<T> is a value type and will never be null. Use IsNone or IsSome instead.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Option<T> is a struct and cannot be null. Use IsNone to check for absence of value.");

    /// <summary>
    /// Gets the list of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions for relevant syntax nodes.
    /// </summary>
    /// <param name="context">The analysis context used to register actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeBinaryExpression, 
            SyntaxKind.EqualsExpression, 
            SyntaxKind.NotEqualsExpression);
        context.RegisterSyntaxNodeAction(AnalyzeIsPattern, 
            SyntaxKind.IsPatternExpression);
    }

    /// <summary>
    /// Analyzes binary expressions (== and !=) to detect null comparisons with Option types.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
    {
        var binaryExpression = (BinaryExpressionSyntax)context.Node;
        
        // Check if either side is null
        var leftIsNull = IsNullLiteral(binaryExpression.Left);
        var rightIsNull = IsNullLiteral(binaryExpression.Right);
        
        if (!leftIsNull && !rightIsNull)
            return;

        // Get the non-null expression
        var expression = leftIsNull ? binaryExpression.Right : binaryExpression.Left;
        
        // Check if it's an Option<T> type
        var typeInfo = context.SemanticModel.GetTypeInfo(expression);
        if (!SymbolHelpers.IsOptionType(typeInfo.Type))
            return;

        var diagnostic = Diagnostic.Create(Rule, binaryExpression.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Analyzes 'is' pattern expressions to detect null pattern matching with Option types.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeIsPattern(SyntaxNodeAnalysisContext context)
    {
        var isPattern = (IsPatternExpressionSyntax)context.Node;
        
        // Check if pattern is null
        if (isPattern.Pattern is not ConstantPatternSyntax { Expression: LiteralExpressionSyntax literal } || 
            !IsNullLiteral(literal))
            return;

        // Check if expression is Option<T>
        var typeInfo = context.SemanticModel.GetTypeInfo(isPattern.Expression);
        if (!SymbolHelpers.IsOptionType(typeInfo.Type))
            return;

        var diagnostic = Diagnostic.Create(Rule, isPattern.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether the specified expression is a null literal.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <returns>true if the expression is a null literal; otherwise, false.</returns>
    private static bool IsNullLiteral(ExpressionSyntax expression)
    {
        return expression is LiteralExpressionSyntax literal && 
               literal.IsKind(SyntaxKind.NullLiteralExpression);
    }
}