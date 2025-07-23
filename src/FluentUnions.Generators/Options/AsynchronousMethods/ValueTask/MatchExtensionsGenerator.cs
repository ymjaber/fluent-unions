using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous Match extension methods for Option types with tuple values using ValueTask.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous pattern matching on Option types
    /// containing tuples using ValueTask for improved performance. It generates overloads for tuples
    /// with 2 to MaxElements elements, supporting both synchronous and asynchronous match functions
    /// on ValueTask-wrapped Options.
    /// 
    /// Generated method patterns include:
    /// 1. Async match on sync Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, ValueTask&lt;TTarget&gt;&gt; some,
    ///     Func&lt;ValueTask&lt;TTarget&gt;&gt; none)
    /// </code>
    /// 
    /// 2. Sync match on async Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this ValueTask&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, TTarget&gt; some,
    ///     Func&lt;TTarget&gt; none)
    /// </code>
    /// 
    /// 3. Async match on async Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this ValueTask&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, ValueTask&lt;TTarget&gt;&gt; some,
    ///     Func&lt;ValueTask&lt;TTarget&gt;&gt; none)
    /// </code>
    /// 
    /// All operations:
    /// - Ensure exhaustive handling of both Some and None cases
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Return ValueTask&lt;TTarget&gt; for optimal performance
    /// 
    /// ValueTask provides superior performance for pattern matching operations that frequently
    /// complete synchronously, reducing allocation pressure in hot paths.
    /// </remarks>
    [Generator]
    public class ValueTaskMatchExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.ValueTask.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static ValueTask<TTarget> MatchAsync<{{types}}, TTarget>(
                                         in this Option<({{types}})> option,
                                         Func<{{types}}, ValueTask<TTarget>> some,
                                         Func<ValueTask<TTarget>> none)
                                     {
                                         return option.IsSome ? some({{items}}) : none();
                                     }

                                     public static async ValueTask<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this ValueTask<Option<({{types}})>> option,
                                         Func<{{types}}, TTarget> some,
                                         Func<TTarget> none)
                                         => (await option.ConfigureAwait(false)).Match(some, none);
                                 
                                     public static async ValueTask<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this ValueTask<Option<({{types}})>> option,
                                         Func<{{types}}, ValueTask<TTarget>> some,
                                         Func<ValueTask<TTarget>> none)
                                         => await (await option.ConfigureAwait(false)).MatchAsync(some, none).ConfigureAwait(false);
                                         
                                 
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}