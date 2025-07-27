using AwesomeAssertions.Execution;
using FluentUnions;

namespace AwesomeAssertions;

/// <summary>
/// Provides extension methods to enable fluent assertions for FluentUnions types.
/// </summary>
/// <remarks>
/// This class serves as the entry point for all FluentUnions assertions. By calling the Should() method
/// on Option or Result types, you gain access to a rich set of assertion methods specific to each type.
/// These extensions integrate seamlessly with the AwesomeAssertions framework.
/// 
/// Example usage:
/// <code>
/// // Option assertions
/// var option = Option.Some(42);
/// option.Should().BeSome();
/// option.Should().BeSomeWithValue(42);
/// 
/// // Result assertions
/// var result = Result.Success();
/// result.Should().Succeed();
/// 
/// // Result with value assertions
/// var valueResult = Result.Success("Hello");
/// valueResult.Should().SucceedWithValue("Hello");
/// </code>
/// </remarks>
public static class ShouldExtensions
{
    /// <summary>
    /// Returns an <see cref="OptionAssertions{T}"/> object that can be used to assert against an <see cref="Option{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the option.</typeparam>
    /// <param name="instance">The option instance to assert against.</param>
    /// <returns>An assertions object providing fluent assertion methods for the option.</returns>
    /// <example>
    /// <code>
    /// var option = Option.Some("test");
    /// option.Should().BeSome();
    /// option.Should().BeSomeWithValue("test");
    /// 
    /// var none = Option&lt;string&gt;.None;
    /// none.Should().BeNone();
    /// </code>
    /// </example>
    public static OptionAssertions<T> Should<T>(this Option<T> instance) where T : notnull => new(instance, AssertionChain.GetOrCreate());
 
    /// <summary>
    /// Returns a <see cref="ResultAssertions"/> object that can be used to assert against a <see cref="Result"/> instance.
    /// </summary>
    /// <param name="instance">The result instance to assert against.</param>
    /// <returns>An assertions object providing fluent assertion methods for the unit result.</returns>
    /// <example>
    /// <code>
    /// var success = Result.Success();
    /// success.Should().Succeed();
    /// 
    /// var failure = Result.Failure(new Error("ERR001", "Something went wrong"));
    /// failure.Should().Fail()
    ///     .WithErrorCode("ERR001")
    ///     .WithErrorMessage("Something went wrong");
    /// </code>
    /// </example>
    public static ResultAssertions Should(this Result instance) => new(instance, AssertionChain.GetOrCreate());
    
    /// <summary>
    /// Returns a <see cref="ResultAssertions{T}"/> object that can be used to assert against a <see cref="Result{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="instance">The result instance to assert against.</param>
    /// <returns>An assertions object providing fluent assertion methods for the value result.</returns>
    /// <example>
    /// <code>
    /// var success = Result.Success(42);
    /// success.Should().Succeed();
    /// success.Should().SucceedWithValue(42);
    /// 
    /// var failure = Result.Failure&lt;int&gt;(new ValidationError("VAL001", "Invalid input"));
    /// failure.Should().Fail()
    ///     .WithErrorType&lt;ValidationError&gt;()
    ///     .WithErrorCode("VAL001");
    /// </code>
    /// </example>
    public static ResultAssertions<T> Should<T>(this Result<T> instance) => new(instance, AssertionChain.GetOrCreate());
}