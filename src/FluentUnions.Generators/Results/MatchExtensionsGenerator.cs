using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Match extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow pattern matching on Result types containing tuples.
    /// It generates overloads for tuples with 2 to MaxElements (typically 8) elements, enabling developers
    /// to destructure tuple values directly in the Match method's success callback.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Match&lt;T1, T2, TTarget&gt;(
    ///     this Result&lt;(T1, T2)&gt; result,
    ///     Func&lt;T1, T2, TTarget&gt; success,
    ///     Func&lt;Error, TTarget&gt; failure)
    /// </code>
    /// 
    /// This allows for cleaner code when working with Result types that contain multiple values as tuples,
    /// automatically destructuring the tuple in the success case.
    /// </remarks>
    [Generator]
    public class MatchExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static TTarget Match<{{types}}, TTarget>(
                                         in this Result<({{types}})> result,
                                         Func<{{types}}, TTarget> success,
                                         Func<Error, TTarget> failure)
                                     {
                                         return result.IsSuccess ? success({{items}}) : failure(result.Error);
                                     }

                                 
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}