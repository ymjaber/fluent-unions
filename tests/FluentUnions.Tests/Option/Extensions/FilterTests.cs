namespace FluentUnions.Tests.OptionTests.Extensions;

public class FilterTests
{
    [Fact]
    public void Filter_WhenSome_AndPredicateTrue_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(10);

        // Act
        var result = option.Filter(x => x > 5);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void Filter_WhenSome_AndPredicateFalse_ReturnsNone()
    {
        // Arrange
        var option = Option.Some(3);

        // Act
        var result = option.Filter(x => x > 5);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Filter_WhenNone_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.Filter(x => x > 5);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Filter_ChainedFilters()
    {
        // Arrange
        var option = Option.Some(15);

        // Act
        var result = option
            .Filter(x => x > 10)
            .Filter(x => x < 20)
            .Filter(x => x % 3 == 0);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(15, result.Value);
    }

    [Fact]
    public void Filter_ChainedFilters_FailsOnSecond()
    {
        // Arrange
        var option = Option.Some(25);

        // Act
        var result = option
            .Filter(x => x > 10)  // Pass
            .Filter(x => x < 20)  // Fail
            .Filter(x => x % 5 == 0); // Not executed

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Filter_WithString()
    {
        // Arrange
        var option = Option.Some("hello");

        // Act
        var result = option.Filter(s => s.Length > 3);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void Filter_WithComplexType()
    {
        // Arrange
        var option = Option.Some(new Person { Name = "John", Age = 25 });

        // Act
        var result = option.Filter(p => p.Age >= 18);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("John", result.Value.Name);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(5, false)]
    [InlineData(10, true)]
    [InlineData(15, true)]
    public void Filter_WithVariousValues(int value, bool shouldPass)
    {
        // Arrange
        var option = Option.Some(value);

        // Act
        var result = option.Filter(x => x >= 10);

        // Assert
        Assert.Equal(shouldPass, result.IsSome);
        if (shouldPass)
        {
            Assert.Equal(value, result.Value);
        }
    }

    [Fact]
    public void Filter_PredicateAlwaysTrue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.Filter(_ => true);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Filter_PredicateAlwaysFalse()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.Filter(_ => false);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Filter_CombinedWithMap()
    {
        // Arrange
        var option = Option.Some("hello world");

        // Act
        var result = option
            .Filter(s => s.Contains(" "))
            .Map(s => s.ToUpper())
            .Filter(s => s.Length > 5);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("HELLO WORLD", result.Value);
    }

    [Fact]
    public void Filter_UsedForValidation()
    {
        // Arrange
        var email = Option.Some("test@example.com");

        // Act
        var result = email
            .Filter(e => e.Contains("@"))
            .Filter(e => e.Contains("."))
            .Filter(e => e.Length > 5);

        // Assert
        Assert.True(result.IsSome);
    }

    [Fact]
    public void Filter_UsedForValidation_Invalid()
    {
        // Arrange
        var email = Option.Some("invalid-email");

        // Act
        var result = email
            .Filter(e => e.Contains("@"))
            .Filter(e => e.Contains("."))
            .Filter(e => e.Length > 5);

        // Assert
        Assert.True(result.IsNone);
    }

    private class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }
}