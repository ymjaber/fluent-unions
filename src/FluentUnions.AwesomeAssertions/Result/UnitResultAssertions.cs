using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;
using FluentUnions;

namespace AwesomeAssertions;

/// <summary>
/// Contains assertion methods for <see cref="Result"/> instances (unit results without a value).
/// </summary>
/// <remarks>
/// This class provides a fluent API for asserting the state of Result instances that represent
/// operations that can succeed or fail but don't return a value on success. It supports
/// assertions for both success and failure states, with detailed error assertions available
/// through the Fail() method.
/// </remarks>
public class ResultAssertions : ReferenceTypeAssertions<Result, ResultAssertions>
{
    private readonly AssertionChain _assertionChain;
    /// <summary>
    /// Gets the identifier used in assertion failure messages.
    /// </summary>
    protected override string Identifier => "Result";

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultAssertions"/> class.
    /// </summary>
    /// <param name="subject">The result instance to assert against.</param>
    /// <param name="assertionChain">The assertion chain used to track assertion state.</param>
    public ResultAssertions(Result subject, AssertionChain assertionChain) : base(subject, assertionChain)
    {
        _assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the result represents a successful operation.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain additional assertions.</returns>
    /// <exception>The result represents a failed operation.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Success();
    /// result.Should().Succeed("the operation completed without errors");
    /// </code>
    /// </example>
    public AndConstraint<ResultAssertions> Succeed(string because = "", params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.IsSuccess)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} to succeed, but it failed.");

        return new AndConstraint<ResultAssertions>(this);
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
    /// var result = Result.Failure(new ValidationError("VAL001", "Invalid input"));
    /// result.Should().Fail()
    ///     .WithErrorType&lt;ValidationError&gt;()
    ///     .WithErrorCode("VAL001")
    ///     .WithErrorMessage("Invalid input");
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