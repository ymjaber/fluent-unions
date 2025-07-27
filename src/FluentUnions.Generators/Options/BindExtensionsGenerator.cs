using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates Bind extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable monadic binding (flatMap) operations
    /// on Option values containing tuples. It generates overloads for tuples with 2 to MaxElements elements,
    /// allowing developers to chain operations that may fail or produce no value.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Bind&lt;T1, T2, TTarget&gt;(
    ///     this Option&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, Option&lt;TTarget&gt;&gt; binder)
    /// </code>
    /// 
    /// The Bind operation:
    /// - If the Option has a value (IsSome), applies the binder function to the tuple elements
    /// - If the Option is None, returns Option&lt;TTarget&gt;.None without executing the binder
    /// - The binder function itself returns an Option, enabling composition of operations that may fail
    /// 
    /// This is the fundamental operation for monadic composition with Option types.
    /// </remarks>
    [Generator]
    public class BindExtensionsGenerator : IIncrementalGenerator
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
                var fileName = "Option.BindExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         /// <summary>
                                         /// Applies a binder function to the value inside an <see cref="Option{T}"/> containing a tuple with {{i}} elements.
                                         /// </summary>
                                         /// {{typeParamDocs}}
                                         /// <typeparam name="TTarget">The type of the value in the resulting Option.</typeparam>
                                         /// <param name="source">The source <see cref="Option{T}"/> containing a tuple.</param>
                                         /// <param name="binder">A function that takes the tuple elements and returns a new Option.</param>
                                         /// <returns>The Option returned by the binder if the source was Some; otherwise, None.</returns>
                                         /// <remarks>
                                         /// Bind (also known as flatMap or >>=) is the fundamental monadic composition operation.
                                         /// It allows chaining operations that may fail or produce no value. If the source Option is None,
                                         /// the binder function is not executed and None is returned. This enables safe composition
                                         /// of operations without explicit null checking.
                                         /// </remarks>
                                         public static Option<TTarget> Bind<{{types}}, TTarget>(
                                             in this Option<({{types}})> source,
                                             Func<{{types}}, Option<TTarget>> binder)
                                         {
                                             if (source.IsNone) return Option<TTarget>.None;
                                             return binder({{items}});
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