using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates Bind extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable monadic binding (flatMap) operations
    /// on Option values containing tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// allowing developers to chain operations that may fail or produce no value.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Bind&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, Option&lt;TTarget&gt;&gt; binder)
    /// </code>
    /// 
    /// The Bind operation:
    /// - If the Option has a value (IsSome), applies the binder function to the tuple elements
    /// - If the Option is None, returns Option&lt;TTarget&gt;.None without executing the binder
    /// - The binder function itself returns an Option, enabling composition of operations that may fail
    /// 
    /// This is the fundamental operation for monadic composition with Option types.
    /// </remarks>
    [Generator]
    public class BindExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.BindExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                         public static Option<TTarget> Bind<{{types}}, TTarget>(
                                             in this Option<({{types}})> source,
                                             Func<{{types}}, Option<TTarget>> binder)
                                         {
                                             if (source.IsNone) return Option<TTarget>.None;
                                             return binder({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"))}});
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