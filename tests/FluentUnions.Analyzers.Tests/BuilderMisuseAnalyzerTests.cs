using System.Collections.Immutable;
using FluentUnions.Analyzers.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace FluentUnions.Analyzers.Tests;

public class BuilderMisuseAnalyzerTests
{
    #region EnsureBuilder Tests

    [Fact]
    public async Task EnsureBuilder_AssignedToVariable_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var result = Result.Success(42);
        var builder = result.Ensure; // Should trigger FU0105
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
        Assert.Contains("EnsureBuilder is not properly completed", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task EnsureBuilder_ReturnedDirectly_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    EnsureBuilder<int> GetBuilder()
    {
        var result = Result.Success(42);
        return result.Ensure; // Should trigger FU0105
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
    }

    [Fact]
    public async Task EnsureBuilder_InVoidExpressionBodiedMethod_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test(Result<int> result) => result.Ensure; // Should trigger FU0105
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
    }

    [Fact]
    public async Task EnsureBuilder_WithSatisfies_ShouldNotReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var result = Result.Success(42);
        var validated = result.Ensure.Satisfies(x => x > 0, new Error(""Must be positive"")); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task EnsureBuilder_WithPredefinedMethod_ShouldNotReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var result = Result.Success(""hello"");
        var validated = result.Ensure.NotEmpty(); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task EnsureBuilder_PassedAsArgument_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Process(object obj) { }
    
    void Test()
    {
        var result = Result.Success(42);
        Process(result.Ensure); // Should trigger FU0105
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
    }

    [Fact]
    public async Task EnsureBuilder_Awaited_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System.Threading.Tasks;

class Program
{
    async Task Test()
    {
        var result = Result.Success(42);
        await result.Ensure; // Should trigger FU0105
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
    }

    [Fact]
    public async Task EnsureBuilder_ChainedPredefinedMethods_ShouldNotReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var result = Result.Success(10);
        var validated = result.Ensure.GreaterThan(0); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    #endregion

    #region FilterBuilder Tests

    [Fact]
    public async Task FilterBuilder_AssignedToVariable_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(42);
        var builder = option.Filter; // Should trigger FU0007
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0007", diagnostics[0].Id);
        Assert.Contains("FilterBuilder is not properly completed", diagnostics[0].GetMessage());
    }

    [Fact]
    public async Task FilterBuilder_ReturnedDirectly_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    FilterBuilder<int> GetBuilder()
    {
        var option = Option.Some(42);
        return option.Filter; // Should trigger FU0007
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0007", diagnostics[0].Id);
    }

    [Fact]
    public async Task FilterBuilder_WithSatisfies_ShouldNotReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(42);
        var filtered = option.Filter.Satisfies(x => x > 0); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task FilterBuilder_WithPredefinedMethod_ShouldNotReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(""hello"");
        var filtered = option.Filter.NotEmpty(); // Should NOT trigger warning
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task FilterBuilder_InConditionalExpression_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(42);
        var result = true ? option.Filter : null; // Should trigger FU0007
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0007", diagnostics[0].Id);
    }

    [Fact]
    public async Task FilterBuilder_UsedInBinaryExpression_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(42);
        var check = option.Filter == null; // Should trigger FU0007
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0007", diagnostics[0].Id);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public async Task EnsureBuilder_InLambdaExpression_ShouldReportDiagnostic()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;
using System;

class Program
{
    void Test()
    {
        var result = Result.Success(42);
        Action action = () => result.Ensure; // Should trigger FU0105
    }
}");

        Assert.Single(diagnostics);
        Assert.Equal("FU0105", diagnostics[0].Id);
    }

    [Fact]
    public async Task FilterBuilder_InLocalFunction_ShouldWorkCorrectly()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var option = Option.Some(42);
        
        Option<int> ProcessOption()
        {
            return option.Filter.GreaterThan(0); // Should NOT trigger warning
        }
        
        var result = ProcessOption();
    }
}");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task Builders_InComplexExpressions_ShouldDetectCorrectly()
    {
        var analyzer = new BuilderMisuseAnalyzer();
        var diagnostics = await GetDiagnosticsAsync(analyzer, @"
using FluentUnions;

class Program
{
    void Test()
    {
        var result = Result.Success(42);
        var option = Option.Some(""test"");
        
        // Valid usages
        var r1 = result.Ensure.GreaterThan(0);
        var o1 = option.Filter.NotEmpty();
        
        // Invalid usages
        var r2 = result.Ensure; // Should trigger FU0105
        var o2 = option.Filter; // Should trigger FU0007
    }
}");

        Assert.Equal(2, diagnostics.Length);
        Assert.Contains(diagnostics, d => d.Id == "FU0105");
        Assert.Contains(diagnostics, d => d.Id == "FU0007");
    }

    #endregion

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