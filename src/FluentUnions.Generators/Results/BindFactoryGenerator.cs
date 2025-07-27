using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Bind factory methods for combining multiple Result-returning operations.
    /// </summary>
    /// <remarks>
    /// This generator produces static factory methods on the Result struct that combine multiple
    /// Result-returning functions into a single Result containing a tuple of all values.
    /// It generates overloads for combining 2 to MaxElements operations.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Result.Bind&lt;T1, T2&gt;(
    ///     Func&lt;Result&lt;T1&gt;&gt; result1,
    ///     Func&lt;Result&lt;T2&gt;&gt; result2)
    /// returns Result&lt;(T1, T2)&gt;
    /// </code>
    /// 
    /// The Bind factory method:
    /// - Executes each function in sequence
    /// - If any function returns a failure, short-circuits and returns that failure
    /// - If all functions succeed, combines their values into a tuple Result
    /// 
    /// This is useful for:
    /// - Combining multiple independent operations that might fail
    /// - Short-circuit evaluation when any operation fails
    /// - Collecting results from multiple sources into a single Result
    /// 
    /// Note: Operations are executed sequentially, not in parallel.
    /// </remarks>
    [Generator]
    public class BindFactoryGenerator : IIncrementalGenerator
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
                var className = "ValueResult.BindFactory";
                var fileName = $"{className}.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic readonly partial struct Result\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n", Enumerable.Range(1, i).Select(n => $"{Tab(1)}/// <typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} value in the resulting tuple.</typeparam>"));
                    
                    builder.Append($"{Tab(1)}/// <summary>\n");
                    builder.Append($"{Tab(1)}/// Combines {i} Result-returning operations into a single Result containing a tuple, using short-circuit evaluation.\n");
                    builder.Append($"{Tab(1)}/// </summary>\n");
                    builder.Append(typeParamDocs + "\n");
                    builder.Append(string.Join("\n", Enumerable.Range(1, i).Select(n => $"{Tab(1)}/// <param name=\"result{n}\">A function that returns the {GetOrdinal(n)} Result value.</param>")));
                    builder.Append($"\n{Tab(1)}/// <returns>A <see cref=\"Result{{T}}\"/> containing a tuple with all values if all operations succeed; otherwise, the first error encountered.</returns>\n");
                    builder.Append($"{Tab(1)}/// <remarks>\n");
                    builder.Append($"{Tab(1)}/// The Bind factory method executes Result-returning operations sequentially with short-circuit evaluation:\n");
                    builder.Append($"{Tab(1)}/// - Each function is executed in order\n");
                    builder.Append($"{Tab(1)}/// - If any function returns a failure, execution stops and that error is returned immediately\n");
                    builder.Append($"{Tab(1)}/// - Only if all functions succeed are their values combined into a tuple Result\n");
                    builder.Append($"{Tab(1)}/// \n");
                    builder.Append($"{Tab(1)}/// This differs from BindAll which accumulates all errors. Use Bind when you want fail-fast behavior\n");
                    builder.Append($"{Tab(1)}/// and don't need to collect all possible errors. The sequential execution means later operations\n");
                    builder.Append($"{Tab(1)}/// won't run if earlier ones fail, which can be more efficient for expensive operations.\n");
                    builder.Append($"{Tab(1)}/// </remarks>\n");
                    builder.Append(Tab(1) + $"public static Result<({types})> BindAppend<{types}>(\n");
                    builder.Append(string.Join(",\n",
                        Enumerable.Range(1, i).Select(n => $"{Tab(2)}Func<Result<TValue{n}>> result{n}")));
                    builder.Append($")\n{Tab(1)}{{\n");

                    for (int j = 1; j <= i; j++)
                    {
                        builder.Append($"{Tab(2)}Result<TValue{j}> r{j} = result{j}();\n");
                        builder.Append($"{Tab(2)}if(r{j}.IsFailure) return r{j}.Error;\n\n");
                    }

                    builder.Append($"{Tab(2)}return ({string.Join(", ", Enumerable.Range(1, i).Select(n => $"r{n}.Value"))});\n");

                    builder.Append(Tab(1) + "}\n\n");
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}