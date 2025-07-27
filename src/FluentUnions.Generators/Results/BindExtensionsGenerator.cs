using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Bind extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that enable monadic composition of Result operations
    /// when working with tuple values. It generates overloads for tuples with 2 to MaxElements elements.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Bind&lt;T1, T2, TTarget&gt;(
    ///     this Result&lt;(T1, T2)&gt; source,
    ///     Func&lt;T1, T2, Result&lt;TTarget&gt;&gt; binder)
    /// </code>
    /// 
    /// The Bind operation (also known as FlatMap or SelectMany):
    /// - If the Result is successful, applies the binder function to produce a new Result
    /// - If the Result is a failure, propagates the error without executing the binder
    /// - Flattens the nested Result structure, avoiding Result&lt;Result&lt;T&gt;&gt;
    /// 
    /// This is essential for chaining operations that might fail, maintaining clean error propagation
    /// through a sequence of potentially failing operations.
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
                var fileName = "ValueResult.BindExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         /// <summary>
                                         /// Applies a binder function to the value inside a successful <see cref="Result{T}"/> containing a tuple with {{i}} elements.
                                         /// </summary>
                                         /// {{typeParamDocs}}
                                         /// <typeparam name="TTarget">The type of the value in the resulting Result.</typeparam>
                                         /// <param name="source">The source <see cref="Result{T}"/> containing a tuple.</param>
                                         /// <param name="binder">A function that takes the tuple elements and returns a new Result that may succeed or fail.</param>
                                         /// <returns>The Result returned by the binder if the source was successful; otherwise, the original error.</returns>
                                         /// <remarks>
                                         /// Bind (also known as flatMap or >>=) is the fundamental operation for railway-oriented programming.
                                         /// It allows chaining operations that may fail, automatically short-circuiting on the first error.
                                         /// If the source Result is a failure, the binder is not executed and the error is propagated.
                                         /// This enables clean error handling without nested conditionals.
                                         /// </remarks>
                                         public static Result<TTarget> Bind<{{types}}, TTarget>(
                                             in this Result<({{types}})> source,
                                             Func<{{types}}, Result<TTarget>> binder)
                                         {
                                             if (source.IsFailure) return source.Error;
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