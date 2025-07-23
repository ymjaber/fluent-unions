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
                    builder.Append($$"""
                                     public static Option<({{types}})> OnSome<{{types}}>(in this Option<({{types}})> option, Action<{{types}}> action)
                                     {
                                         if (option.IsSome) action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}});
                                         return option;
                                     }
                                     
                                     public static Option<({{types}})> OnEither<{{types}}>(in this Option<({{types}})> option, Action<{{types}}> some, Action none)
                                     {
                                         if (option.IsSome) some({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"))}});
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