using System.Collections.Immutable;
using FluentUnions.Analyzers.Option;
using FluentUnions.Analyzers.Option.CodeFixes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace FluentUnions.Analyzers.Tests;

public class OptionPreferNoneCodeFixTests
{
    [Fact]
    public async Task DefaultExpression_ReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<int> option = default(Option<int>);
    }
}";

        const string expected = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<int> option = Option.None;
    }
}";

        await VerifyCodeFixAsync(source, expected);
    }

    [Fact]
    public async Task DefaultLiteral_ReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<string> option = default;
    }
}";

        const string expected = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<string> option = Option.None;
    }
}";

        await VerifyCodeFixAsync(source, expected);
    }

    [Fact]
    public async Task DefaultExpression_InReturnStatement_ReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    Option<int> GetOption()
    {
        return default(Option<int>);
    }
}";

        const string expected = @"
using FluentUnions;

class Program
{
    Option<int> GetOption()
    {
        return Option.None;
    }
}";

        await VerifyCodeFixAsync(source, expected);
    }

    [Fact]
    public async Task DefaultLiteral_InReturnStatement_ReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    Option<string> GetOption()
    {
        return default;
    }
}";

        const string expected = @"
using FluentUnions;

class Program
{
    Option<string> GetOption()
    {
        return Option.None;
    }
}";

        await VerifyCodeFixAsync(source, expected);
    }

    [Fact]
    public async Task DefaultExpression_InConditional_ReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    Option<int> GetOption(bool condition)
    {
        return condition ? Option.Some(42) : default(Option<int>);
    }
}";

        const string expected = @"
using FluentUnions;

class Program
{
    Option<int> GetOption(bool condition)
    {
        return condition ? Option.Some(42) : Option.None;
    }
}";

        await VerifyCodeFixAsync(source, expected);
    }

    [Fact]
    public async Task MultipleDefaults_AllReplacedWithOptionNone()
    {
        const string source = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<int> option1 = default(Option<int>);
        Option<string> option2 = default;
        var option3 = GetOption() ?? default(Option<bool>);
    }
    
    Option<bool> GetOption() => default;
}";

        const string expected = @"
using FluentUnions;

class Program
{
    void Test()
    {
        Option<int> option1 = Option.None;
        Option<string> option2 = Option.None;
        var option3 = GetOption() ?? Option.None;
    }
    
    Option<bool> GetOption() => Option.None;
}";

        await VerifyCodeFixAsync(source, expected);
    }

    private static async Task VerifyCodeFixAsync(string source, string expected)
    {
        var analyzer = new OptionPreferNoneAnalyzer();
        var codeFixProvider = new OptionPreferNoneCodeFixProvider();
        
        // Get diagnostics
        var document = CreateDocument(source);
        var compilation = await document.Project.GetCompilationAsync();
        var compilationWithAnalyzer = compilation!.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(analyzer));
        var diagnostics = await compilationWithAnalyzer.GetAnalyzerDiagnosticsAsync();
        
        // Apply all code fixes using Fix All
        var fixAllProvider = codeFixProvider.GetFixAllProvider();
        var fixAllContext = new FixAllContext(
            document,
            codeFixProvider,
            FixAllScope.Document,
            "ReplaceWithOptionNone",
            codeFixProvider.FixableDiagnosticIds,
            new FixAllDiagnosticProvider(diagnostics),
            CancellationToken.None);
            
        var fixAllAction = await fixAllProvider.GetFixAsync(fixAllContext);
        if (fixAllAction != null)
        {
            var operations = await fixAllAction.GetOperationsAsync(CancellationToken.None);
            var solution = operations.OfType<ApplyChangesOperation>().First().ChangedSolution;
            document = solution.GetDocument(document.Id)!;
        }
        
        // Verify the fix
        var fixedRoot = await document.GetSyntaxRootAsync();
        var actualFixed = fixedRoot!.ToFullString();
        
        Assert.Equal(expected.Trim(), actualFixed.Trim());
    }
    
    private class FixAllDiagnosticProvider : FixAllContext.DiagnosticProvider
    {
        private readonly ImmutableArray<Diagnostic> _diagnostics;
        
        public FixAllDiagnosticProvider(ImmutableArray<Diagnostic> diagnostics)
        {
            _diagnostics = diagnostics;
        }
        
        public override Task<IEnumerable<Diagnostic>> GetAllDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>(_diagnostics);
            
        public override Task<IEnumerable<Diagnostic>> GetDocumentDiagnosticsAsync(Document document, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>(_diagnostics);
            
        public override Task<IEnumerable<Diagnostic>> GetProjectDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            => Task.FromResult<IEnumerable<Diagnostic>>(_diagnostics);
    }

    private static Document CreateDocument(string source)
    {
        var projectId = ProjectId.CreateNewId();
        var documentId = DocumentId.CreateNewId(projectId);
        
        var solution = new AdhocWorkspace()
            .CurrentSolution
            .AddProject(projectId, "TestProject", "TestProject", LanguageNames.CSharp)
            .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(FluentUnions.Option).Assembly.Location))
            .AddDocument(documentId, "Test.cs", SourceText.From(source));

        var project = solution.GetProject(projectId)!
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        return project.GetDocument(documentId)!;
    }
}