namespace FluentUnions.AwesomeAssertions.Tests.OptionTests;

public class OptionAssertionsTests
{
    #region BeSome Tests

    [Fact]
    public void BeSome_WhenOptionHasValue_ShouldPass()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        option.Should().BeSome();
    }

    [Fact]
    public void BeSome_WhenOptionIsNone_ShouldFail()
    {
        // Arrange
        var option = Option<int>.None;

        // Act & Assert
        var act = () => option.Should().BeSome();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option to have a value, but found none.");
    }

    [Fact]
    public void BeSome_ShouldReturnAndWhichConstraint()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        option.Should().BeSome()
            .Which.Should().Be(42);
    }

    #endregion

    #region BeSomeWithValue Tests

    [Fact]
    public void BeSomeWithValue_WhenValueMatches_ShouldPass()
    {
        // Arrange
        var option = Option.Some("Hello");

        // Act & Assert
        option.Should().BeSomeWithValue("Hello");
    }

    [Fact]
    public void BeSomeWithValue_WhenValueDoesNotMatch_ShouldFail()
    {
        // Arrange
        var option = Option.Some("Hello");

        // Act & Assert
        var act = () => option.Should().BeSomeWithValue("World");
        
        act.Should().Throw<XunitException>()
            .WithMessage("*Expected*World*but*Hello*");
    }

    [Fact]
    public void BeSomeWithValue_WhenOptionIsNone_ShouldFail()
    {
        // Arrange
        var option = Option<string>.None;

        // Act & Assert
        var act = () => option.Should().BeSomeWithValue("Hello");
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option to have a value, but found none.");
    }

    #endregion

    #region BeSomeMatching Tests

    [Fact]
    public void BeSomeMatching_WhenPredicateMatches_ShouldPass()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        option.Should().BeSomeMatching(x => x > 40 && x < 50);
    }

    [Fact]
    public void BeSomeMatching_WhenPredicateDoesNotMatch_ShouldFail()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        var act = () => option.Should().BeSomeMatching(x => x < 40);
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option value to match the predicate, but it did not.");
    }

    [Fact]
    public void BeSomeMatching_WhenOptionIsNone_ShouldFail()
    {
        // Arrange
        var option = Option<int>.None;

        // Act & Assert
        var act = () => option.Should().BeSomeMatching(x => true);
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option to have a value, but found none.");
    }

    #endregion

    #region NotBeSomeWithValue Tests

    [Fact]
    public void NotBeSomeWithValue_WhenValueIsDifferent_ShouldPass()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        option.Should().NotBeSomeWithValue(100);
    }

    [Fact]
    public void NotBeSomeWithValue_WhenOptionIsNone_ShouldPass()
    {
        // Arrange
        var option = Option<int>.None;

        // Act & Assert
        option.Should().NotBeSomeWithValue(42);
    }

    [Fact]
    public void NotBeSomeWithValue_WhenValueMatches_ShouldFail()
    {
        // Arrange
        var option = Option.Some(42);

        // Act & Assert
        var act = () => option.Should().NotBeSomeWithValue(42);
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option not to have value 42, but found it.");
    }

    #endregion

    #region BeSomeSatisfying Tests

    [Fact]
    public void BeSomeSatisfying_WhenAssertionsPass_ShouldPass()
    {
        // Arrange
        var option = Option.Some(new Person { Name = "Yousef", Age = 30 });

        // Act & Assert
        option.Should().BeSomeSatisfying(person =>
        {
            person.Name.Should().Be("Yousef");
            person.Age.Should().Be(30);
        });
    }

    [Fact]
    public void BeSomeSatisfying_WhenAssertionsFail_ShouldFail()
    {
        // Arrange
        var option = Option.Some(new Person { Name = "Yousef", Age = 30 });

        // Act & Assert
        var act = () => option.Should().BeSomeSatisfying(person =>
        {
            person.Name.Should().Be("Ahmed");
        });
        
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void BeSomeSatisfying_WhenOptionIsNone_ShouldFail()
    {
        // Arrange
        var option = Option<Person>.None;

        // Act & Assert
        var act = () => option.Should().BeSomeSatisfying(person =>
        {
            person.Name.Should().NotBeNull();
        });
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option to have a value, but found none.");
    }

    #endregion

    #region BeNone Tests

    [Fact]
    public void BeNone_WhenOptionIsNone_ShouldPass()
    {
        // Arrange
        var option = Option<string>.None;

        // Act & Assert
        option.Should().BeNone();
    }

    [Fact]
    public void BeNone_WhenOptionHasValue_ShouldFail()
    {
        // Arrange
        var option = Option.Some("Hello");

        // Act & Assert
        var act = () => option.Should().BeNone();
        
        act.Should().Throw<XunitException>()
            .WithMessage("Expected option to have no value, but found one.");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Option_FromNullableValue_ShouldWorkWithAssertions()
    {
        // Arrange
        string? nullableValue = null;
        var noneOption = nullableValue.AsOption();
        
        string? someValue = "Hello";
        var someOption = someValue.AsOption();

        // Act & Assert
        noneOption.Should().BeNone();
        someOption.Should().BeSomeWithValue("Hello");
    }

    [Fact]
    public void Option_WithComplexType_ShouldWorkWithAssertions()
    {
        // Arrange
        var person = new Person { Name = "Yousef", Age = 30 };
        var option = Option.Some(person);

        // Act & Assert
        option.Should().BeSome()
            .Which.Name.Should().Be("Yousef");
        
        option.Should().BeSomeSatisfying(p =>
        {
            p.Name.Should().StartWith("Y");
            p.Age.Should().BeGreaterThan(18);
        });
    }

    #endregion

    private class Person
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
    }
}