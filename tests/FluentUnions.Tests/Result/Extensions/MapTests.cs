namespace FluentUnions.Tests.ResultTests.Extensions;

public class MapTests
{
    [Fact]
    public void Map_WhenSuccess_TransformsValue()
    {
        // Arrange
        var result = Result.Success(5);

        // Act
        var mapped = result.Map(x => x * 2);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public void Map_WhenFailure_PropagatesError()
    {
        // Arrange
        var error = new Error("E001", "Original error");
        var result = Result.Failure<int>(error);

        // Act
        var mapped = result.Map(x => x * 2);

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Map_ChangesType()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var mapped = result.Map(x => x.ToString());

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void Map_ChainedOperations()
    {
        // Arrange
        var result = Result.Success(5);

        // Act
        var final = result
            .Map(x => x * 2)
            .Map(x => x + 1)
            .Map(x => x.ToString());

        // Assert
        Assert.True(final.IsSuccess);
        Assert.Equal("11", final.Value);
    }

    [Fact]
    public void Map_ChainedOperations_StopsOnFailure()
    {
        // Arrange
        var error = new Error("E001", "Error");
        var result = Result.Failure<int>(error);

        // Act
        var final = result
            .Map(x => x * 2)
            .Map(x => x + 1)
            .Map(x => x.ToString());

        // Assert
        Assert.True(final.IsFailure);
        Assert.Equal(error, final.Error);
    }

    [Fact]
    public void Map_WithComplexType()
    {
        // Arrange
        var user = new User { FirstName = "John", LastName = "Doe" };
        var result = Result.Success(user);

        // Act
        var mapped = result.Map(u => $"{u.FirstName} {u.LastName}");

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("John Doe", mapped.Value);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(10, 20)]
    [InlineData(-5, -10)]
    public void Map_WithVariousValues_TransformsCorrectly(int input, int expected)
    {
        // Arrange
        var result = Result.Success(input);

        // Act
        var mapped = result.Map(x => x * 2);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal(expected, mapped.Value);
    }

    [Fact]
    public void Map_PreservesReferenceEquality()
    {
        // Arrange
        var obj = new object();
        var result = Result.Success(obj);

        // Act
        var mapped = result.Map(x => x);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Same(obj, mapped.Value);
    }

    private class User
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}