namespace FluentUnions.Tests.OptionTests
{

public class OptionTests
{
    [Fact]
    public void Some_CreatesOptionWithValue()
    {
        // Arrange & Act
        var option = Option.Some(42);

        // Assert
        Assert.True(option.IsSome);
        Assert.False(option.IsNone);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void None_CreatesEmptyOption()
    {
        // Arrange & Act
        var option = Option<int>.None;

        // Assert
        Assert.False(option.IsSome);
        Assert.True(option.IsNone);
    }

    [Fact]
    public void Value_WhenNone_ThrowsInvalidOperationException()
    {
        // Arrange
        var option = Option<int>.None;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => option.Value);
        Assert.Equal("Option is None", exception.Message);
    }

    [Fact]
    public void ImplicitOperator_FromValue_CreatesSome()
    {
        // Arrange & Act
        Option<int> option = 42;

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void ImplicitOperator_FromNone_CreatesNone()
    {
        // Arrange & Act
        Option<int> option = Option.None;

        // Assert
        Assert.False(option.IsSome);
        Assert.True(option.IsNone);
    }

    [Fact]
    public void TryGetValue_WhenSome_ReturnsTrueAndValue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.TryGetValue(out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetValue_WhenNone_ReturnsFalseAndDefault()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.TryGetValue(out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
    }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.Some(42);

        // Act & Assert
        Assert.True(option1.Equals(option2));
        Assert.True(option1 == option2);
        Assert.False(option1 != option2);
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.Some(43);

        // Act & Assert
        Assert.False(option1.Equals(option2));
        Assert.False(option1 == option2);
        Assert.True(option1 != option2);
    }

    [Fact]
    public void Equals_SomeAndNone_ReturnsFalse()
    {
        // Arrange
        var some = Option.Some(42);
        var none = Option<int>.None;

        // Act & Assert
        Assert.False(some.Equals(none));
        Assert.False(none.Equals(some));
    }

    [Fact]
    public void Equals_BothNone_ReturnsTrue()
    {
        // Arrange
        var none1 = Option<int>.None;
        var none2 = Option<int>.None;

        // Act & Assert
        Assert.True(none1.Equals(none2));
        Assert.True(none1 == none2);
    }

    [Fact]
    public void Equals_WithValue_ComparesCorrectly()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        Assert.True(option.Equals(42));
        Assert.False(option.Equals(43));
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHash()
    {
        // Arrange
        var option1 = Option.Some(42);
        var option2 = Option.Some(42);

        // Act & Assert
        Assert.Equal(option1.GetHashCode(), option2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_None_ReturnsConsistentHash()
    {
        // Arrange
        var none1 = Option<int>.None;
        var none2 = Option<int>.None;

        // Act & Assert
        Assert.Equal(none1.GetHashCode(), none2.GetHashCode());
    }

    [Fact]
    public void ToString_Some_ReturnsFormattedValue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.ToString();

        // Assert
        Assert.Equal("Some(42)", result);
    }

    [Fact]
    public void ToString_None_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.ToString();

        // Assert
        Assert.Equal("None", result);
    }

    [Fact]
    public void DefaultValue_IsNone()
    {
        // Arrange & Act
        var option = default(Option<int>);

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("")]
    public void Some_WithString_WorksCorrectly(string value)
    {
        // Arrange & Act
        var option = Option.Some(value);

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(value, option.Value);
    }

    [Fact]
    public void Some_WithComplexType_WorksCorrectly()
    {
        // Arrange
        var complexObject = new TestClass { Id = 1, Name = "Test" };

        // Act
        var option = Option.Some(complexObject);

        // Assert
        Assert.True(option.IsSome);
        Assert.Same(complexObject, option.Value);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
}
