using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates action-based extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow executing side effects on Option values
    /// containing tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// providing OnSome and OnEither methods for handling both cases.
    /// 
    /// Generated methods include:
    /// - OnSome: Executes an action when the Option has a value, with tuple destructuring
    /// - OnEither: Executes one action when the Option has a value, another when it's None
    /// 
    /// Example patterns:
    /// <code>
    /// OnSome&lt;T1, T2&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Action&lt;T1, T2&gt; action)
    ///     
    /// OnEither&lt;T1, T2&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Action&lt;T1, T2&gt; some,
    ///     Action none)
    /// </code>
    /// 
    /// These methods are useful for:
    /// - Logging or debugging Option values
    /// - Performing side effects without transforming the value
    /// - Chaining operations while maintaining the original Option
    /// 
    /// All methods return the original Option to enable fluent chaining.
    /// </remarks>
    [Generator]
    public class ActionExtensionsGenerator : IIncrementalGenerator
    {
        private static string GetOrdinal(int number) => number switch
        {
            1 => "first",
            2 => "second",
            3 => "third",
            4 => "fourth",
            5 => "fifth",
            6 => "sixth",
            7 => "seventh",
            8 => "eighth",
            9 => "ninth",
            10 => "tenth",
            _ => $"{number}th"
        };

        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.ActionExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     /// <summary>
                                     /// Executes an action on the value inside an <see cref="Option{T}"/> containing a tuple with {{i}} elements if it has a value.
                                     /// </summary>
                                     /// {{typeParamDocs}}
                                     /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
                                     /// <param name="action">An action to execute on the tuple elements if the Option has a value.</param>
                                     /// <returns>The original Option to enable fluent chaining.</returns>
                                     /// <remarks>
                                     /// OnSome is useful for performing side effects (such as logging, debugging, or updating external state)
                                     /// when an Option has a value. The action is only executed if the Option is Some.
                                     /// The original Option is always returned unchanged, allowing for method chaining.
                                     /// </remarks>
                                     public static Option<({{types}})> OnSome<{{types}}>(in this Option<({{types}})> option, Action<{{types}}> action)
                                     {
                                         if (option.IsSome) action({{items}});
                                         return option;
                                     }
                                     
                                     /// <summary>
                                     /// Executes one of two actions based on whether an <see cref="Option{T}"/> containing a tuple with {{i}} elements has a value.
                                     /// </summary>
                                     /// {{typeParamDocs}}
                                     /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
                                     /// <param name="some">An action to execute on the tuple elements if the Option has a value.</param>
                                     /// <param name="none">An action to execute if the Option is None.</param>
                                     /// <returns>The original Option to enable fluent chaining.</returns>
                                     /// <remarks>
                                     /// OnEither ensures that exactly one action is executed based on the Option's state.
                                     /// This is useful for handling both Some and None cases with side effects, such as logging
                                     /// different messages or updating UI based on presence or absence of a value.
                                     /// The original Option is always returned unchanged.
                                     /// </remarks>
                                     public static Option<({{types}})> OnEither<{{types}}>(in this Option<({{types}})> option, Action<{{types}}> some, Action none)
                                     {
                                         if (option.IsSome) some({{items}});
                                         else none();
                                         return option;
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