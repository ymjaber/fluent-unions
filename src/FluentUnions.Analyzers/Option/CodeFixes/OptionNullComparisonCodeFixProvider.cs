using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option.CodeFixes;

/// <summary>
/// Provides code fixes for the OptionNullComparison diagnostic by replacing null comparisons with appropriate Option property checks.
/// </summary>
/// <remarks>
/// This code fix provider handles two types of null comparison patterns:
/// - Binary expressions (== null, != null): Replaced with IsNone or IsSome respectively
/// - Is pattern expressions (is null): Replaced with IsNone
/// 
/// The code fix maintains the semantic meaning of the original comparison while using the proper Option API.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OptionNullComparisonCodeFixProvider)), Shared]
public class OptionNullComparisonCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Gets the list of diagnostic IDs that this provider can fix.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => 
        ImmutableArray.Create(DiagnosticIds.OptionNullComparison);

    /// <summary>
    /// Gets the fix all provider for applying the fix to multiple diagnostics at once.
    /// </summary>
    public sealed override FixAllProvider GetFixAllProvider() => 
        WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for the detected null comparison diagnostics.
    /// </summary>
    /// <param name="context">The code fix context containing the diagnostics and document.</param>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var node = root.FindNode(diagnosticSpan);
        
        if (node is BinaryExpressionSyntax binaryExpression)
        {
            await RegisterBinaryExpressionFixesAsync(context, binaryExpression);
        }
        else if (node is IsPatternExpressionSyntax isPattern)
        {
            await RegisterIsPatternFixesAsync(context, isPattern);
        }
    }

    /// <summary>
    /// Registers code fixes for binary expression null comparisons (== null, != null).
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <param name="binaryExpression">The binary expression comparing with null.</param>
    private Task RegisterBinaryExpressionFixesAsync(CodeFixContext context, BinaryExpressionSyntax binaryExpression)
    {
        var isEqualsOperator = binaryExpression.IsKind(SyntaxKind.EqualsExpression);
        var replacementProperty = isEqualsOperator ? "IsNone" : "IsSome";
        
        context.RegisterCodeFix(
            CodeAction.Create(
                title: $"Replace with {replacementProperty}",
                createChangedDocument: c => ReplaceWithPropertyCheckAsync(
                    context.Document, binaryExpression, replacementProperty, c),
                equivalenceKey: $"ReplaceWith{replacementProperty}"),
            context.Diagnostics.First());
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Registers code fixes for 'is null' pattern expressions.
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <param name="isPattern">The is pattern expression.</param>
    private Task RegisterIsPatternFixesAsync(CodeFixContext context, IsPatternExpressionSyntax isPattern)
    {
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Replace with IsNone",
                createChangedDocument: c => ReplaceIsPatternWithPropertyCheckAsync(
                    context.Document, isPattern, "IsNone", c),
                equivalenceKey: "ReplaceWithIsNone"),
            context.Diagnostics.First());
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Replaces a binary null comparison expression with an Option property check.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="binaryExpression">The binary expression to replace.</param>
    /// <param name="propertyName">The name of the Option property to use (IsNone or IsSome).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document.</returns>
    private async Task<Document> ReplaceWithPropertyCheckAsync(Document document, 
        BinaryExpressionSyntax binaryExpression, string propertyName, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;
        
        // Find the option expression (the non-null side)
        var optionExpression = IsNullLiteral(binaryExpression.Left) 
            ? binaryExpression.Right 
            : binaryExpression.Left;
        
        // Create property access
        var propertyAccess = generator.MemberAccessExpression(optionExpression, propertyName);
        
        // Replace the binary expression
        editor.ReplaceNode(binaryExpression, propertyAccess);
        
        return editor.GetChangedDocument();
    }

    /// <summary>
    /// Replaces an 'is null' pattern expression with an Option property check.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="isPattern">The is pattern expression to replace.</param>
    /// <param name="propertyName">The name of the Option property to use (typically IsNone).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document.</returns>
    private async Task<Document> ReplaceIsPatternWithPropertyCheckAsync(Document document, 
        IsPatternExpressionSyntax isPattern, string propertyName, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;
        
        // Create property access
        var propertyAccess = generator.MemberAccessExpression(isPattern.Expression, propertyName);
        
        // Replace the is pattern expression
        editor.ReplaceNode(isPattern, propertyAccess);
        
        return editor.GetChangedDocument();
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