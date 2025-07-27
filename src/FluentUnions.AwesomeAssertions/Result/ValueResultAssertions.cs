using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;
using FluentUnions;

namespace AwesomeAssertions;

/// <summary>
/// Contains assertion methods for <see cref="Result{T}"/> instances (results with a value).
/// </summary>
/// <typeparam name="T">The type of the value that may be contained in a successful result.</typeparam>
/// <remarks>
/// This class provides a fluent API for asserting the state and value of Result instances that
/// represent operations that can succeed with a value or fail with an error. It supports
/// assertions for both success and failure states, with value-based comparisons and custom
/// validation for successful results.
/// </remarks>
public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
{
    private readonly AssertionChain _assertionChain;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultAssertions{T}"/> class.
    /// </summary>
    /// <param name="subject">The result instance to assert against.</param>
    /// <param name="assertionChain">The assertion chain used to track assertion state.</param>
    public ResultAssertions(Result<T> subject, AssertionChain assertionChain) : base(subject, assertionChain)
    {
        _assertionChain = assertionChain;
    }

    /// <summary>
    /// Gets the identifier used in assertion failure messages.
    /// </summary>
    protected override string Identifier => "Result{T}";

    /// <summary>
    /// Asserts that the result represents a successful operation.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain assertions on the contained value.</returns>
    /// <exception>The result represents a failed operation.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Success(42);
    /// result.Should().Succeed()
    ///     .Which.Should().BeGreaterThan(0);
    /// </code>
    /// </example>
    public AndWhichConstraint<ResultAssertions<T>, T> Succeed(string because = "", params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.IsSuccess)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} to succeed, but it failed.");

        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the result is successful and contains a specific value.
    /// </summary>
    /// <param name="expectedValue">The expected value of the successful result.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The result is failed or contains a different value.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Success("Hello World");
    /// result.Should().SucceedWithValue("Hello World");
    /// </code>
    /// </example>
    public AndWhichConstraint<ResultAssertions<T>, T> SucceedWithValue(
        T expectedValue,
        string because = "",
        params object[] becauseArgs)
    {
        Succeed(because, becauseArgs);

        Subject.Value.Should().Be(expectedValue);
        
        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the result is successful and its value matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test the result's value against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The result is failed or the value doesn't match the predicate.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Success(42);
    /// result.Should().SucceedMatching(x => x > 40 &amp;&amp; x &lt; 50);
    /// </code>
    /// </example>
    public AndWhichConstraint<ResultAssertions<T>, T> SucceedMatching(Func<T, bool> predicate, string because = "", params object[] becauseArgs)
    {
        Succeed(because, becauseArgs);

        _assertionChain
            .ForCondition(predicate(Subject.Value))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} value to match the predicate, but it did not.");

        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the result is successful and its value satisfies the specified assertions.
    /// </summary>
    /// <param name="assertions">An action containing assertions to be performed on the result's value.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The result is failed or the contained assertions fail.</exception>
    /// <remarks>
    /// This method is useful for performing multiple assertions on the result value in a fluent manner.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Result.Success(new Product { Name = "Widget", Price = 9.99m });
    /// result.Should().SucceedSatisfying(product => 
    /// {
    ///     product.Name.Should().NotBeEmpty();
    ///     product.Price.Should().BePositive();
    /// });
    /// </code>
    /// </example>
    public AndWhichConstraint<ResultAssertions<T>, T> SucceedSatisfying(Action<T> assertions, string because = "", params object[] becauseArgs)
    {
        Succeed(because, becauseArgs);

        assertions(Subject.Value);

        return new AndWhichConstraint<ResultAssertions<T>, T>(this, Subject.Value);
    }

    /// <summary>
    /// Asserts that the result represents a failed operation and provides access to error assertions.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for asserting against the error details.</returns>
    /// <exception>The result represents a successful operation.</exception>
    /// <remarks>
    /// This method not only asserts that the result failed but also returns a specialized assertions
    /// object that allows for detailed error validation including error type, code, and message.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Result.Failure&lt;int&gt;(new NotFoundError("ITEM_404", "Item not found"));
    /// result.Should().Fail()
    ///     .WithErrorType&lt;NotFoundError&gt;()
    ///     .WithErrorCode("ITEM_404")
    ///     .WithErrorMessage("Item not found");
    /// </code>
    /// </example>
    public FailedResultAssertions Fail(string because = "", params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.IsFailure)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} to fail, but it succeeded.");

        return new FailedResultAssertions(Subject.Error, AssertionChain.GetOrCreate());
    }
}