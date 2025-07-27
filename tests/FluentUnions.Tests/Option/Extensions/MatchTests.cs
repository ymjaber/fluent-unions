namespace FluentUnions.Tests.OptionTests.Extensions;

public class MatchTests
{
    [Fact]
    public void Match_WhenSome_ExecutesSomeFunction()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.Match(
            some: value => value * 2,
            none: () => 0
        );

        // Assert
        Assert.Equal(84, result);
    }

    [Fact]
    public void Match_WhenNone_ExecutesNoneFunction()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.Match(
            some: value => value * 2,
            none: () => -1
        );

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Match_DifferentReturnType()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.Match(
            some: value => $"The value is {value}",
            none: () => "No value"
        );

        // Assert
        Assert.Equal("The value is 42", result);
    }

    [Fact]
    public void Match_WithComplexType()
    {
        // Arrange
        var option = Option.Some(new Product { Id = 1, Name = "Widget", Price = 9.99m });

        // Act
        var result = option.Match(
            some: product => $"{product.Name} costs ${product.Price}",
            none: () => "Product not found"
        );

        // Assert
        Assert.Equal("Widget costs $9.99", result);
    }

    [Fact]
    public void Match_WithSideEffects()
    {
        // Arrange
        var option = Option.Some(42);
        var someCalled = false;
        var noneCalled = false;

        // Act
        option.Match(
            some: value => { someCalled = true; return value; },
            none: () => { noneCalled = true; return 0; }
        );

        // Assert
        Assert.True(someCalled);
        Assert.False(noneCalled);
    }

    [Fact]
    public void Match_None_WithSideEffects()
    {
        // Arrange
        var option = Option<int>.None;
        var someCalled = false;
        var noneCalled = false;

        // Act
        option.Match(
            some: value => { someCalled = true; return value; },
            none: () => { noneCalled = true; return 0; }
        );

        // Assert
        Assert.False(someCalled);
        Assert.True(noneCalled);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void Match_WithBoolean(bool value, string expected)
    {
        // Arrange
        var option = Option.Some(value);

        // Act
        var result = option.Match(
            some: v => v ? "Yes" : "No",
            none: () => "Unknown"
        );

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Match_InLinqExpression()
    {
        // Arrange
        var options = new[]
        {
            Option.Some(1),
            Option<int>.None,
            Option.Some(3),
            Option<int>.None,
            Option.Some(5)
        };

        // Act
        var sum = options.Sum(opt => opt.Match(
            some: value => value,
            none: () => 0
        ));

        // Assert
        Assert.Equal(9, sum);
    }

    [Fact]
    public void Match_NestedOptions()
    {
        // Arrange
        var nestedOption = Option.Some(Option.Some(42));

        // Act
        var result = nestedOption.Match(
            some: inner => inner.Match(
                some: value => value * 2,
                none: () => -1
            ),
            none: () => 0
        );

        // Assert
        Assert.Equal(84, result);
    }

    [Fact]
    public void Match_ReturnsOption()
    {
        // Arrange
        var option = Option.Some(10);

        // Act
        var result = option.Match(
            some: value => value > 5 ? Option.Some(value * 2) : Option<int>.None,
            none: () => Option<int>.None
        );

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(20, result.Value);
    }

    private class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
    }
}