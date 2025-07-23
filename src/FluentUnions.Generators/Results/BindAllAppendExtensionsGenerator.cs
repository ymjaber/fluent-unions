using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates BindAllAppend extension methods for Result types with error accumulation.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that combine existing Result values with new ones
    /// by appending values to form larger tuples, while accumulating all errors if any Results fail.
    /// It generates overloads for all combinations where the total tuple size is between 2 and MaxElements.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// BindAllAppend&lt;TSource1, TSource2, TTarget&gt;(
    ///     this Result&lt;(TSource1, TSource2)&gt; result,
    ///     in Result&lt;TTarget&gt; binder)
    /// returns Result&lt;(TSource1, TSource2, TTarget)&gt;
    /// </code>
    /// 
    /// The BindAllAppend operation differs from BindAppend:
    /// - Takes a Result value directly instead of a function
    /// - Uses ErrorBuilder to accumulate ALL errors from both Results
    /// - If both Results fail, combines errors into a composite error
    /// - If any Result succeeds while others fail, returns the accumulated errors
    /// - If all Results succeed, combines all values into a larger tuple
    /// 
    /// This is useful for:
    /// - Validation scenarios where you want all errors reported
    /// - Combining pre-computed Results while preserving all error information
    /// - Building comprehensive error reports in data processing pipelines
    /// 
    /// The 'in' parameter modifier is used for performance optimization with value types.
    /// </remarks>
    [Generator]
    public class BindAllAppendExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.BindAllAppendExtensions.g.cs";

                StringBuilder builder =
                    new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    for (int j = 1; j < i; j++)
                    {
                        string sourceTypes = j == 1
                            ? "TSource"
                            : string.Join(", ", Enumerable.Range(1, j).Select(n => "TSource" + n));

                        string tupleSourceTypes = j == 1
                            ? sourceTypes
                            : $"({sourceTypes})";

                        int k = i - j;
                        string targetTypes = k == 1
                            ? "TTarget"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => "TTarget" + n));

                        string tupleTargetTypes = k == 1
                            ? targetTypes
                            : $"({targetTypes})";

                        string sourceItems = j == 1
                            ? "result.Value"
                            : string.Join(", ", Enumerable.Range(1, j).Select(n => $"result.Value.Item{n}"));
                        
                        string targetItems = k == 1
                            ? "binder.Value"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => $"binder.Value.Item{n}"));

                        builder.Append($$"""
                                             public static Result<({{sourceTypes}}, {{targetTypes}})> BindAllAppend<{{sourceTypes}}, {{targetTypes}}>(
                                                 in this Result<{{tupleSourceTypes}}> result,
                                                 in Result<{{tupleTargetTypes}}> binder)
                                             {
                                                 return new ErrorBuilder()
                                                     .AppendOnFailure(result)
                                                     .AppendOnFailure(binder)
                                                     .TryBuild(out Error error)
                                                         ? error
                                                         : ({{sourceItems}}, {{targetItems}});
                                             }


                                         """);
                    }
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}