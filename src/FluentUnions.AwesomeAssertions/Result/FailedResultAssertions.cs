using AwesomeAssertions.Execution;
using AwesomeAssertions.Primitives;
using FluentUnions;

namespace AwesomeAssertions;

/// <summary>
/// Contains assertion methods for <see cref="Error"/> instances from failed results.
/// </summary>
/// <remarks>
/// This class provides a fluent API for asserting against error details when a Result has failed.
/// It supports various validations including error type, code, message content, and custom predicates.
/// All methods return the same FailedResultAssertions instance to enable method chaining.
/// </remarks>
public class FailedResultAssertions : ReferenceTypeAssertions<Error, FailedResultAssertions>
{
    private readonly AssertionChain _assertionChain;
    /// <summary>
    /// Gets the identifier used in assertion failure messages.
    /// </summary>
    protected override string Identifier => "Error";

    /// <summary>
    /// Initializes a new instance of the <see cref="FailedResultAssertions"/> class.
    /// </summary>
    /// <param name="subject">The error instance to assert against.</param>
    /// <param name="assertionChain">The assertion chain used to track assertion state.</param>
    public FailedResultAssertions(Error subject, AssertionChain assertionChain) : base(subject, assertionChain)
    {
        _assertionChain = assertionChain;
    }

    /// <summary>
    /// Asserts that the error is of a specific type.
    /// </summary>
    /// <typeparam name="TType">The expected error type.</typeparam>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not of the expected type.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new ValidationError("VAL001", "Invalid input"));
    /// result.Should().Fail()
    ///     .WithErrorType&lt;ValidationError&gt;();
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorType<TType>(
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.GetType() == typeof(TType))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} to be of type {0}, but found {1}.",
                typeof(TType),
                Subject.GetType());

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }
    
    /// <summary>
    /// Asserts that the error has a specific code.
    /// </summary>
    /// <param name="expectedCode">The expected error code.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error code does not match the expected value.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR_NETWORK", "Network timeout"));
    /// result.Should().Fail()
    ///     .WithErrorCode("ERR_NETWORK");
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorCode(
        string expectedCode,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.Code == expectedCode)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} code to be {0}, but found {1}.",
                expectedCode,
                Subject.Code);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }
    
    /// <summary>
    /// Asserts that the error has a specific message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error message does not match the expected value.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR001", "Operation failed"));
    /// result.Should().Fail()
    ///     .WithErrorMessage("Operation failed");
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorMessage(
        string expectedMessage,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.Message == expectedMessage)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} message to be {0}, but found {1}.",
                expectedMessage,
                Subject.Message);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }
    
    
    /// <summary>
    /// Asserts that the error equals the expected error instance.
    /// </summary>
    /// <param name="expectedError">The expected error instance.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error does not equal the expected error.</exception>
    /// <remarks>
    /// This method performs a full equality comparison including type, code, message, and metadata.
    /// </remarks>
    /// <example>
    /// <code>
    /// var expectedError = new ValidationError("VAL001", "Invalid email format");
    /// var result = Result.Failure(expectedError);
    /// result.Should().Fail()
    ///     .WithError(expectedError);
    /// </code>
    /// </example>
    public FailedResultAssertions WithError(
        Error expectedError,
        string because = "",
        params object[] becauseArgs)
    {

        Subject.Should().Be(expectedError, because, becauseArgs);
        
        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Asserts that the error message contains a specific substring.
    /// </summary>
    /// <param name="expectedSubstring">The substring that should be contained in the error message.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error message does not contain the expected substring.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR001", "Connection to database failed"));
    /// result.Should().Fail()
    ///     .WithErrorMessageContaining("database");
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorMessageContaining(
        string expectedSubstring,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.Message.Contains(expectedSubstring))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} message to contain {0}, but found {1}.",
                expectedSubstring,
                Subject.Message);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Asserts that the error message starts with a specific prefix.
    /// </summary>
    /// <param name="expectedPrefix">The prefix that the error message should start with.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error message does not start with the expected prefix.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR001", "Validation failed: Invalid email"));
    /// result.Should().Fail()
    ///     .WithErrorMessageStartingWith("Validation failed");
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorMessageStartingWith(
        string expectedPrefix,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.Message.StartsWith(expectedPrefix))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} message to start with {0}, but found {1}.",
                expectedPrefix,
                Subject.Message);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Asserts that the error message ends with a specific suffix.
    /// </summary>
    /// <param name="expectedSuffix">The suffix that the error message should end with.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error message does not end with the expected suffix.</exception>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR001", "Operation timed out after 30 seconds"));
    /// result.Should().Fail()
    ///     .WithErrorMessageEndingWith("30 seconds");
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorMessageEndingWith(
        string expectedSuffix,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(Subject.Message.EndsWith(expectedSuffix))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected failure {context:Error} message to end with {0}, but found {1}.",
                expectedSuffix,
                Subject.Message);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Asserts that the error message matches a custom predicate.
    /// </summary>
    /// <param name="predicate">A function to test the error message against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error message does not match the predicate.</exception>
    /// <remarks>
    /// This method is useful for complex message validation scenarios that can't be expressed
    /// with simple string comparisons.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Result.Failure(new Error("ERR001", "Failed after 3 retries"));
    /// result.Should().Fail()
    ///     .WithErrorMessageMatching(msg => msg.Contains("retries") &amp;&amp; msg.Any(char.IsDigit));
    /// </code>
    /// </example>
    public FailedResultAssertions WithErrorMessageMatching(
        Func<string, bool> predicate,
        string because = "",
        params object[] becauseArgs)
    {
        _assertionChain
            .ForCondition(predicate(Subject.Message))
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected failure {context:Error} message to match the predicate, but it did not. Message was: {0}.", Subject.Message);

        return new FailedResultAssertions(Subject, AssertionChain.GetOrCreate());
    }

    /// <summary>
    /// Asserts that the error is an AggregateError and returns specialized assertions for it.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>An <see cref="AndWhichConstraint{TParent,TSubject}"/> containing the AggregateError for further assertions.</returns>
    /// <exception>The error is not an AggregateError.</exception>
    /// <example>
    /// <code>
    /// var result = Result.BindAllAppend(failedResult1, failedResult2);
    /// result.Should().Fail()
    ///     .WithAggregateError()
    ///     .Which.Errors.Should().HaveCount(2);
    /// </code>
    /// </example>
    public AndWhichConstraint<FailedResultAssertions, AggregateError> WithAggregateError(
        string because = "",
        params object[] becauseArgs)
    {
        WithErrorType<AggregateError>(because, becauseArgs);
        return new AndWhichConstraint<FailedResultAssertions, AggregateError>(this, (AggregateError)Subject);
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing a specific number of errors.
    /// </summary>
    /// <param name="expectedCount">The expected number of errors in the aggregate.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or contains a different number of errors.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateMultipleFields();
    /// result.Should().Fail()
    ///     .WithAggregateErrorCount(3);
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorCount(
        int expectedCount,
        string because = "",
        params object[] becauseArgs)
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Count == expectedCount)
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain {0} error(s), but found {1}.",
                expectedCount,
                aggregate.Errors.Count);
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing an error of the specified type.
    /// </summary>
    /// <typeparam name="TError">The type of error that should be contained in the aggregate.</typeparam>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain an error of the specified type.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateUser(userData);
    /// result.Should().Fail()
    ///     .WithAggregateErrorContaining&lt;ValidationError&gt;();
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorContaining<TError>(
        string because = "",
        params object[] becauseArgs)
        where TError : Error
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Any(e => e.GetType() == typeof(TError)))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain an error of type {0}, but it did not.",
                typeof(TError).Name);
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing an error with the specified message.
    /// </summary>
    /// <param name="expectedMessage">The message that should be present in one of the aggregate's errors.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain an error with the specified message.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateForm(formData);
    /// result.Should().Fail()
    ///     .WithAggregateErrorContainingMessage("Email is required");
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorContainingMessage(
        string expectedMessage,
        string because = "",
        params object[] becauseArgs)
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Any(e => e.Message.Contains(expectedMessage)))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain an error with message containing {0}, but it did not.",
                expectedMessage);
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing a specific error.
    /// </summary>
    /// <param name="expectedError">The error that should be present in the aggregate.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain the expected error.</exception>
    /// <example>
    /// <code>
    /// var expectedError = new ValidationError("Email is required");
    /// var result = ValidateForm(formData);
    /// result.Should().Fail()
    ///     .WithAggregateErrorContaining(expectedError);
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorContaining(
        Error expectedError,
        string because = "",
        params object[] becauseArgs)
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Any(e => e.Equals(expectedError)))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain error {0}, but it did not.",
                expectedError);
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing an error with the specified code.
    /// </summary>
    /// <param name="expectedCode">The code that should be present in one of the aggregate's errors.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain an error with the specified code.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateMultipleOperations();
    /// result.Should().Fail()
    ///     .WithAggregateErrorContainingCode("VALIDATION_ERROR");
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorContainingCode(
        string expectedCode,
        string because = "",
        params object[] becauseArgs)
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Any(e => e.Code == expectedCode))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain an error with code {0}, but it did not.",
                expectedCode);
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing an error matching the specified predicate.
    /// </summary>
    /// <param name="errorPredicate">A function to test errors against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain an error matching the predicate.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateComplexOperation();
    /// result.Should().Fail()
    ///     .WithAggregateErrorMatching(error => 
    ///         error.Code == "TIMEOUT" &amp;&amp; error.Message.Contains("database"));
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorMatching(
        Func<Error, bool> errorPredicate,
        string because = "",
        params object[] becauseArgs)
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.Any(errorPredicate))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain an error matching the predicate, but it did not.");
                
        return this;
    }

    /// <summary>
    /// Asserts that the error is an AggregateError containing an error of the specified type matching the predicate.
    /// </summary>
    /// <typeparam name="TError">The type of error to match.</typeparam>
    /// <param name="errorPredicate">A function to test the typed error against a condition.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <paramref name="because"/>.</param>
    /// <returns>A <see cref="FailedResultAssertions"/> object for chaining additional assertions.</returns>
    /// <exception>The error is not an AggregateError or doesn't contain a matching error.</exception>
    /// <example>
    /// <code>
    /// var result = ValidateUserData();
    /// result.Should().Fail()
    ///     .WithAggregateErrorMatching&lt;ValidationError&gt;(error => 
    ///         error.Code == "INVALID_EMAIL" &amp;&amp; error.Message.StartsWith("Email"));
    /// </code>
    /// </example>
    public FailedResultAssertions WithAggregateErrorMatching<TError>(
        Func<TError, bool> errorPredicate,
        string because = "",
        params object[] becauseArgs)
        where TError : Error
    {
        var aggregate = WithAggregateError(because, becauseArgs).Which;
        
        _assertionChain
            .ForCondition(aggregate.Errors.OfType<TError>().Any(errorPredicate))
            .BecauseOf(because, becauseArgs)
            .FailWith(
                "Expected {context:AggregateError} to contain an error of type {0} matching the predicate, but it did not.",
                typeof(TError).Name);
                
        return this;
    }
}