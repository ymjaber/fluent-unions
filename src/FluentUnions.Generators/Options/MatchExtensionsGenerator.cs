using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates Match extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow pattern matching on Option types containing tuples.
    /// It generates overloads for tuples with 2 to MaxElements elements, enabling developers
    /// to destructure tuple values directly in the Match method's some callback.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Match&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, TTarget&gt; some,
    ///     Func&lt;TTarget&gt; none)
    /// </code>
    /// 
    /// The Match operation:
    /// - If the Option has a value (IsSome), applies the some function with destructured tuple elements
    /// - If the Option is empty (IsNone), executes the none function
    /// - Both branches must return the same type TTarget
    /// 
    /// This provides a functional, exhaustive way to handle Option values, ensuring both
    /// Some and None cases are explicitly handled while automatically destructuring tuples.
    /// </remarks>
    [Generator]
    public class MatchExtensionsGenerator : IIncrementalGenerator
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
                var fileName = "Option.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TSource{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    
                    builder.Append($$"""
                                     /// <summary>
                                     /// Matches on an <see cref="Option{T}"/> containing a tuple with {{i}} elements, executing one of two functions based on its state.
                                     /// </summary>
                                     /// {{typeParamDocs}}
                                     /// <typeparam name="TTarget">The type of the value returned by the match functions.</typeparam>
                                     /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
                                     /// <param name="some">A function to execute on the tuple elements if the Option has a value.</param>
                                     /// <param name="none">A function to execute if the Option is None.</param>
                                     /// <returns>The value returned by either the some or none function.</returns>
                                     /// <remarks>
                                     /// Match provides exhaustive pattern matching for Option types, ensuring both Some and None cases
                                     /// are handled. This is a fundamental functional programming pattern that guarantees a value
                                     /// is always produced, making it impossible to forget handling the None case.
                                     /// The tuple elements are automatically destructured when passed to the some function.
                                     /// </remarks>
                                     public static TTarget Match<{{types}}, TTarget>(
                                         in this Option<({{types}})> option,
                                         Func<{{types}}, TTarget> some,
                                         Func<TTarget> none)
                                     {
                                         return option.IsSome ? some({{items}}) : none();
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