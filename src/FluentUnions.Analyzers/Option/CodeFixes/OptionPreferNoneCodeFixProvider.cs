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
/// Provides code fixes for the OptionPreferNoneOverDefault diagnostic by replacing default expressions with Option.None.
/// </summary>
/// <remarks>
/// This code fix provider handles two types of default expressions:
/// - Explicit default expressions: default(Option&lt;T&gt;) → Option.None
/// - Default literal expressions: default → Option.None (when target type is Option&lt;T&gt;)
/// 
/// The code fix maintains type safety while providing clearer intent through the use of Option.None.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OptionPreferNoneCodeFixProvider)), Shared]
public class OptionPreferNoneCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Gets the list of diagnostic IDs that this provider can fix.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => 
        ImmutableArray.Create(DiagnosticIds.OptionPreferNoneOverDefault);

    /// <summary>
    /// Gets the fix all provider for applying the fix to multiple diagnostics at once.
    /// </summary>
    public sealed override FixAllProvider GetFixAllProvider() => 
        WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for the detected default expression diagnostics.
    /// </summary>
    /// <param name="context">The code fix context containing the diagnostics and document.</param>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var node = root.FindNode(diagnosticSpan);
        
        if (node is DefaultExpressionSyntax || node is LiteralExpressionSyntax)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Replace with Option.None",
                    createChangedDocument: c => ReplaceWithOptionNoneAsync(context.Document, node, c),
                    equivalenceKey: "ReplaceWithOptionNone"),
                diagnostic);
        }
    }

    /// <summary>
    /// Replaces a default expression with Option.None.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="defaultExpression">The default expression to replace.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document.</returns>
    private async Task<Document> ReplaceWithOptionNoneAsync(Document document, 
        SyntaxNode defaultExpression, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;
        
        // Create Option.None expression
        var optionType = generator.IdentifierName("Option");
        var noneProperty = generator.IdentifierName("None");
        var optionNoneExpression = generator.MemberAccessExpression(optionType, noneProperty);
        
        // Replace the default expression
        editor.ReplaceNode(defaultExpression, optionNoneExpression);
        
        return editor.GetChangedDocument();
    }
}