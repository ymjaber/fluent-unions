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
/// Provides code fixes for the OptionValueAccessWithoutCheck diagnostic by adding proper safety checks.
/// </summary>
/// <remarks>
/// This code fix provider offers two approaches to fix unsafe Option.Value access:
/// 1. Add IsSome check: Wraps the value access in an if statement that checks IsSome
/// 2. Use Match method: Replaces the value access with a Match call that handles both Some and None cases
/// 
/// The Match approach is generally preferred as it forces handling of both cases explicitly,
/// while the IsSome check is simpler for cases where None handling is not needed.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OptionValueAccessCodeFixProvider)), Shared]
public class OptionValueAccessCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Gets the list of diagnostic IDs that this provider can fix.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => 
        ImmutableArray.Create(DiagnosticIds.OptionValueAccessWithoutCheck);

    /// <summary>
    /// Gets the fix all provider for applying the fix to multiple diagnostics at once.
    /// </summary>
    public sealed override FixAllProvider GetFixAllProvider() => 
        WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for unsafe Option.Value access diagnostics.
    /// </summary>
    /// <param name="context">The code fix context containing the diagnostics and document.</param>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var memberAccess = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf()
            .OfType<MemberAccessExpressionSyntax>().FirstOrDefault();
        
        if (memberAccess == null) return;

        // Register code fix to add IsSome check
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Add IsSome check",
                createChangedDocument: c => AddIsSomeCheckAsync(context.Document, memberAccess, c),
                equivalenceKey: "AddIsSomeCheck"),
            diagnostic);

        // Register code fix to use Match
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Use Match method",
                createChangedDocument: c => UseMatchMethodAsync(context.Document, memberAccess, c),
                equivalenceKey: "UseMatch"),
            diagnostic);
    }

    /// <summary>
    /// Adds an IsSome check around the unsafe value access.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="memberAccess">The member access expression accessing Value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document with the IsSome check added.</returns>
    private async Task<Document> AddIsSomeCheckAsync(Document document, 
        MemberAccessExpressionSyntax memberAccess, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;
        
        // Find the statement containing the value access
        var statement = memberAccess.FirstAncestorOrSelf<StatementSyntax>();
        if (statement == null) return document;

        // Create the IsSome check
        var optionExpression = memberAccess.Expression;
        var isSomeAccess = generator.MemberAccessExpression(optionExpression, "IsSome");
        
        // Create if statement
        var ifStatement = generator.IfStatement(
            isSomeAccess,
            new[] { statement });

        // Replace the original statement with the if statement
        editor.ReplaceNode(statement, ifStatement as SyntaxNode);
        
        return editor.GetChangedDocument();
    }

    /// <summary>
    /// Replaces the unsafe value access with a Match method call.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="memberAccess">The member access expression accessing Value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document with the Match method call.</returns>
    /// <remarks>
    /// The generated Match call includes a lambda for the Some case that contains the original expression,
    /// and a None case that throws an InvalidOperationException to maintain the original behavior.
    /// </remarks>
    private async Task<Document> UseMatchMethodAsync(Document document, 
        MemberAccessExpressionSyntax memberAccess, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        
        if (semanticModel == null) return document;

        // Find the expression or statement containing the value access
        var containingExpression = memberAccess.FirstAncestorOrSelf<ExpressionSyntax>(
            e => e.Parent is StatementSyntax || e.Parent is EqualsValueClauseSyntax);
        
        if (containingExpression == null) return document;

        // Get the option expression
        var optionExpression = memberAccess.Expression;
        
        // Create Match call
        var someParameter = generator.ParameterDeclaration("value");
        var someLambda = generator.ValueReturningLambdaExpression(new[] { someParameter }, containingExpression);
        var noneLambda = generator.ValueReturningLambdaExpression(
            generator.ThrowExpression(
                generator.ObjectCreationExpression(
                    SyntaxFactory.ParseTypeName("System.InvalidOperationException"),
                    generator.LiteralExpression("Option has no value"))));

        var matchCall = generator.InvocationExpression(
            generator.MemberAccessExpression(optionExpression, "Match"),
            someLambda,
            noneLambda);

        // Replace the containing expression with the Match call
        editor.ReplaceNode(containingExpression, matchCall);
        
        return editor.GetChangedDocument();
    }
}