using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates asynchronous Map extension methods for Result types with tuple values using Task.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous transformations of successful Result values
    /// containing tuples. It generates overloads for tuples with 2 to MaxElements elements, supporting
    /// both synchronous and asynchronous mapper functions on Task-wrapped Results.
    /// 
    /// Generated method patterns include:
    /// 1. Async mapper on sync Result:
    /// <code>
    /// MapAsync&lt;T1, T2, TTarget&gt;(
    ///     this Result&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, Task&lt;TTarget&gt;&gt; mapper)
    /// </code>
    /// 
    /// 2. Sync mapper on async Result:
    /// <code>
    /// MapAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Result&lt;(T1, T2)&gt;&gt; source,
    ///     Func&lt;T1, T2, TTarget&gt; mapper)
    /// </code>
    /// 
    /// 3. Async mapper on async Result:
    /// <code>
    /// MapAsync&lt;T1, T2, TTarget&gt;(
    ///     this Task&lt;Result&lt;(T1, T2)&gt;&gt; source,
    ///     Func&lt;T1, T2, Task&lt;TTarget&gt;&gt; mapper)
    /// </code>
    /// 
    /// All operations:
    /// - Propagate failure Results without executing the mapper
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Return Task&lt;Result&lt;TTarget&gt;&gt; for composability
    /// 
    /// This enables functional composition in async workflows while maintaining error handling semantics.
    /// </remarks>
    [Generator]
    public class TaskMapExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.Task.MapExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         public static async Task<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this Result<({{types}})> source,
                                             Func<{{types}}, Task<TTarget>> mapper)
                                         {
                                             if (source.IsFailure) return source.Error;
                                             return await mapper({{items}}).ConfigureAwait(false);
                                         }
                                         
                                         public static async Task<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this Task<Result<({{types}})>> source,
                                             Func<{{types}}, TTarget> mapper)
                                             => (await source.ConfigureAwait(false)).Map(mapper);
                                              
                                         public static async Task<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this Task<Result<({{types}})>> source,
                                             Func<{{types}}, Task<TTarget>> mapper)
                                             => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
                                             
                                     
                                     """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}