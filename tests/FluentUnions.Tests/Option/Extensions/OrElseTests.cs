namespace FluentUnions.Tests.OptionTests.Extensions;

public class OrElseTests
{
    [Fact]
    public void OrElse_WhenSome_ReturnsOriginalValue()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = option.OrElse(() => Option.Some(100));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void OrElse_WhenNone_ReturnsFallbackValue()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.OrElse(() => Option.Some(100));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(100, result.Value);
    }

    [Fact]
    public void OrElse_WhenNone_AndFallbackIsNone_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.OrElse(() => Option<int>.None);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void OrElse_ChainedFallbacks()
    {
        // Arrange
        var option1 = Option<int>.None;
        var option2 = Option<int>.None;
        var option3 = Option.Some(42);

        // Act
        var result = option1
            .OrElse(() => option2)
            .OrElse(() => option3);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void OrElse_LazyEvaluation_NotCalledWhenSome()
    {
        // Arrange
        var option = Option.Some(42);
        var fallbackCalled = false;

        // Act
        var result = option.OrElse(() =>
        {
            fallbackCalled = true;
            return Option.Some(100);
        });

        // Assert
        Assert.False(fallbackCalled);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void OrElse_LazyEvaluation_CalledWhenNone()
    {
        // Arrange
        var option = Option<int>.None;
        var fallbackCalled = false;

        // Act
        var result = option.OrElse(() =>
        {
            fallbackCalled = true;
            return Option.Some(100);
        });

        // Assert
        Assert.True(fallbackCalled);
        Assert.Equal(100, result.Value);
    }

    [Fact]
    public void OrElse_WithComplexType()
    {
        // Arrange
        var primary = Option<User>.None;
        var fallback = new User { Id = 1, Name = "Default User" };

        // Act
        var result = primary.OrElse(() => Option.Some(fallback));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("Default User", result.Value.Name);
    }

    [Fact]
    public void OrElse_UsedForDefaultValues()
    {
        // Arrange
        Option<string> GetConfigValue(string key) => Option<string>.None;

        // Act
        var connectionString = GetConfigValue("ConnectionString")
            .OrElse(() => GetConfigValue("DefaultConnectionString"))
            .OrElse(() => Option.Some("Server=localhost;Database=Test"));

        // Assert
        Assert.True(connectionString.IsSome);
        Assert.Equal("Server=localhost;Database=Test", connectionString.Value);
    }

    [Fact]
    public void OrElse_CombinedWithMap()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option
            .OrElse(() => Option.Some(5))
            .Map(x => x * 2);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void OrElse_MultipleSources()
    {
        // Arrange
        var sources = new[]
        {
            Option<string>.None,
            Option<string>.None,
            Option.Some("Found!"),
            Option.Some("Not used")
        };

        // Act
        var result = sources[0]
            .OrElse(() => sources[1])
            .OrElse(() => sources[2])
            .OrElse(() => sources[3]);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("Found!", result.Value);
    }

    [Fact]
    public void OrElse_WithDifferentFallbackLogic()
    {
        // Arrange
        var userInput = Option<int>.None;
        var environmentValue = Option<int>.None;
        var configValue = Option<int>.None;

        // Act
        var port = userInput
            .OrElse(() => environmentValue)
            .OrElse(() => configValue)
            .OrElse(() => Option.Some(8080)); // Default port

        // Assert
        Assert.True(port.IsSome);
        Assert.Equal(8080, port.Value);
    }

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}