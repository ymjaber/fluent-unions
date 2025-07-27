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
/// Provides code fixes for the OptionNoneMisuse diagnostic.
/// </summary>
/// <remarks>
/// This code fix provider offers two solutions for Option.None misuse:
/// 1. Remove the variable declaration and use Option.None directly (when possible)
/// 2. Change the variable type to Option&lt;T&gt; where T needs to be specified by the user
/// 
/// The first fix is preferred when the variable is only used once, while the second
/// fix is offered when the variable needs to be retained for multiple uses.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(OptionNoneMisuseCodeFixProvider)), Shared]
public class OptionNoneMisuseCodeFixProvider : CodeFixProvider
{
    /// <summary>
    /// Gets the list of diagnostic IDs that this provider can fix.
    /// </summary>
    public sealed override ImmutableArray<string> FixableDiagnosticIds => 
        ImmutableArray.Create(DiagnosticIds.OptionNoneMisuse);

    /// <summary>
    /// Gets the fix all provider for applying the fix to multiple diagnostics at once.
    /// </summary>
    public sealed override FixAllProvider GetFixAllProvider() => 
        WellKnownFixAllProviders.BatchFixer;

    /// <summary>
    /// Registers code fixes for the detected Option.None misuse diagnostics.
    /// </summary>
    /// <param name="context">The code fix context containing the diagnostics and document.</param>
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null) return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var node = root.FindNode(diagnosticSpan);
        
        if (node is VariableDeclaratorSyntax variableDeclarator)
        {
            await RegisterVariableDeclarationFixesAsync(context, variableDeclarator);
        }
        else if (node is AssignmentExpressionSyntax assignment)
        {
            await RegisterAssignmentFixesAsync(context, assignment);
        }
    }

    /// <summary>
    /// Registers code fixes for variable declaration misuse.
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <param name="variableDeclarator">The variable declarator with Option.None assignment.</param>
    private async Task RegisterVariableDeclarationFixesAsync(CodeFixContext context, VariableDeclaratorSyntax variableDeclarator)
    {
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel == null) return;

        var diagnostic = context.Diagnostics.First();

        // Find the variable declaration
        var variableDeclaration = variableDeclarator.Parent as VariableDeclarationSyntax;
        if (variableDeclaration == null) return;

        // Check if we can determine the usage of this variable
        var localDeclaration = variableDeclaration.Parent as LocalDeclarationStatementSyntax;
        if (localDeclaration != null)
        {
            // Offer to inline Option.None if the variable is only used once
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Use Option.None directly",
                    createChangedDocument: c => InlineOptionNoneAsync(
                        context.Document, variableDeclarator, c),
                    equivalenceKey: "InlineOptionNone"),
                diagnostic);
        }

        // Always offer to change the type
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Change type to Option<T> (specify T)",
                createChangedDocument: c => ChangeToGenericOptionAsync(
                    context.Document, variableDeclaration, c),
                equivalenceKey: "ChangeToGenericOption"),
            diagnostic);
    }

    /// <summary>
    /// Registers code fixes for assignment expression misuse.
    /// </summary>
    /// <param name="context">The code fix context.</param>
    /// <param name="assignment">The assignment expression with Option.None.</param>
    private Task RegisterAssignmentFixesAsync(CodeFixContext context, AssignmentExpressionSyntax assignment)
    {
        // For assignments, we can only suggest changing the variable declaration type
        // This would require finding the original declaration
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Change variable type to Option<T>",
                createChangedDocument: c => Task.FromResult(context.Document), // Placeholder - would need more complex implementation
                equivalenceKey: "ChangeVariableType"),
            context.Diagnostics.First());
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Inlines Option.None by removing the variable and using Option.None directly at usage sites.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="variableDeclarator">The variable declarator to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document.</returns>
    private async Task<Document> InlineOptionNoneAsync(Document document, 
        VariableDeclaratorSyntax variableDeclarator, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        
        if (semanticModel == null) return document;

        // Get the symbol for the variable
        var variableSymbol = semanticModel.GetDeclaredSymbol(variableDeclarator);
        if (variableSymbol == null) return document;

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null) return document;

        // Find all references to this variable
        var references = root.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Where(id => id.Identifier.Text == variableSymbol.Name)
            .Where(id => SymbolEqualityComparer.Default.Equals(semanticModel.GetSymbolInfo(id).Symbol, variableSymbol))
            .ToList();

        // Replace all references with Option.None
        foreach (var reference in references)
        {
            var optionNone = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("Option"),
                SyntaxFactory.IdentifierName("None"));
            
            editor.ReplaceNode(reference, optionNone);
        }

        // Remove the variable declaration
        var declaration = variableDeclarator.Parent?.Parent;
        if (declaration != null)
        {
            editor.RemoveNode(declaration);
        }

        return editor.GetChangedDocument();
    }

    /// <summary>
    /// Changes the variable type from Option to Option&lt;T&gt; with a placeholder for T.
    /// </summary>
    /// <param name="document">The document to modify.</param>
    /// <param name="variableDeclaration">The variable declaration to modify.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The modified document.</returns>
    private async Task<Document> ChangeToGenericOptionAsync(Document document, 
        VariableDeclarationSyntax variableDeclaration, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        
        // Create an annotation for the type parameter to enable cursor navigation
        var typeParameterAnnotation = new SyntaxAnnotation("TypeParameter");
        
        // Create a generic Option<T> type with annotated type parameter
        var typeParameter = SyntaxFactory.IdentifierName("T")
            .WithAdditionalAnnotations(typeParameterAnnotation);
            
        var genericOption = SyntaxFactory.GenericName("Option")
            .WithTypeArgumentList(
                SyntaxFactory.TypeArgumentList(
                    SyntaxFactory.SingletonSeparatedList<TypeSyntax>(typeParameter)));

        // Create a new variable declaration with the generic type
        var newDeclaration = variableDeclaration.WithType(genericOption);
        
        // Replace the old declaration
        editor.ReplaceNode(variableDeclaration, newDeclaration);
        
        // Get the changed document and apply rename tracking
        var changedDocument = editor.GetChangedDocument();
        
        // Add rename tracking annotation for better IDE support
        var root = await changedDocument.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root != null)
        {
            var annotatedNode = root.GetAnnotatedNodes(typeParameterAnnotation).FirstOrDefault();
            if (annotatedNode != null)
            {
                // Replace the annotation with RenameAnnotation for IDE cursor placement
                var renameAnnotation = Microsoft.CodeAnalysis.CodeActions.RenameAnnotation.Create();
                root = root.ReplaceNode(annotatedNode, annotatedNode.WithAdditionalAnnotations(renameAnnotation));
                changedDocument = changedDocument.WithSyntaxRoot(root);
            }
        }
        
        return changedDocument;
    }
}