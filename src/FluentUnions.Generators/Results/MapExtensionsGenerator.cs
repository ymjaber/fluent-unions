using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Map extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow transformation of successful Result values
    /// when those values are tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// enabling developers to map over tuple values with automatic destructuring.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Map&lt;T1, T2, TTarget&gt;(
    ///     this Result&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, TTarget&gt; mapper)
    /// </code>
    /// 
    /// The Map operation:
    /// - If the Result is successful, applies the mapper function to the tuple elements
    /// - If the Result is a failure, propagates the error without executing the mapper
    /// 
    /// This enables functional composition while maintaining error handling semantics.
    /// </remarks>
    [Generator]
    public class MapExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.MapExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         public static Result<TTarget> Map<{{types}}, TTarget>(
                                             in this Result<({{types}})> source,
                                             Func<{{types}}, TTarget> mapper)
                                         {
                                             if (source.IsFailure) return source.Error;
                                             return mapper({{items}});
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