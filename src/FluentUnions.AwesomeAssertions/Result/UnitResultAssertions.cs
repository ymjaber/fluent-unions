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

    /// <summary>
    /// Asserts that the result failed with the specified error.
    /// </summary>
    /// <param name="expectedError">The expected error that should match the result's error.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for asserting additional error details.</returns>
    /// <exception>The result is successful or contains a different error.</exception>
    /// <example>
    /// <code>
    /// var expectedError = new ValidationError("Input is required");
    /// var result = Result.Failure(expectedError);
    /// result.Should().FailWith(expectedError);
    /// </code>
    /// </example>
    public FailedResultAssertions FailWith(Error expectedError, string because = "", params object[] becauseArgs)
    {
        var assertions = Fail(because, becauseArgs);
        assertions.WithError(expectedError, because, becauseArgs);
        return assertions;
    }

    /// <summary>
    /// Asserts that the result failed with an error of the specified type.
    /// </summary>
    /// <typeparam name="TError">The expected type of the error.</typeparam>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for asserting additional error details.</returns>
    /// <exception>The result is successful or the error is not of the expected type.</exception>
    /// <remarks>
    /// This method provides a convenient way to assert the error type and continue with additional
    /// error assertions in a fluent manner.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new AuthenticationError("Invalid credentials"));
    /// result.Should().FailWith&lt;AuthenticationError&gt;()
    ///     .WithErrorMessage("Invalid credentials");
    /// </code>
    /// </example>
    public FailedResultAssertions FailWith<TError>(string because = "", params object[] becauseArgs)
        where TError : Error
    {
        var assertions = Fail(because, becauseArgs);
        assertions.WithErrorType<TError>(because, becauseArgs);
        return assertions;
    }

    /// <summary>
    /// Asserts that the result failed with an error matching the specified predicate.
    /// </summary>
    /// <param name="errorPredicate">A function to test the error against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for asserting additional error details.</returns>
    /// <exception>The result is successful or the error doesn't match the predicate.</exception>
    /// <remarks>
    /// This method allows for flexible error matching when you need to verify multiple properties
    /// of an error in a single assertion.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = ProcessRequest();
    /// result.Should().FailWith(error => 
    ///     error.Code == "TIMEOUT" &amp;&amp; 
    ///     error.Message.Contains("exceeded"));
    /// </code>
    /// </example>
    public FailedResultAssertions FailWith(Func<Error, bool> errorPredicate, string because = "", params object[] becauseArgs)
    {
        var assertions = Fail(because, becauseArgs);
        
        _assertionChain
            .ForCondition(errorPredicate(Subject.Error))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} error to match the predicate, but it did not.");
            
        return assertions;
    }

    /// <summary>
    /// Asserts that the result failed with an error of the specified type matching the predicate.
    /// </summary>
    /// <typeparam name="TError">The expected type of the error.</typeparam>
    /// <param name="errorPredicate">A function to test the typed error against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for asserting additional error details.</returns>
    /// <exception>The result is successful, the error is not of the expected type, or doesn't match the predicate.</exception>
    /// <remarks>
    /// This method combines type checking with custom predicate matching, providing type-safe access
    /// to error-specific properties in the predicate.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = AuthorizeUser(userId);
    /// result.Should().FailWith&lt;AuthorizationError&gt;(error => 
    ///     error.Code == "FORBIDDEN" &amp;&amp; 
    ///     error.Message.Contains("insufficient permissions"));
    /// </code>
    /// </example>
    public FailedResultAssertions FailWith<TError>(Func<TError, bool> errorPredicate, string because = "", params object[] becauseArgs)
        where TError : Error
    {
        var assertions = Fail(because, becauseArgs);
        assertions.WithErrorType<TError>(because, becauseArgs);
        
        _assertionChain
            .ForCondition(errorPredicate((TError)Subject.Error))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:Result} error to match the predicate, but it did not.");
            
        return assertions;
    }
}