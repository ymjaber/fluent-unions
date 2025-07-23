namespace FluentUnions.Analyzers.Common;

/// <summary>
/// Defines diagnostic categories used by FluentUnions analyzers to classify the types of issues they detect.
/// </summary>
/// <remarks>
/// These categories help developers and tools (like IDE filters) organize and prioritize diagnostics.
/// Each category represents a specific aspect of code quality that the analyzers evaluate.
/// </remarks>
public static class Categories
{
    /// <summary>
    /// Category for diagnostics related to incorrect or suboptimal usage of FluentUnions types.
    /// </summary>
    /// <remarks>
    /// This category includes issues such as accessing values without proper checks,
    /// comparing Option types with null, or using patterns that could lead to runtime errors.
    /// </remarks>
    public const string Usage = "Usage";
    
    /// <summary>
    /// Category for diagnostics related to design patterns and best practices with FluentUnions.
    /// </summary>
    /// <remarks>
    /// This category includes recommendations for using more idiomatic patterns,
    /// such as preferring Match over if-else chains or using None instead of default constructors.
    /// </remarks>
    public const string Design = "Design";
    
    /// <summary>
    /// Category for diagnostics related to performance considerations when using FluentUnions.
    /// </summary>
    /// <remarks>
    /// This category includes issues that might impact runtime performance,
    /// such as unnecessary allocations or inefficient patterns.
    /// </remarks>
    public const string Performance = "Performance";
}