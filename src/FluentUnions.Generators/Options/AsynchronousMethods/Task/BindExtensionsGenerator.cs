using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous Bind extension methods for Option types with tuple values using Task.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous monadic binding (flatMap) operations
    /// on Option values containing tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// supporting both synchronous and asynchronous binder functions on Task-wrapped Options.
    /// 
    /// Generated method patterns include:
    /// 1. Async binder on sync Option:
    /// <code>
    /// BindAsync&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, Task&lt;Option&lt;TTarget&gt;&gt;&gt; binder)
    /// </code>
    /// 
    /// 2. Sync binder on async Option:
    /// <code>
    /// BindAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; source,
    ///     Func&lt;T1, T2, Option&lt;TTarget&gt;&gt; binder)
    /// </code>
    /// 
    /// 3. Async binder on async Option:
    /// <code>
    /// BindAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; source,
    ///     Func&lt;T1, T2, Task&lt;Option&lt;TTarget&gt;&gt;&gt; binder)
    /// </code>
    /// 
    /// All operations:
    /// - Return Task.FromResult(None) immediately if the source is None
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Enable chaining of async operations that may fail or produce no value
    /// 
    /// This is the fundamental operation for async monadic composition with Option types.
    /// </remarks>
    [Generator]
    public class TaskBindExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.Task.BindExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                         public static Task<Option<TTarget>> BindAsync<{{types}}, TTarget>(
                                             in this Option<({{types}})> source,
                                             Func<{{types}}, Task<Option<TTarget>>> binder)
                                         {
                                             if (source.IsNone) return Task.FromResult(Option<TTarget>.None);
                                             return binder({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"))}});
                                         }
                                         
                                         public static async Task<Option<TTarget>> BindAsync<{{types}}, TTarget>(
                                             this Task<Option<({{types}})>> source,
                                             Func<{{types}}, Option<TTarget>> binder)
                                             => (await source.ConfigureAwait(false)).Bind(binder);
                                     
                                         public static async Task<Option<TTarget>> BindAsync<{{types}}, TTarget>(
                                             this Task<Option<({{types}})>> source,
                                             Func<{{types}}, Task<Option<TTarget>>> binder)
                                             => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
                                             
                                     
                                     """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}