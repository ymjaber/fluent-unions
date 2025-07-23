using System.Threading.Tasks;
using FluentUnions;
using Xunit;

namespace FluentUnions.Tests.OptionTests.Extensions;

public class AsyncExtensionTests
{
    [Fact]
    public async Task MapAsync_WhenSome_TransformsValue()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = await option.MapAsync((Func<int, Task<int>>)(async x =>
        {
            await Task.Delay(1);
            return x * 2;
        }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public async Task MapAsync_WhenNone_ReturnsNone()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = await option.MapAsync((Func<int, Task<int>>)(async x =>
        {
            await Task.Delay(1);
            return x * 2;
        }));

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task BindAsync_WhenSome_AppliesAsyncFunction()
    {
        // Arrange
        var option = Option.Some("42");

        // Act
        var result = await option.BindAsync((Func<string, Task<Option<int>>>)(async str =>
        {
            await Task.Delay(1);
            return int.TryParse(str, out var num)
                ? Option.Some(num)
                : Option<int>.None;
        }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task MatchAsync_WhenSome_ExecutesAsyncSomeFunction()
    {
        // Arrange
        var option = Option.Some(10);

        // Act
        var result = await option.MatchAsync(
            some: (Func<int, Task<int>>)(async value =>
            {
                await Task.Delay(1);
                return value * 2;
            }),
            none: (Func<Task<int>>)(async () =>
            {
                await Task.Delay(1);
                return 0;
            })
        );

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public async Task MatchAsync_WhenNone_ExecutesAsyncNoneFunction()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = await option.MatchAsync(
            some: (Func<int, Task<int>>)(async value =>
            {
                await Task.Delay(1);
                return value * 2;
            }),
            none: (Func<Task<int>>)(async () =>
            {
                await Task.Delay(1);
                return -1;
            })
        );

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public async Task FilterAsync_WhenPredicateTrue_ReturnsSome()
    {
        // Arrange
        var option = Option.Some(10);

        // Act
        var result = await option.FilterAsync((Func<int, Task<bool>>)(async x =>
        {
            await Task.Delay(1);
            return x > 5;
        }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public async Task FilterAsync_WhenPredicateFalse_ReturnsNone()
    {
        // Arrange
        var option = Option.Some(3);

        // Act
        var result = await option.FilterAsync((Func<int, Task<bool>>)(async x =>
        {
            await Task.Delay(1);
            return x > 5;
        }));

        // Assert
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task OrElseAsync_WhenNone_ExecutesAsyncFallback()
    {
        // Arrange
        var option = Option<int>.None;

        // Act
        var result = await option.OrElseAsync((Func<Task<Option<int>>>)(async () =>
        {
            await Task.Delay(1);
            return Option.Some(42);
        }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task ChainedAsyncOperations()
    {
        // Arrange
        var option = Option.Some(5);

        // Act
        var result = await option
            .MapAsync((Func<int, Task<int>>)(async x =>
            {
                await Task.Delay(1);
                return x * 2;
            }))
            .BindAsync((Func<int, Task<Option<string>>>)(async x =>
            {
                await Task.Delay(1);
                return x > 5 ? Option.Some(x.ToString()) : Option<string>.None;
            }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("10", result.Value);
    }

    [Fact]
    public async Task AsyncOperations_WithValueTask()
    {
        // Arrange
        var option = Option.Some(42);

        // Act
        var result = await option.MapAsync((Func<int, Task<int>>)(async x =>
        {
            await Task.CompletedTask;
            return x * 2;
        }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal(84, result.Value);
    }

    [Fact]
    public async Task AsyncExtensions_SimulateApiCall()
    {
        // Arrange
        async Task<Option<User>> GetUserAsync(int id)
        {
            await Task.Delay(1);
            return id == 1 
                ? Option.Some(new User { Id = 1, Name = "John" })
                : Option<User>.None;
        }

        // Act
        var result = await Option.Some(1)
            .BindAsync(GetUserAsync)
            .MapAsync((Func<User, Task<string>>)(async user =>
            {
                await Task.Delay(1);
                return user.Name.ToUpper();
            }));

        // Assert
        Assert.True(result.IsSome);
        Assert.Equal("JOHN", result.Value);
    }

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}