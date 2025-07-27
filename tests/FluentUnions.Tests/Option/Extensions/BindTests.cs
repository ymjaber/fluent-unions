namespace FluentUnions.Tests.OptionTests.Extensions;

public class BindTests
{
    [Fact]
    public void Bind_WhenSome_AppliesFunction()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = option.Bind(x => Option.Some(x * 2));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void Bind_WhenNone_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = option.Bind(x => Option.Some(x * 2));

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Bind_WhenFunctionReturnsNone_ReturnsNone()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = option.Bind(x => Option<int>.None);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Bind_ChainedOperations_PropagatesNone()
    {
        // Arrange
        var option = Option.Some(10);

        // Act
        var result = option
            .Bind(x => x > 5 ? Option.Some(x) : Option<int>.None)
            .Bind(x => x < 20 ? Option.Some(x * 2) : Option<int>.None)
            .Bind(x => Option.Some(x.ToString()));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("20", result.Value);
    }

    [Fact]
    public void Bind_ChainedOperations_StopsAtFirstNone()
    {
        // Arrange
        var option = Option.Some(3);

        // Act
        var result = option
            .Bind(x => x > 5 ? Option.Some(x) : Option<int>.None) // Returns None
            .Bind(x => Option.Some(x * 2)) // Not executed
            .Bind(x => Option.Some(x.ToString())); // Not executed

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Bind_ChangesType()
    {
        // Arrange
        var option = Option.Some("42");

        // Act
        var result = option.Bind(str => 
            int.TryParse(str, out var num) 
                ? Option.Some(num) 
                : Option<int>.None);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Bind_WithInvalidParse_ReturnsNone()
    {
        // Arrange
        var option = Option.Some("not a number");

        // Act
        var result = option.Bind(str => 
            int.TryParse(str, out var num) 
                ? Option.Some(num) 
                : Option<int>.None);

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public void Bind_WithComplexType()
    {
        // Arrange
        var option = Option.Some(new User { Id = 1, Email = "test@example.com" });

        // Act
        var result = option.Bind(user => 
            user.Email.Contains("@") 
                ? Option.Some(user.Email.Split('@')[1]) 
                : Option<string>.None);

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("example.com", result.Value);
    }

    [Fact]
    public void Bind_SimulatesNullableLookup()
    {
        // Arrange
        var users = new Dictionary<int, User>
        {
            [1] = new User { Id = 1, Email = "user1@example.com" },
            [2] = new User { Id = 2, Email = "user2@example.com" }
        };

        Option<User> GetUser(int id) => 
            users.TryGetValue(id, out var user) 
                ? Option.Some(user) 
                : Option<User>.None;

        // Act
        var result = Option.Some(1)
            .Bind(GetUser)
            .Bind(user => Option.Some(user.Email));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("user1@example.com", result.Value);
    }

    [Fact]
    public void Bind_SimulatesNullableLookup_NotFound()
    {
        // Arrange
        var users = new Dictionary<int, User>();

        Option<User> GetUser(int id) => 
            users.TryGetValue(id, out var user) 
                ? Option.Some(user) 
                : Option<User>.None;

        // Act
        var result = Option.Some(1)
            .Bind(GetUser)
            .Bind(user => Option.Some(user.Email));

        // Assert
        Assert.True(result.IsNone);
    }

    private class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
    }
}