using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that suggests using the Match method instead of if/else patterns when handling Option types.
/// </summary>
/// <remarks>
/// This analyzer promotes a more functional programming style by detecting if/else statements that check
/// IsSome or IsNone properties and suggesting the use of the Match method instead. The analyzer triggers when:
/// - An if statement checks option.IsSome or option.IsNone (or their negations)
/// - The if statement has an else clause
/// - At least one branch accesses the Option value
/// 
/// The Match method provides better type safety and a more idiomatic approach to handling Option values,
/// ensuring both Some and None cases are handled explicitly.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OptionPreferMatchAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for the prefer Match rule.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.OptionPreferMatchOverIfElse,
        title: "Prefer Match over if/else for Option handling",
        messageFormat: "Consider using Match method instead of if/else pattern for Option<{0}>",
        category: Categories.Design,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "The Match method provides a more idiomatic and functional approach to handling Option values.");

    /// <summary>
    /// Gets the list of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions for if statements.
    /// </summary>
    /// <param name="context">The analysis context used to register actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
    }

    /// <summary>
    /// Analyzes if statements to detect patterns that could be replaced with Match method calls.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
    {
        var ifStatement = (IfStatementSyntax)context.Node;
        
        // Check if the condition is checking IsSome or IsNone
        var optionInfo = GetOptionCheckInfo(ifStatement.Condition, context.SemanticModel);
        if (optionInfo == null)
            return;

        // Check if there's an else clause
        if (ifStatement.Else == null)
            return;

        // Both branches should handle the Option value
        if (!BothBranchesHandleOption(ifStatement, ifStatement.Else, optionInfo.Expression, context.SemanticModel))
            return;

        var typeArg = GetOptionTypeArgument(optionInfo.Expression, context.SemanticModel);
        var diagnostic = Diagnostic.Create(
            Rule, 
            ifStatement.GetLocation(),
            typeArg?.ToDisplayString() ?? "T");
            
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines if the condition expression is checking an Option's IsSome or IsNone property.
    /// </summary>
    /// <param name="condition">The condition expression to analyze.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>Information about the Option check if found; otherwise, null.</returns>
    private static OptionCheckInfo? GetOptionCheckInfo(ExpressionSyntax condition, SemanticModel semanticModel)
    {
        if (condition is MemberAccessExpressionSyntax memberAccess)
        {
            var propertyName = memberAccess.Name.Identifier.Text;
            if (propertyName == "IsSome" || propertyName == "IsNone")
            {
                var symbol = semanticModel.GetSymbolInfo(memberAccess).Symbol as IPropertySymbol;
                if (symbol != null && SymbolHelpers.IsOptionType(symbol.ContainingType))
                {
                    return new OptionCheckInfo
                    {
                        Expression = memberAccess.Expression,
                        IsCheckingIsSome = propertyName == "IsSome"
                    };
                }
            }
        }
        
        // Handle negation: !option.IsSome or !option.IsNone
        if (condition is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.LogicalNotExpression } notExpr)
        {
            var innerInfo = GetOptionCheckInfo(notExpr.Operand, semanticModel);
            if (innerInfo != null)
            {
                return new OptionCheckInfo
                {
                    Expression = innerInfo.Expression,
                    IsCheckingIsSome = !innerInfo.IsCheckingIsSome
                };
            }
        }
        
        return null;
    }

    /// <summary>
    /// Determines whether both branches of an if/else statement access the Option value.
    /// </summary>
    /// <param name="ifStatement">The if statement to analyze.</param>
    /// <param name="elseClause">The else clause to analyze.</param>
    /// <param name="optionExpression">The Option expression being checked.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if at least one branch accesses the Option; otherwise, false.</returns>
    private static bool BothBranchesHandleOption(IfStatementSyntax ifStatement, ElseClauseSyntax elseClause, 
        ExpressionSyntax optionExpression, SemanticModel semanticModel)
    {
        // Check if the true branch accesses the Option
        var trueBranchAccesses = ifStatement.Statement.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(ma => IsAccessingOption(ma, optionExpression, semanticModel));

        // Check if the false branch accesses the Option
        var falseBranchAccesses = elseClause.Statement.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(ma => IsAccessingOption(ma, optionExpression, semanticModel));

        return trueBranchAccesses || falseBranchAccesses;
    }

    /// <summary>
    /// Determines whether a member access expression is accessing the specified Option.
    /// </summary>
    /// <param name="memberAccess">The member access expression to check.</param>
    /// <param name="optionExpression">The Option expression to compare against.</param>
    /// <param name="semanticModel">The semantic model for symbol resolution.</param>
    /// <returns>true if the member access is accessing the specified Option; otherwise, false.</returns>
    private static bool IsAccessingOption(MemberAccessExpressionSyntax memberAccess, 
        ExpressionSyntax optionExpression, SemanticModel semanticModel)
    {
        var accessedSymbol = semanticModel.GetSymbolInfo(memberAccess.Expression).Symbol;
        var optionSymbol = semanticModel.GetSymbolInfo(optionExpression).Symbol;
        
        return accessedSymbol != null && SymbolEqualityComparer.Default.Equals(accessedSymbol, optionSymbol);
    }

    /// <summary>
    /// Gets the type argument of an Option&lt;T&gt; type.
    /// </summary>
    /// <param name="expression">The expression of Option type.</param>
    /// <param name="semanticModel">The semantic model for type information.</param>
    /// <returns>The type argument T from Option&lt;T&gt;; otherwise, null.</returns>
    private static ITypeSymbol? GetOptionTypeArgument(ExpressionSyntax expression, SemanticModel semanticModel)
    {
        var type = semanticModel.GetTypeInfo(expression).Type;
        if (type is INamedTypeSymbol namedType && SymbolHelpers.IsOptionType(namedType))
        {
            return namedType.TypeArguments.FirstOrDefault();
        }
        return null;
    }

    /// <summary>
    /// Contains information about an Option check in a condition.
    /// </summary>
    private class OptionCheckInfo
    {
        /// <summary>
        /// Gets or sets the Option expression being checked.
        /// </summary>
        public ExpressionSyntax Expression { get; set; } = null!;
        
        /// <summary>
        /// Gets or sets a value indicating whether the check is for IsSome (true) or IsNone (false).
        /// </summary>
        public bool IsCheckingIsSome { get; set; }
    }
}