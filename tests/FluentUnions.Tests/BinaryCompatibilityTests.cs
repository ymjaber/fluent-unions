using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FluentUnions.Tests;

public class BinaryCompatibilityTests
{
    [Fact]
    public void Result_And_EnsureBuilder_HaveSameSize()
    {
        // Assert
        Assert.Equal(Unsafe.SizeOf<Result<int>>(), Unsafe.SizeOf<EnsureBuilder<int>>());
        Assert.Equal(Unsafe.SizeOf<Result<string>>(), Unsafe.SizeOf<EnsureBuilder<string>>());
        Assert.Equal(Unsafe.SizeOf<Result<TestClass>>(), Unsafe.SizeOf<EnsureBuilder<TestClass>>());
    }

    [Fact]
    public void Option_And_FilterBuilder_HaveSameSize()
    {
        // Assert
        Assert.Equal(Unsafe.SizeOf<Option<int>>(), Unsafe.SizeOf<FilterBuilder<int>>());
        Assert.Equal(Unsafe.SizeOf<Option<string>>(), Unsafe.SizeOf<FilterBuilder<string>>());
        Assert.Equal(Unsafe.SizeOf<Option<TestClass>>(), Unsafe.SizeOf<FilterBuilder<TestClass>>());
    }

    [Fact]
    public void Result_To_EnsureBuilder_UnsafeTransform_PreservesSuccessState()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var ensureBuilder = new EnsureBuilder<int>(result);
        // Call Satisfies with a predicate that always returns true to get back a Result
        var rebuilt = ensureBuilder.Satisfies(x => true, new Error("DUMMY", "Dummy"));

        // Assert
        Assert.True(rebuilt.IsSuccess);
        Assert.Equal(42, rebuilt.Value);
    }

    [Fact]
    public void Result_To_EnsureBuilder_UnsafeTransform_PreservesFailureState()
    {
        // Arrange
        var error = new Error("TEST_ERROR", "Test error message");
        var result = Result.Failure<int>(error);

        // Act
        var ensureBuilder = new EnsureBuilder<int>(result);
        // Call Satisfies - it should return the original error
        var rebuilt = ensureBuilder.Satisfies(x => true, new Error("DUMMY", "Dummy"));

        // Assert
        Assert.True(rebuilt.IsFailure);
        Assert.Equal(error.Code, rebuilt.Error.Code);
        Assert.Equal(error.Message, rebuilt.Error.Message);
    }

    [Fact]
    public void EnsureBuilder_To_Result_UnsafeTransform_PreservesSuccessState()
    {
        // Arrange
        var originalResult = Result.Success(42);
        var ensureBuilder = new EnsureBuilder<int>(originalResult);

        // Act
        var result = new Result<int>(ensureBuilder);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void EnsureBuilder_To_Result_UnsafeTransform_PreservesFailureState()
    {
        // Arrange
        var error = new Error("TEST_ERROR", "Test error message");
        var originalResult = Result.Failure<int>(error);
        var ensureBuilder = new EnsureBuilder<int>(originalResult);

        // Act
        var result = new Result<int>(ensureBuilder);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(error.Code, result.Error.Code);
        Assert.Equal(error.Message, result.Error.Message);
    }

    [Fact]
    public void Result_EnsureBuilder_BidirectionalTransform_WithString()
    {
        // Arrange
        var originalResult = Result.Success("Hello, World!");

        // Act - Transform to EnsureBuilder and back
        var ensureBuilder = new EnsureBuilder<string>(originalResult);
        var resultFromBuilder = new Result<string>(ensureBuilder);

        // Assert
        Assert.True(resultFromBuilder.IsSuccess);
        Assert.Equal("Hello, World!", resultFromBuilder.Value);
    }

    [Fact]
    public void Result_EnsureBuilder_BidirectionalTransform_WithComplexType()
    {
        // Arrange
        var testObject = new TestClass { Id = 123, Name = "Test" };
        var originalResult = Result.Success(testObject);

        // Act - Transform to EnsureBuilder and back
        var ensureBuilder = new EnsureBuilder<TestClass>(originalResult);
        var resultFromBuilder = new Result<TestClass>(ensureBuilder);

        // Assert
        Assert.True(resultFromBuilder.IsSuccess);
        Assert.Same(testObject, resultFromBuilder.Value);
        Assert.Equal(123, resultFromBuilder.Value.Id);
        Assert.Equal("Test", resultFromBuilder.Value.Name);
    }

    [Fact]
    public void Option_To_FilterBuilder_UnsafeTransform_PreservesSomeState()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var filterBuilder = new FilterBuilder<int>(option);
        // Call Satisfies with a predicate that always returns true to get back an Option
        var rebuilt = filterBuilder.Satisfies(x => true);

        // Assert
        Assert.True(rebuilt.IsSome);
        Assert.Equal(42, rebuilt.Value);
    }

    [Fact]
    public void Option_To_FilterBuilder_UnsafeTransform_PreservesNoneState()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var filterBuilder = new FilterBuilder<int>(option);
        // Call Satisfies - it should return None
        var rebuilt = filterBuilder.Satisfies(x => true);

        // Assert
        Assert.True(rebuilt.IsNone);
    }

    [Fact]
    public void FilterBuilder_To_Option_UnsafeTransform_PreservesSomeState()
    {
        // Arrange
        var originalOption = Option.Some(42);
        var filterBuilder = new FilterBuilder<int>(originalOption);

        // Act
        var option = new Option<int>(filterBuilder);

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void FilterBuilder_To_Option_UnsafeTransform_PreservesNoneState()
    {
        // Arrange
        var originalOption = Option<int>.None;
        var filterBuilder = new FilterBuilder<int>(originalOption);

        // Act
        var option = new Option<int>(filterBuilder);

        // Assert
        Assert.True(option.IsNone);
    }

    [Fact]
    public void Option_FilterBuilder_BidirectionalTransform_WithString()
    {
        // Arrange
        var originalOption = Option.Some("Hello, World!");

        // Act - Transform to FilterBuilder and back
        var filterBuilder = new FilterBuilder<string>(originalOption);
        var optionFromBuilder = new Option<string>(filterBuilder);

        // Assert
        Assert.True(optionFromBuilder.IsSome);
        Assert.Equal("Hello, World!", optionFromBuilder.Value);
    }

    [Fact]
    public void Option_FilterBuilder_BidirectionalTransform_WithComplexType()
    {
        // Arrange
        var testObject = new TestClass { Id = 456, Name = "Option Test" };
        var originalOption = Option.Some(testObject);

        // Act - Transform to FilterBuilder and back
        var filterBuilder = new FilterBuilder<TestClass>(originalOption);
        var optionFromBuilder = new Option<TestClass>(filterBuilder);

        // Assert
        Assert.True(optionFromBuilder.IsSome);
        Assert.Same(testObject, optionFromBuilder.Value);
        Assert.Equal(456, optionFromBuilder.Value.Id);
        Assert.Equal("Option Test", optionFromBuilder.Value.Name);
    }

    [Fact]
    public unsafe void Result_EnsureBuilder_FieldOffsets_Match()
    {
        // This test verifies that the field layout is identical
        // Note: This uses unsafe code to inspect memory layout

        var result = Result.Success(42);
        var ensureBuilder = new EnsureBuilder<int>(result);

        // Get pointers to the structs
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        var resultPtr = &result;
        var ensureBuilderPtr = &ensureBuilder;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

        // Compare memory content
        var resultBytes = new Span<byte>(resultPtr, Unsafe.SizeOf<Result<int>>());
        var builderBytes = new Span<byte>(ensureBuilderPtr, Unsafe.SizeOf<EnsureBuilder<int>>());

        Assert.True(resultBytes.SequenceEqual(builderBytes));
    }

    [Fact]
    public unsafe void Option_FilterBuilder_FieldOffsets_Match()
    {
        // This test verifies that the field layout is identical
        var option = Option.Some(42);
        var filterBuilder = new FilterBuilder<int>(option);

        // Get pointers to the structs
        var optionPtr = &option;
        var filterBuilderPtr = &filterBuilder;

        // Compare memory content
        var optionBytes = new Span<byte>(optionPtr, Unsafe.SizeOf<Option<int>>());
        var builderBytes = new Span<byte>(filterBuilderPtr, Unsafe.SizeOf<FilterBuilder<int>>());

        Assert.True(optionBytes.SequenceEqual(builderBytes));
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
