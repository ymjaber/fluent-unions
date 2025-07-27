using System.Collections.Immutable;
using FluentUnions.Analyzers.Option;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Tests;

public class BasicAnalyzerTests
{
    [Fact]
    public async Task OptionValueAccessAnalyzer_DetectsUnsafeValueAccess()
    {
        var analyzer = new OptionValueAccessAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        var value = option.Value; // Should trigger FU0001
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0001", diagnostics[0].Id);
        Assert.Contains("Option.Value is accessed without checking IsSome", diagnostics[0].GetMessage());
    }

    private static async Task<Diagnostic[]> GetDiagnosticsAsync(DiagnosticAnalyzer analyzer, string source)
    {
        var compilation = CreateCompilation(source);
        var compilationWithAnalyzers = compilation.WithAnalyzers(
            ImmutableArray.Create(analyzer),
            new AnalyzerOptions(ImmutableArray<AdditionalText>.Empty));
        
        var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();
        return diagnostics.ToArray();
    }

    private static Compilation CreateCompilation(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Option<>).Assembly.Location)
        };

        return CSharpCompilation.Create(
            "TestAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}