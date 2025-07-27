using System.Diagnostics;

namespace FluentUnions.Debugging;

/// <summary>
/// Provides a debug view for <see cref="Option{T}"/> types in the Visual Studio debugger.
/// </summary>
/// <typeparam name="T">The type of the value in the option.</typeparam>
/// <remarks>
/// This class is used internally by the debugger to provide a better visualization
/// of Option values. It shows whether the option has a value and what that value is.
/// </remarks>
[DebuggerDisplay("IsSome = {IsSome}, Value = {_valueString}")]
internal sealed class OptionDebugView<T>
where T : notnull
{
    private readonly Option<T> _option;
    private readonly string _valueString;

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionDebugView{T}"/> class.
    /// </summary>
    /// <param name="option">The option to create a debug view for.</param>
    public OptionDebugView(Option<T> option)
    {
        _option = option;
        _valueString = option.IsSome ? option.Value?.ToString() ?? "null" : "None";
    }

    /// <summary>
    /// Gets a value indicating whether the option has a value.
    /// </summary>
    public bool IsSome => _option.IsSome;
    /// <summary>
    /// Gets the value of the option if it has one; otherwise, the default value of <typeparamref name="T"/>.
    /// </summary>
    public T? Value => _option.IsSome ? _option.Value : default;
}

/// <summary>
/// Provides a debug view for <see cref="Result{T}"/> types in the Visual Studio debugger.
/// </summary>
/// <typeparam name="T">The type of the value in the result.</typeparam>
/// <remarks>
/// This class is used internally by the debugger to provide a better visualization
/// of Result values. It shows whether the result is successful and either the value or error.
/// </remarks>
[DebuggerDisplay("IsSuccess = {IsSuccess}, {_displayString}")]
internal sealed class ResultDebugView<T>
{
    private readonly Result<T> _result;
    private readonly string _displayString;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultDebugView{T}"/> class.
    /// </summary>
    /// <param name="result">The result to create a debug view for.</param>
    public ResultDebugView(Result<T> result)
    {
        _result = result;
        _displayString = result.IsSuccess 
            ? $"Value = {result.Value?.ToString() ?? "null"}"
            : $"Error = {result.Error}";
    }

    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    public bool IsSuccess => _result.IsSuccess;
    /// <summary>
    /// Gets the value of the result if successful; otherwise, the default value of <typeparamref name="T"/>.
    /// </summary>
    public T? Value => _result.IsSuccess ? _result.Value : default;
    /// <summary>
    /// Gets the error of the result if failed; otherwise, null.
    /// </summary>
    public Error? Error => _result.IsFailure ? _result.Error : null;
}
