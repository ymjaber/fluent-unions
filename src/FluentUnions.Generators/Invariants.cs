namespace FluentUnions.Generators
{
    /// <summary>
    /// Provides common constants and utility methods used across all FluentUnions source generators.
    /// </summary>
    /// <remarks>
    /// This class centralizes shared configuration and utilities to ensure consistency
    /// across all generated code.
    /// </remarks>
    public static class Invariants
    {
        /// <summary>
        /// The maximum number of elements supported in generated tuple overloads.
        /// </summary>
        /// <remarks>
        /// This value determines how many tuple elements are supported in generated extension methods.
        /// For example, with a value of 8, generators will create overloads for tuples with 2 to 8 elements.
        /// This aligns with C#'s built-in tuple support which provides special syntax for up to 8 elements.
        /// </remarks>
        public static readonly int MaxElements = 8;
        
        /// <summary>
        /// Creates an indentation string with the specified number of tab levels.
        /// </summary>
        /// <param name="count">The number of indentation levels (each level is 4 spaces).</param>
        /// <returns>A string containing the appropriate number of spaces for indentation.</returns>
        /// <remarks>
        /// This method ensures consistent indentation across all generated code,
        /// using 4 spaces per indentation level as per standard C# conventions.
        /// </remarks>
        public static string Tab(int count) => new string(' ', count * 4);
    }
}