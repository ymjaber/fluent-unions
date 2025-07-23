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