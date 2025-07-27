using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;
using FluentUnions;

namespace AwesomeAssertions;

/// <summary>
/// Contains assertion methods for <see cref="Option{T}"/> instances.
/// </summary>
/// <typeparam name="T">The type of the value that may be contained in the option.</typeparam>
/// <remarks>
/// This class provides a fluent API for asserting the state and value of Option instances.
/// It supports assertions for both Some and None states, as well as value-based comparisons
/// and custom predicate matching.
/// </remarks>
public class OptionAssertions<T> : ReferenceTypeAssertions<Option<T>, OptionAssertions<T>>
    where T : notnull
{
    private readonly AssertionChain _assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAssertions{T}"/> class.
    /// </summary>
    /// <param name="subject">The option instance to assert against.</param>
    /// <param name="assertionChain">The assertion chain used to track assertion state.</param>
    public OptionAssertions(Option<T> subject, AssertionChain assertionChain) : base(subject, assertionChain)
    {
        _assertionChain = assertionChain;
    }

    /// <summary>
    /// Gets the identifier used in assertion failure messages.
    /// </summary>
    protected override string Identifier => "option{T}";

    /// <summary>
    /// Asserts that the option has a value (is in the Some state).
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain assertions on the contained value.</returns>
    /// <exception>The option is None.</exception>
    /// <example>
    /// <code>
    /// var option = Option.Some(42);
    /// option.Should().BeSome("we expect a value to be present")
    ///     .Which.Should().BeGreaterThan(0);
    /// </code>
    /// </example>
    public AndWhichConstraint<OptionAssertions<T>, T> BeSome(string because = "", params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.IsSome)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:option} to have a value, but found none.");

        return new AndWhichConstraint<OptionAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the option has a specific value.
    /// </summary>
    /// <param name="expectedValue">The expected value contained in the option.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The option is None or contains a different value.</exception>
    /// <example>
    /// <code>
    /// var option = Option.Some("Hello");
    /// option.Should().BeSomeWithValue("Hello");
    /// </code>
    /// </example>
    public AndWhichConstraint<OptionAssertions<T>, T> BeSomeWithValue(T expectedValue, string because = "",
        params object[] becauseArgs)
    {
        BeSome(because, becauseArgs);

        Subject.Value.Should().Be(expectedValue);

        return new AndWhichConstraint<OptionAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the option has a value that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test the option's value against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The option is None or the value doesn't match the predicate.</exception>
    /// <example>
    /// <code>
    /// var option = Option.Some(42);
    /// option.Should().BeSomeMatching(x => x > 40 &amp;&amp; x &lt; 50);
    /// </code>
    /// </example>
    public AndWhichConstraint<OptionAssertions<T>, T> BeSomeMatching(Func<T, bool> predicate, string because = "", params object[] becauseArgs)
    {
        BeSome(because, becauseArgs);

        _assertionChain
            .ForCondition(predicate(Subject.Value))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:option} value to match the predicate, but it did not.");

        return new AndWhichConstraint<OptionAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the option does not contain a specific value.
    /// </summary>
    /// <param name="unexpectedValue">The value that should not be contained in the option.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The option contains the unexpected value.</exception>
    /// <remarks>
    /// This assertion passes if the option is None or if it contains a different value than the unexpected one.
    /// </remarks>
    /// <example>
    /// <code>
    /// var option = Option.Some(42);
    /// option.Should().NotBeSomeWithValue(100);
    /// 
    /// var none = Option&lt;int&gt;.None;
    /// none.Should().NotBeSomeWithValue(42); // Also passes
    /// </code>
    /// </example>
    public AndConstraint<OptionAssertions<T>> NotBeSomeWithValue(T unexpectedValue, string because = "", params object[] becauseArgs)
    {
        if (Subject.IsNone)
        {
            return new AndConstraint<OptionAssertions<T>>(this);
        }

        _assertionChain
            .ForCondition(!Subject.Value.Equals(unexpectedValue))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:option} not to have value {0}, but found it.", unexpectedValue);

        return new AndConstraint<OptionAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the option has a value and that the value satisfies the specified assertions.
    /// </summary>
    /// <param name="assertions">An action containing assertions to be performed on the option's value.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The option is None or the contained assertions fail.</exception>
    /// <remarks>
    /// This method is useful for performing multiple assertions on the contained value in a fluent manner.
    /// </remarks>
    /// <example>
    /// <code>
    /// var option = Option.Some(new Person { Name = "John", Age = 30 });
    /// option.Should().BeSomeSatisfying(person => 
    /// {
    ///     person.Name.Should().Be("John");
    ///     person.Age.Should().BeGreaterThan(18);
    /// });
    /// </code>
    /// </example>
    public AndWhichConstraint<OptionAssertions<T>, T> BeSomeSatisfying(Action<T> assertions, string because = "", params object[] becauseArgs)
    {
        BeSome(because, becauseArgs);

        assertions(Subject.Value);

        return new AndWhichConstraint<OptionAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the option has no value (is in the None state).
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The option has a value.</exception>
    /// <example>
    /// <code>
    /// var option = Option&lt;string&gt;.None;
    /// option.Should().BeNone("we expect no value to be present");
    /// </code>
    /// </example>
    public AndConstraint<OptionAssertions<T>> BeNone(string because = "", params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.IsNone)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:option} to have no value, but found one.");

        return new AndConstraint<OptionAssertions<T>>(this);
    }
}