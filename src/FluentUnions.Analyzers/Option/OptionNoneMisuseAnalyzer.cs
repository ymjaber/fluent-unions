using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that detects misuse of Option.None when assigned to a non-generic Option variable.
/// </summary>
/// <remarks>
/// This analyzer detects patterns like:
/// - var none = Option.None;
/// - Option none = Option.None;
/// 
/// These patterns create variables of the non-generic Option type, which has no practical use.
/// The analyzer suggests either using Option.None directly in expressions or assigning it to
/// a properly typed Option&lt;T&gt; variable.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OptionNoneMisuseAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for the Option.None misuse rule.
    /// </summary>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.OptionNoneMisuse,
        title: "Avoid assigning Option.None to non-generic Option variable",
        messageFormat: "Option.None should not be assigned to a variable of type Option. Use it directly or assign to Option<T>.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Option.None is meant to be implicitly converted to Option<T>. Assigning it to a non-generic Option variable prevents this conversion and creates a variable with no practical use.");

    /// <summary>
    /// Gets the list of diagnostic descriptors supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering analysis actions.
    /// </summary>
    /// <param name="context">The analysis context used to register actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeVariableDeclaration, SyntaxKind.VariableDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeAssignment, SyntaxKind.SimpleAssignmentExpression);
    }

    /// <summary>
    /// Analyzes variable declarations for Option.None misuse.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
    {
        var variableDeclaration = (VariableDeclarationSyntax)context.Node;

        foreach (var variable in variableDeclaration.Variables)
        {
            if (variable.Initializer == null)
                continue;

            // Check if the initializer is Option.None
            if (!IsOptionNone(variable.Initializer.Value, context.SemanticModel))
                continue;

            // Get the type of the variable
            var variableType = context.SemanticModel.GetTypeInfo(variableDeclaration.Type).Type;
            
            // If it's an implicit type (var), get the actual type from the initializer
            if (variableType == null || variableType.Name == "var")
            {
                variableType = context.SemanticModel.GetTypeInfo(variable.Initializer.Value).Type;
            }

            // Check if it's the non-generic Option type
            if (IsNonGenericOptionType(variableType))
            {
                var diagnostic = Diagnostic.Create(Rule, variable.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    /// <summary>
    /// Analyzes assignment expressions for Option.None misuse.
    /// </summary>
    /// <param name="context">The syntax node analysis context.</param>
    private static void AnalyzeAssignment(SyntaxNodeAnalysisContext context)
    {
        var assignment = (AssignmentExpressionSyntax)context.Node;

        // Check if the right side is Option.None
        if (!IsOptionNone(assignment.Right, context.SemanticModel))
            return;

        // Get the type of the left side
        var leftType = context.SemanticModel.GetTypeInfo(assignment.Left).Type;

        // Check if it's the non-generic Option type
        if (IsNonGenericOptionType(leftType))
        {
            var diagnostic = Diagnostic.Create(Rule, assignment.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }

    /// <summary>
    /// Determines whether the specified expression is Option.None.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="semanticModel">The semantic model.</param>
    /// <returns>true if the expression is Option.None; otherwise, false.</returns>
    private static bool IsOptionNone(ExpressionSyntax expression, SemanticModel semanticModel)
    {
        if (expression is not MemberAccessExpressionSyntax memberAccess)
            return false;

        if (memberAccess.Name.Identifier.Text != "None")
            return false;

        var symbol = semanticModel.GetSymbolInfo(memberAccess).Symbol;
        if (symbol is not IFieldSymbol fieldSymbol)
            return false;

        return fieldSymbol.ContainingType?.Name == "Option" &&
               fieldSymbol.ContainingType?.ContainingNamespace?.ToString() == "FluentUnions" &&
               fieldSymbol.IsStatic &&
               fieldSymbol.Name == "None";
    }

    /// <summary>
    /// Determines whether the specified type is the non-generic Option type.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>true if the type is non-generic Option; otherwise, false.</returns>
    private static bool IsNonGenericOptionType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null)
            return false;

        return typeSymbol.Name == "Option" &&
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: false };
    }
}