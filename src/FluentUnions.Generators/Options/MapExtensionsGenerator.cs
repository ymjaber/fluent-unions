using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates Map extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow transformation of Option values
    /// when those values are tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// enabling developers to map over tuple values with automatic destructuring.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Map&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, TTarget&gt; mapper)
    /// </code>
    /// 
    /// The Map operation:
    /// - If the Option has a value (IsSome), applies the mapper function to the tuple elements
    /// - If the Option is None, returns Option&lt;TTarget&gt;.None without executing the mapper
    /// 
    /// This enables functional composition while maintaining the Option's semantics.
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
                var fileName = "Option.MapExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         public static Option<TTarget> Map<{{types}}, TTarget>(
                                             in this Option<({{types}})> source,
                                             Func<{{types}}, TTarget> mapper)
                                         {
                                             if (source.IsNone) return Option<TTarget>.None;
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