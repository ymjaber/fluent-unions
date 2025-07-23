using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous Match extension methods for Option types with tuple values using Task.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous pattern matching on Option types
    /// containing tuples. It generates overloads for tuples with 2 to MaxElements elements, supporting
    /// both synchronous and asynchronous match functions on Task-wrapped Options.
    /// 
    /// Generated method patterns include:
    /// 1. Async match on sync Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, Task&lt;TTarget&gt;&gt; some,
    ///     Func&lt;Task&lt;TTarget&gt;&gt; none)
    /// </code>
    /// 
    /// 2. Sync match on async Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, TTarget&gt; some,
    ///     Func&lt;TTarget&gt; none)
    /// </code>
    /// 
    /// 3. Async match on async Option:
    /// <code>
    /// MatchAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, Task&lt;TTarget&gt;&gt; some,
    ///     Func&lt;Task&lt;TTarget&gt;&gt; none)
    /// </code>
    /// 
    /// All operations:
    /// - Ensure exhaustive handling of both Some and None cases
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Return Task&lt;TTarget&gt; for composability in async workflows
    /// 
    /// This provides a functional, type-safe way to handle Option values in async contexts.
    /// </remarks>
    [Generator]
    public class TaskMatchExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.Task.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         in this Option<({{types}})> option,
                                         Func<{{types}}, Task<TTarget>> some,
                                         Func<Task<TTarget>> none)
                                     {
                                         return option.IsSome ? some({{items}}) : none();
                                     }

                                     public static async Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this Task<Option<({{types}})>> option,
                                         Func<{{types}}, TTarget> some,
                                         Func<TTarget> none)
                                         => (await option.ConfigureAwait(false)).Match(some, none);
                                 
                                     public static async Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this Task<Option<({{types}})>> option,
                                         Func<{{types}}, Task<TTarget>> some,
                                         Func<Task<TTarget>> none)
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