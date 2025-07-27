namespace FluentUnions.Tests.OptionTests.Extensions;

public class MapTests
{
    [Fact]
    public void Map_WhenSome_TransformsValue()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = option.Map(x => x * 2);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void Map_WhenNone_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.Map(x => x * 2);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Map_ChangesType()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.Map(x => x.ToString());

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("42", result.Value);
    }

    [Fact]
    public void Map_WithComplexTransformation()
    {
        // Arrange
        var option = Option.Some(new Person { FirstName = "John", LastName = "Doe" });

        // Act
        var result = option.Map(p => $"{p.FirstName} {p.LastName}");

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("John Doe", result.Value);
    }

    [Fact]
    public void Map_ChainedOperations()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = option
            .Map(x => x * 2)
            .Map(x => x + 1)
            .Map(x => x.ToString());

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("11", result.Value);
    }

    [Fact]
    public void Map_ToEmptyString_ReturnsSomeWithEmptyString()
    {
        // Arrange
        var option = Option.Some("test");

        // Act
        var result = option.Map(x => string.Empty);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(string.Empty, result.Value);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(10, 20)]
    [InlineData(-5, -10)]
    public void Map_WithVariousValues_TransformsCorrectly(int input, int expected)
    {
        // Arrange
        var option = Option.Some(input);

        // Act
        var result = option.Map(x => x * 2);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(expected, result.Value);
    }

    private class Person
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}