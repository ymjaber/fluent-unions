using Xunit;

namespace FluentUnions.PackageValidation;

public class AnalyzerValidationTests
{
    [Fact]
    public void Option_Should_Not_Be_Compared_With_Null()
    {
        var option = Option.Some(42);
        
        // The following lines should trigger analyzer warnings when built
        // Uncommenting them would produce: "Option<T> should not be compared with null"
        
        // BAD: These should trigger FU001 analyzer warning
        // if (option == null) { }
        // if (option != null) { }
        // if (option is null) { }
        
        // GOOD: These are the correct ways
        if (option.IsNone) { }
        if (option.IsSome) { }
        
        Assert.True(option.IsSome);
    }

    [Fact]
    public void Option_Value_Should_Not_Be_Accessed_Without_Check()
    {
        var option = Option<string>.None;
        
        // The following line should trigger analyzer warning when built
        // Uncommenting it would produce: "Option.Value accessed without checking IsSome"
        
        // BAD: This should trigger FU002 analyzer warning
        // var value = option.Value;
        
        // GOOD: These are the correct ways
        if (option.IsSome)
        {
            var value = option.Value;
            Assert.NotNull(value);
        }
        
        var safeValue = option.Match(
            some: v => v,
            none: () => "default"
        );
        
        Assert.Equal("default", safeValue);
    }

    [Fact]
    public void Result_Value_Should_Not_Be_Accessed_Without_Check()
    {
        var result = Result.Failure<int>(new ValidationError("TEST001", "Test error"));
        
        // The following line should trigger analyzer warning when built
        // Uncommenting it would produce: "Result.Value accessed without checking IsSuccess"
        
        // BAD: This should trigger FU004 analyzer warning
        // var value = result.Value;
        
        // GOOD: These are the correct ways
        if (result.IsSuccess)
        {
            var value = result.Value;
#pragma warning disable xUnit2020
            Assert.True(false, "Should not reach here");
#pragma warning restore xUnit2020
        }
        
        var safeValue = result.Match(
            success: v => v,
            failure: _ => -1
        );
        
        Assert.Equal(-1, safeValue);
    }

    [Fact]
    public void Result_Error_Should_Not_Be_Accessed_Without_Check()
    {
        var result = Result.Success(42);
        
        // The following line should trigger analyzer warning when built
        // Uncommenting it would produce: "Result.Error accessed without checking IsFailure"
        
        // BAD: This should trigger FU005 analyzer warning
        // var error = result.Error;
        
        // GOOD: These are the correct ways
        if (result.IsFailure)
        {
            var error = result.Error;
#pragma warning disable xUnit2020
            Assert.True(false, "Should not reach here");
#pragma warning restore xUnit2020
        }
        
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Prefer_Match_Over_If_Checks()
    {
        var option = Option.Some("test");
        
        // The analyzer should suggest using Match instead of if/else patterns
        // This is a style preference that FU003 and FU006 analyzers promote
        
        // Less preferred pattern (might trigger info/suggestion):
        string result1;
        if (option.IsSome)
        {
            result1 = option.Value.ToUpper();
        }
        else
        {
            result1 = "NONE";
        }
        
        // Preferred pattern:
        var result2 = option.Match(
            some: v => v.ToUpper(),
            none: () => "NONE"
        );
        
        Assert.Equal(result1, result2);
        Assert.Equal("TEST", result2);
    }
}