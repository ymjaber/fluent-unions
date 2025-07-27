namespace FluentUnions.Analyzers.Common;

/// <summary>
/// Defines unique diagnostic IDs for all FluentUnions analyzers.
/// </summary>
/// <remarks>
/// Diagnostic IDs follow a consistent pattern:
/// - FU0001-FU0099: Option-related diagnostics
/// - FU0101-FU0199: Result-related diagnostics
/// Each ID uniquely identifies a specific diagnostic rule that can be suppressed or configured independently.
/// </remarks>
public static class DiagnosticIds
{
    // Option<T> analyzers
    
    /// <summary>
    /// Diagnostic ID for detecting unsafe access to Option.Value without checking if the option has a value.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when code accesses the Value property of an Option without first
    /// verifying that IsSome is true, which could lead to an InvalidOperationException at runtime.
    /// </remarks>
    public const string OptionValueAccessWithoutCheck = "FU0001";
    
    /// <summary>
    /// Diagnostic ID for suggesting the use of Match method instead of if-else chains with Option types.
    /// </summary>
    /// <remarks>
    /// This diagnostic promotes more functional and idiomatic code by suggesting Match
    /// when developers use if (option.IsSome) else patterns.
    /// </remarks>
    public const string OptionPreferMatchOverIfElse = "FU0002";
    
    /// <summary>
    /// Diagnostic ID for detecting null comparisons with Option types.
    /// </summary>
    /// <remarks>
    /// Since Option is a value type (struct), it can never be null. This diagnostic warns
    /// when code compares an Option with null and suggests using IsNone or IsSome instead.
    /// </remarks>
    public const string OptionNullComparison = "FU0003";
    
    /// <summary>
    /// Diagnostic ID for suggesting Option.None instead of default(Option) or new Option().
    /// </summary>
    /// <remarks>
    /// This diagnostic promotes clearer intent by suggesting the explicit use of Option.None
    /// when creating an empty option value.
    /// </remarks>
    public const string OptionPreferNoneOverDefault = "FU0004";
    
    /// <summary>
    /// Diagnostic ID for detecting misuse of Option.None when assigned to non-generic Option variable.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when Option.None is assigned to a variable of type Option (non-generic)
    /// instead of being used directly or assigned to a properly typed Option&lt;T&gt; variable.
    /// </remarks>
    public const string OptionNoneMisuse = "FU0005";
    
    /// <summary>
    /// Diagnostic ID for detecting Option values that are created but never consumed or checked.
    /// </summary>
    /// <remarks>
    /// This diagnostic helps prevent bugs where operations that return Option are called
    /// but their value is never examined, potentially ignoring important data.
    /// </remarks>
    public const string UnhandledOption = "FU0006";
    
    /// <summary>
    /// Diagnostic ID for detecting FilterBuilder that is not properly completed with Build() or implicit conversion.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when a FilterBuilder is assigned to a variable, returned from a method,
    /// or otherwise used without being completed with Build() or an implicit conversion to Option.
    /// </remarks>
    public const string FilterBuilderMisuse = "FU0007";
    
    // Result<T> analyzers
    
    /// <summary>
    /// Diagnostic ID for detecting unsafe access to Result.Value without checking if the result is successful.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when code accesses the Value property of a Result without first
    /// verifying that IsSuccess is true, which could lead to an InvalidOperationException at runtime.
    /// </remarks>
    public const string ResultValueAccessWithoutCheck = "FU0101";
    
    /// <summary>
    /// Diagnostic ID for detecting unsafe access to Result.Error without checking if the result has failed.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when code accesses the Error property of a Result without first
    /// verifying that IsFailure is true, which could lead to an InvalidOperationException at runtime.
    /// </remarks>
    public const string ResultErrorAccessWithoutCheck = "FU0102";
    
    /// <summary>
    /// Diagnostic ID for suggesting the use of Match method instead of if-else chains with Result types.
    /// </summary>
    /// <remarks>
    /// This diagnostic promotes more functional and idiomatic code by suggesting Match
    /// when developers use if (result.IsSuccess) else patterns.
    /// </remarks>
    public const string ResultPreferMatchOverIfElse = "FU0103";
    
    /// <summary>
    /// Diagnostic ID for detecting Result values that are created but never consumed or checked.
    /// </summary>
    /// <remarks>
    /// This diagnostic helps prevent bugs where operations that return Result are called
    /// but their success/failure status is never examined, potentially ignoring errors.
    /// </remarks>
    public const string UnhandledResult = "FU0104";
    
    /// <summary>
    /// Diagnostic ID for detecting EnsureBuilder that is not properly completed with Build() or implicit conversion.
    /// </summary>
    /// <remarks>
    /// This diagnostic warns when an EnsureBuilder is assigned to a variable, returned from a method,
    /// or otherwise used without being completed with Build(), Map(), Bind(), or an implicit conversion to Result.
    /// </remarks>
    public const string EnsureBuilderMisuse = "FU0105";
}