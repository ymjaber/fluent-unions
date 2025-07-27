namespace FluentUnions.Tests.OptionTests
{

public class OptionFactoryTests
{
    [Fact]
    public void From_WithNonNullValue_CreatesSome()
    {
        // Arrange & Act
        var option = Option.From<string>("test");

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal("test", option.Value);
    }

    [Fact]
    public void From_WithNullValue_CreatesNone()
    {
        // Arrange & Act
        var option = Option.From<string>(null);

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void From_WithNullableValueType_NonNull_CreatesSome()
    {
        // Arrange
        int? value = 42;

        // Act
        var option = Option.From(value);

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void From_WithNullableValueType_Null_CreatesNone()
    {
        // Arrange
        int? value = null;

        // Act
        var option = Option.From(value);

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void Some_WithValue_CreatesSome()
    {
        // Arrange & Act
        var option = Option.Some(42);

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void None_Property_CreatesNone()
    {
        // Arrange & Act
        Option<string> option = Option.None;

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void AsOption_WithNonNullValue_CreatesSome()
    {
        // Arrange
        string value = "test";

        // Act
        var option = value.AsOption();

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal("test", option.Value);
    }

    [Fact]
    public void AsOption_WithNullValue_CreatesNone()
    {
        // Arrange
        string? value = null;

        // Act
        var option = value.AsOption();

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void AsOption_WithNullableValueType_NonNull_CreatesSome()
    {
        // Arrange
        int? value = 42;

        // Act
        var option = value.AsOption();

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(42, option.Value);
    }

    [Fact]
    public void AsOption_WithNullableValueType_Null_CreatesNone()
    {
        // Arrange
        int? value = null;

        // Act
        var option = value.AsOption();

        // Assert
        Assert.True(option.IsNone);
        Assert.False(option.IsSome);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void From_WithVariousIntegers_CreatesSome(int value)
    {
        // Act
        var option = Option.From((int?)value);

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal(value, option.Value);
    }

    [Fact]
    public void From_WithEmptyString_CreatesSome()
    {
        // Arrange & Act
        var option = Option.From("");

        // Assert
        Assert.True(option.IsSome);
        Assert.Equal("", option.Value);
    }

    [Fact]
    public void From_WithComplexType_WorksCorrectly()
    {
        // Arrange
        var complexObject = new TestClass { Id = 1, Name = "Test" };

        // Act
        var option = Option.From(complexObject);

        // Assert
        Assert.True(option.IsSome);
        Assert.Same(complexObject, option.Value);
    }

    [Fact]
    public void From_WithNullComplexType_CreatesNone()
    {
        // Arrange
        TestClass? complexObject = null;

        // Act
        var option = Option.From(complexObject);

        // Assert
        Assert.True(option.IsNone);
    }

    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
}
