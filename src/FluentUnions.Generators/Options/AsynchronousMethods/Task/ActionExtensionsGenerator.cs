using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous action-based extension methods for Option types with tuple values using Task.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous side effects on Option values
    /// containing tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// supporting both synchronous and asynchronous actions on Task-wrapped Options.
    /// 
    /// Generated method patterns include:
    /// 
    /// OnSomeAsync overloads:
    /// 1. Async action on sync Option
    /// 2. Sync action on async Option
    /// 3. Async action on async Option
    /// 
    /// OnEitherAsync overloads:
    /// 1. Async actions on sync Option
    /// 2. Sync actions on async Option
    /// 3. Async actions on async Option
    /// 
    /// Example:
    /// <code>
    /// OnSomeAsync&lt;T1, T2&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, Task&gt; action)
    /// </code>
    /// 
    /// All operations:
    /// - Return the original Option to enable fluent chaining
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Execute actions only when appropriate (Some/None cases)
    /// 
    /// These methods are useful for async logging, notifications, or other side effects
    /// while maintaining the functional pipeline.
    /// </remarks>
    [Generator]
    public class TaskActionExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.Task.ActionExtensions.g.cs";

                StringBuilder builder =
                    new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                         public static async Task<Option<({{types}})>> OnSomeAsync<{{types}}>(this Option<({{types}})> option, Func<{{types}}, Task> action)
                                         {
                                             if (option.IsSome) await action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}}).ConfigureAwait(false);
                                             return option;
                                         }
                                         
                                         public static async Task<Option<({{types}})>> OnSomeAsync<{{types}}>(this Task<Option<({{types}})>> option, Action<{{types}}> action)
                                         => (await option.ConfigureAwait(false)).OnSome(action);
                                         
                                         public static async Task<Option<({{types}})>> OnSomeAsync<{{types}}>(this Task<Option<({{types}})>> option, Func<{{types}}, Task> action)
                                         => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
                                         
                                         public static async Task<Option<({{types}})>> OnEitherAsync<{{types}}>(this Option<({{types}})> option, Func<{{types}}, Task> some, Func<Task> none)
                                         {
                                             if (option.IsSome) await some({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}}).ConfigureAwait(false);
                                             else await none().ConfigureAwait(false);
                                             return option;
                                         }
                                         
                                         public static async Task<Option<({{types}})>> OnEitherAsync<{{types}}>(this Task<Option<({{types}})>> option, Action<{{types}}> some, Action none)
                                         => (await option.ConfigureAwait(false)).OnEither(some, none);
                                         
                                         public static async Task<Option<({{types}})>> OnEitherAsync<{{types}}>(this Task<Option<({{types}})>> option, Func<{{types}}, Task> some, Func<Task> none)
                                         => await (await option.ConfigureAwait(false)).OnEitherAsync(some, none).ConfigureAwait(false);
                                     
                                     
                                     """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}