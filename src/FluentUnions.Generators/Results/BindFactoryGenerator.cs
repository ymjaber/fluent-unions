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