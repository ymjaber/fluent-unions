using System.Collections.Immutable;
using FluentUnions.Analyzers.Option;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Tests;

public class UnhandledOptionAnalyzerTests
{
    [Fact]
    public async Task UnhandledOptionAnalyzer_DetectsUnhandledOptionCreation()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        Option.Some(42); // Should trigger FU0006
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0006", diagnostics[0].Id);
        Assert.Contains("Option<int> is not being handled", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_DetectsPureFunctionNotHandled()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        option.Map(x => x * 2); // Should trigger FU0006 with pure function message
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0006", diagnostics[0].Id);
        Assert.Contains("This pure function returns a value that should be used", diagnostics[0].GetMessage());
    }

    [Fact(Skip = "Match extension method detection requires full compilation context with all extension methods")]
    public async Task UnhandledOptionAnalyzer_DetectsUnhandledMatch()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        option.Match(
            some: x => x * 2,
            none: () => -1
        ); // Should trigger FU0006 with pure function message
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0006", diagnostics[0].Id);
        Assert.Contains("This pure function returns a value that should be used", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_AcceptsSideEffectMethods()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        option.OnSome(x => Console.WriteLine(x)); // Should NOT trigger warning
        option.OnNone(() => Console.WriteLine(""None"")); // Should NOT trigger warning
        option.OnEither(
            x => Console.WriteLine($""Some: {x}""),
            () => Console.WriteLine(""None"")
        ); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_AcceptsTryGetValueInCondition()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        if (option.TryGetValue(out var value))
        {
            Console.WriteLine($""Value: {value}"");
        }
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_AcceptsTryGetValueStandalone()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        option.TryGetValue(out var value); // Should NOT trigger warning - it's a handling method
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_AcceptsTryGetValueInWhileLoop()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    Option<int> GetNext() => Option.Some(42);

    void TestMethod()
    {
        var option = GetNext();
        while (option.TryGetValue(out var value))
        {
            Console.WriteLine($""Processing: {value}"");
            option = GetNext();
        }
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_DetectsUnhandledFilter()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var option = Option.Some(42);
        option.Filter(x => x > 40); // Should trigger FU0006 with pure function message
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0006", diagnostics[0].Id);
        Assert.Contains("This pure function returns a value that should be used", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_AcceptsHandledOption()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    Option<int> GetNumber() => Option.Some(42);
    
    void TestMethod()
    {
        var option = GetNumber(); // Should NOT trigger warning
        
        if (option.IsSome)
        {
            // Use the value
        }
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledOptionAnalyzer_SuggestsProperHandlingMethods()
    {
        var analyzer = new UnhandledOptionAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    Option<int> GetNumber() => Option.Some(42);
    
    void TestMethod()
    {
        GetNumber(); // Should trigger FU0006 with proper suggestions
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0006", diagnostics[0].Id);
        var message = diagnostics[0].GetMessage();
        Assert.Contains("GetValueOrDefault", message);
        Assert.Contains("GetValueOrThrow", message);
        Assert.Contains("OnSome/OnNone/OnEither", message);
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

        // Get all loaded assemblies to ensure we have all dependencies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();

        // Ensure FluentUnions is included
        assemblies.Add(MetadataReference.CreateFromFile(typeof(FluentUnions.Option<>).Assembly.Location));

        return CSharpCompilation.Create(
            "TestAssembly",
            new[] { syntaxTree },
            assemblies.Distinct(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}
