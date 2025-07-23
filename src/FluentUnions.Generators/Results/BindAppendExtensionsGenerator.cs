using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates BindAppend extension methods for Result types to accumulate values into tuples.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that combine existing Result values with new ones
    /// by appending the new values to form larger tuples. It generates overloads for all combinations
    /// where the total tuple size is between 2 and MaxElements.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// BindAppend&lt;TSource1, TSource2, TTarget&gt;(
    ///     this Result&lt;(TSource1, TSource2)&gt; result,
    ///     Func&lt;TSource1, TSource2, Result&lt;TTarget&gt;&gt; binder)
    /// returns Result&lt;(TSource1, TSource2, TTarget)&gt;
    /// </code>
    /// 
    /// The BindAppend operation:
    /// - If the source Result is a failure, propagates the error
    /// - If successful, passes the values to the binder function
    /// - If the binder returns a failure, propagates that error
    /// - If the binder succeeds, combines all values into a larger tuple
    /// 
    /// This is useful for:
    /// - Building up a set of values through a series of operations
    /// - Accumulating results while maintaining error handling
    /// - Creating workflows that gather multiple pieces of data
    /// 
    /// Example usage:
    /// <code>
    /// Result&lt;User&gt; userResult = GetUser(id);
    /// Result&lt;(User, Profile)&gt; combined = userResult
    ///     .BindAppend(user => GetProfile(user.ProfileId));
    /// </code>
    /// </remarks>
    [Generator]
    public class BindAppendExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.BindAppendExtensions.g.cs";

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
                            ? "target"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => $"target.Item{n}"));

                        builder.Append($$"""
                                             public static Result<({{sourceTypes}}, {{targetTypes}})> BindAppend<{{sourceTypes}}, {{targetTypes}}>(
                                                 in this Result<{{tupleSourceTypes}}> result,
                                                 Func<{{sourceTypes}}, Result<{{tupleTargetTypes}}>> binder)
                                             {
                                                 if (result.IsFailure) return result.Error;
                                             
                                                 var targetResult = binder({{sourceItems}});
                                                 if (targetResult.IsFailure) return targetResult.Error;
                                                 
                                                 var target = targetResult.Value;
                                                 return ({{sourceItems}}, {{targetItems}});
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