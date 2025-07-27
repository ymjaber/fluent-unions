using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates BindAll factory methods for combining multiple Result values with error accumulation.
    /// </summary>
    /// <remarks>
    /// This generator produces static factory methods on the Result struct that combine multiple
    /// Result values into a single Result, accumulating all errors if multiple failures occur.
    /// It generates overloads for combining 2 to MaxElements Results.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Result.BindAll&lt;T1, T2&gt;(
    ///     in Result&lt;T1&gt; result1,
    ///     in Result&lt;T2&gt; result2)
    /// returns Result&lt;(T1, T2)&gt;
    /// </code>
    /// 
    /// The BindAll factory method differs from Bind in that:
    /// - It accepts Result values directly (not functions returning Results)
    /// - It accumulates ALL errors from failed Results, not just the first one
    /// - Uses ErrorBuilder to combine multiple errors into a composite error
    /// - If all Results are successful, combines their values into a tuple Result
    /// 
    /// This is useful for:
    /// - Validation scenarios where you want to collect all validation errors
    /// - Combining pre-computed Result values
    /// - Scenarios where knowing all failures is more valuable than fail-fast behavior
    /// 
    /// The 'in' parameter modifier is used for performance optimization with value types.
    /// </remarks>
    [Generator]
    public class BindAllFactoryGenerator : IIncrementalGenerator
    {
        private static string GetOrdinal(int number) => number switch
        {
            1 => "first",
            2 => "second",
            3 => "third",
            4 => "fourth",
            5 => "fifth",
            6 => "sixth",
            7 => "seventh",
            8 => "eighth",
            9 => "ninth",
            10 => "tenth",
            _ => $"{number}th"
        };

        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var className = "ValueResult.BindAllFactory";
                var fileName = $"{className}.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic readonly partial struct Result\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n", Enumerable.Range(1, i).Select(n => $"{Tab(1)}/// <typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} value in the resulting tuple.</typeparam>"));
                    
                    builder.Append($"{Tab(1)}/// <summary>\n");
                    builder.Append($"{Tab(1)}/// Combines {i} Result values into a single Result containing a tuple, accumulating all errors if any fail.\n");
                    builder.Append($"{Tab(1)}/// </summary>\n");
                    builder.Append(typeParamDocs + "\n");
                    builder.Append(string.Join("\n", Enumerable.Range(1, i).Select(n => $"{Tab(1)}/// <param name=\"result{n}\">The {GetOrdinal(n)} Result value to combine.</param>")));
                    builder.Append($"\n{Tab(1)}/// <returns>A <see cref=\"Result{{T}}\"/> containing a tuple with all values if all Results succeed; otherwise, a failure with all accumulated errors.</returns>\n");
                    builder.Append($"{Tab(1)}/// <remarks>\n");
                    builder.Append($"{Tab(1)}/// The BindAll factory method combines multiple Result values with comprehensive error accumulation:\n");
                    builder.Append($"{Tab(1)}/// - All Result values are evaluated (no short-circuiting)\n");
                    builder.Append($"{Tab(1)}/// - If any Results fail, ALL errors are collected using ErrorBuilder\n");
                    builder.Append($"{Tab(1)}/// - Returns a composite error containing all failure information\n");
                    builder.Append($"{Tab(1)}/// - Only if ALL Results succeed are their values combined into a tuple\n");
                    builder.Append($"{Tab(1)}/// \n");
                    builder.Append($"{Tab(1)}/// This differs from Bind which uses short-circuit evaluation. Use BindAll when you need\n");
                    builder.Append($"{Tab(1)}/// comprehensive error reporting, such as in validation scenarios where users benefit from\n");
                    builder.Append($"{Tab(1)}/// seeing all errors at once rather than fixing them one at a time.\n");
                    builder.Append($"{Tab(1)}/// \n");
                    builder.Append($"{Tab(1)}/// The 'in' parameter modifier provides performance optimization for value types.\n");
                    builder.Append($"{Tab(1)}/// </remarks>\n");
                    builder.Append(Tab(1) + $"public static Result<({types})> BindAllAppend<{types}>(\n");
                    builder.Append(string.Join(",\n",
                        Enumerable.Range(1, i).Select(n => $"{Tab(2)}in Result<TValue{n}> result{n}")));
                    builder.Append($")\n{Tab(1)}{{\n");

                    builder.Append(Tab(2) + "return new ErrorBuilder()\n");
                    
                    for (int j = 1; j <= i; j++)
                    {
                        builder.Append($"{Tab(3)}.AppendOnFailure(result{j})\n");
                    }

                    builder.Append(Tab(3) + ".TryBuild(out Error error)\n");
                    builder.Append(Tab(4) + "? error\n");
                    builder.Append(Tab(4) + $": ({string.Join(", ", Enumerable.Range(1, i).Select(n => $"result{n}.Value"))});\n");

                    builder.Append(Tab(1) + "}\n\n");
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}