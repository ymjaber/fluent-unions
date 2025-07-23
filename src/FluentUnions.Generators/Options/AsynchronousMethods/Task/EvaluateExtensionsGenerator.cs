using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous Filter extension methods for Option types with tuple values using Task.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous filtering of Option values
    /// based on predicates. It generates overloads for tuples with 2 to MaxElements elements,
    /// supporting both synchronous and asynchronous predicates on Task-wrapped Options.
    /// 
    /// Generated method patterns include:
    /// 1. Async predicate on sync Option:
    /// <code>
    /// FilterAsync&lt;T1, T2&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, Task&lt;bool&gt;&gt; predicate)
    /// </code>
    /// 
    /// 2. Sync predicate on async Option:
    /// <code>
    /// FilterAsync&lt;T1, T2&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, bool&gt; predicate)
    /// </code>
    /// 
    /// 3. Async predicate on async Option:
    /// <code>
    /// FilterAsync&lt;T1, T2&gt;(
    ///     this Task&lt;Option&lt;(T1, T2)&gt;&gt; option,
    ///     Func&lt;T1, T2, Task&lt;bool&gt;&gt; predicate)
    /// </code>
    /// 
    /// All operations:
    /// - Return None immediately if the source is None
    /// - Apply the predicate only if the Option has a value
    /// - Return the original Option if predicate is true, None if false
    /// - Use ConfigureAwait(false) for proper async context handling
    /// 
    /// This enables conditional propagation in async workflows based on validation or business rules.
    /// </remarks>
    [Generator]
    public class TaskFilterExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.Task.FilterExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static async Task<Option<({{types}})>> FilterAsync<{{types}}>(
                                         this Option<({{types}})> option,
                                         Func<{{types}}, Task<bool>> predicate)
                                     {
                                         if (option.IsNone) return option;
                                     
                                         return await predicate({{items}}).ConfigureAwait(false) ? option : Option<({{types}})>.None;
                                     }

                                     public static async Task<Option<({{types}})>> FilterAsync<{{types}}>(
                                         this Task<Option<({{types}})>> option,
                                         Func<{{types}}, bool> predicate)
                                         => (await option.ConfigureAwait(false)).Filter(predicate);
                                 
                                     public static async Task<Option<({{types}})>> FilterAsync<{{types}}>(
                                         this Task<Option<({{types}})>> option,
                                         Func<{{types}}, Task<bool>> predicate)
                                         => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
                                         
                                 
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}