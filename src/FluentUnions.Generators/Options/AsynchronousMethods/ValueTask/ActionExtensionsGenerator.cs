using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates asynchronous action-based extension methods for Option types with tuple values using ValueTask.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable asynchronous side effects on Option values
    /// containing tuples using ValueTask for improved performance. It generates overloads for tuples
    /// with 2 to MaxElements elements, supporting both synchronous and asynchronous actions on
    /// ValueTask-wrapped Options.
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
    ///     Func&lt;T1, T2, ValueTask&gt; action)
    /// </code>
    /// 
    /// All operations:
    /// - Return the original Option to enable fluent chaining
    /// - Use ConfigureAwait(false) for proper async context handling
    /// - Execute actions only when appropriate (Some/None cases)
    /// 
    /// ValueTask is particularly beneficial for side-effect operations that often complete
    /// synchronously, such as writing to an in-memory cache or performing validation.
    /// </remarks>
    [Generator]
    public class ValueTaskActionExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.ValueTask.ActionExtensions.g.cs";

                StringBuilder builder =
                    new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                         public static async ValueTask<Option<({{types}})>> OnSomeAsync<{{types}}>(this Option<({{types}})> option, Func<{{types}}, ValueTask> action)
                                         {
                                             if (option.IsSome) await action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}}).ConfigureAwait(false);
                                             return option;
                                         }
                                         
                                         public static async ValueTask<Option<({{types}})>> OnSomeAsync<{{types}}>(this ValueTask<Option<({{types}})>> option, Action<{{types}}> action)
                                         => (await option.ConfigureAwait(false)).OnSome(action);
                                         
                                         public static async ValueTask<Option<({{types}})>> OnSomeAsync<{{types}}>(this ValueTask<Option<({{types}})>> option, Func<{{types}}, ValueTask> action)
                                         => await (await option.ConfigureAwait(false)).OnSomeAsync(action).ConfigureAwait(false);
                                         
                                         public static async ValueTask<Option<({{types}})>> OnEitherAsync<{{types}}>(this Option<({{types}})> option, Func<{{types}}, ValueTask> some, Func<ValueTask> none)
                                         {
                                             if (option.IsSome) await some({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}}).ConfigureAwait(false);
                                             else await none().ConfigureAwait(false);
                                             return option;
                                         }
                                         
                                         public static async ValueTask<Option<({{types}})>> OnEitherAsync<{{types}}>(this ValueTask<Option<({{types}})>> option, Action<{{types}}> some, Action none)
                                         => (await option.ConfigureAwait(false)).OnEither(some, none);
                                         
                                         public static async ValueTask<Option<({{types}})>> OnEitherAsync<{{types}}>(this ValueTask<Option<({{types}})>> option, Func<{{types}}, ValueTask> some, Func<ValueTask> none)
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