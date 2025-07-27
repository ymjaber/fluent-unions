using System.Collections.Immutable;
using FluentUnions.Analyzers.Result;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Tests;

public class UnhandledResultAnalyzerTests
{
    [Fact]
    public async Task UnhandledResultAnalyzer_DetectsUnhandledResultCreation()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        Result.Failure(new Error(""ERR001"", ""Test error"")); // Should trigger FU0104
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0104", diagnostics[0].Id);
        Assert.Contains("Result is not being handled", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_DetectsPureFunctionNotHandled()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Success<int>(42);
        result.Map(x => x * 2); // Should trigger FU0104 with pure function message
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0104", diagnostics[0].Id);
        Assert.Contains("This pure function returns a value that should be used", diagnostics[0].GetMessage());
    }

    [Fact(Skip = "Match extension method detection requires full compilation context with all extension methods")]
    public async Task UnhandledResultAnalyzer_DetectsUnhandledMatch()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Success<int>(42);
        result.Match(
            success: x => x * 2,
            failure: err => -1
        ); // Should trigger FU0104 with pure function message
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0104", diagnostics[0].Id);
        Assert.Contains("This pure function returns a value that should be used", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_AcceptsSideEffectMethods()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Success<int>(42);
        result.OnSuccess(x => Console.WriteLine(x)); // Should NOT trigger warning
        result.OnFailure(err => Console.WriteLine(err)); // Should NOT trigger warning
        result.OnEither(
            x => Console.WriteLine($""Success: {x}""),
            err => Console.WriteLine($""Failure: {err}"")
        ); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_AcceptsTryGetValueInCondition()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Success<int>(42);
        if (result.TryGetValue(out var value))
        {
            Console.WriteLine($""Value: {value}"");
        }
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_AcceptsTryGetErrorInCondition()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Failure<int>(new Error(""ERR001"", ""Test error""));
        if (result.TryGetError(out var error))
        {
            Console.WriteLine($""Error: {error.Message}"");
        }
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_AcceptsTryGetValueStandalone()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Success<int>(42);
        result.TryGetValue(out var value); // Should NOT trigger warning - it's a handling method
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_AcceptsTryGetErrorStandalone()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    void TestMethod()
    {
        var result = Result.Failure<int>(new Error(""ERR001"", ""Test error""));
        result.TryGetError(out var error); // Should NOT trigger warning - it's a handling method
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_SuggestsGetValueOrThrowForValueResult()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    Result<int> GetNumber() => Result.Success(42);
    
    void TestMethod()
    {
        GetNumber(); // Should trigger FU0104 with GetValueOrThrow suggestion
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0104", diagnostics[0].Id);
        Assert.Contains("GetValueOrThrow", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task UnhandledResultAnalyzer_SuggestsThrowIfFailureForUnitResult()
    {
        var analyzer = new UnhandledResultAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class TestClass
{
    Result DoSomething() => Result.Success();
    
    void TestMethod()
    {
        DoSomething(); // Should trigger FU0104 with ThrowIfFailure suggestion
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0104", diagnostics[0].Id);
        Assert.Contains("ThrowIfFailure", diagnostics[0].GetMessage());
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
        assemblies.Add(MetadataReference.CreateFromFile(typeof(FluentUnions.Result).Assembly.Location));

        return CSharpCompilation.Create(
            "TestAssembly",
            new[] { syntaxTree },
            assemblies.Distinct(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }
}
