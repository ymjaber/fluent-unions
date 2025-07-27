using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that suggests using Option.None instead of default expressions when creating empty Option values.
/// </summary>
/// <remarks>
/// This analyzer promotes clarity and intent by detecting uses of default(Option&lt;T&gt;) or default literal
/// expressions and suggesting the more expressive Option.None. The analyzer triggers on:
/// - Explicit default expressions: default(Option&lt;T&gt;)
/// - Default literal expressions: default when the target type is Option&lt;T&gt;
/// 
/// Using Option.None makes the code more readable and clearly expresses the intent to create
/// an empty option value, rather than relying on the default value semantics.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OptionPreferNoneAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for the prefer None rule.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.OptionPreferNoneOverDefault,
        title: "Prefer Option.None over default(Option<T>)",
        messageFormat: "Use Option.None instead of default(Option<{0}>)",
        category: Categories.Design,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Option.None is more expressive and clearer than default(Option<T>).");

    /// <summary>
    /// Gets the list of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions for default expressions.
    /// </summary>
    /// <param name="context">The analysis context used to register actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeDefaultExpression, SyntaxKind.DefaultExpression);
        context.RegisterSyntaxNodeAction(AnalyzeDefaultLiteral, SyntaxKind.DefaultLiteralExpression);
    }

    /// <summary>
    /// Analyzes explicit default expressions like default(Option&lt;T&gt;).
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeDefaultExpression(SyntaxNodeAnalysisContext context)
    {
        var defaultExpression = (DefaultExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(defaultExpression.Type);
        
        CheckAndReportIfOptionType(context, defaultExpression, typeInfo.Type);
    }

    /// <summary>
    /// Analyzes default literal expressions where the target type is Option&lt;T&gt;.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeDefaultLiteral(SyntaxNodeAnalysisContext context)
    {
        var defaultLiteral = (LiteralExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(defaultLiteral);
        
        CheckAndReportIfOptionType(context, defaultLiteral, typeInfo.ConvertedType);
    }

    /// <summary>
    /// Checks if the given type is an Option type and reports a diagnostic if it is.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    /// <param name="expression">The expression to report the diagnostic on.</param>
    /// <param name="type">The type to check.</param>
    private static void CheckAndReportIfOptionType(SyntaxNodeAnalysisContext context, 
        ExpressionSyntax expression, ITypeSymbol? type)
    {
        if (!SymbolHelpers.IsOptionType(type))
            return;

        var typeArg = "T";
        if (type is INamedTypeSymbol namedType && namedType.TypeArguments.Length > 0)
        {
            typeArg = namedType.TypeArguments[0].ToDisplayString();
        }

        var diagnostic = Diagnostic.Create(Rule, expression.GetLocation(), typeArg);
        context.ReportDiagnostic(diagnostic);
    }
}