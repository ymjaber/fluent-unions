using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Match extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow pattern matching on Result types containing tuples.
    /// It generates overloads for tuples with 2 to MaxElements (typically 8) elements, enabling developers
    /// to destructure tuple values directly in the Match method's success callback.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Match&lt;T1, T2, TTarget&gt;(
    ///     this Result&lt;(T1, T2)&gt; result,
    ///     Func&lt;T1, T2, TTarget&gt; success,
    ///     Func&lt;Error, TTarget&gt; failure)
    /// </code>
    /// 
    /// This allows for cleaner code when working with Result types that contain multiple values as tuples,
    /// automatically destructuring the tuple in the success case.
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
                var fileName = "ValueResult.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TSource{n}\">The type of the {GetOrdinal(n)} tuple element in the source Result.</typeparam>"));
                    
                    builder.Append($$"""
                                     /// <summary>
                                     /// Pattern matches on a <see cref="Result{T}"/> containing a tuple with {{i}} elements, executing different functions based on success or failure.
                                     /// </summary>
                                     /// {{typeParamDocs}}
                                     /// <typeparam name="TTarget">The type of the value returned by the match operation.</typeparam>
                                     /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to match on.</param>
                                     /// <param name="success">The function to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
                                     /// <param name="failure">The function to execute if the Result is a failure, receiving the error.</param>
                                     /// <returns>The value returned by either the success or failure function.</returns>
                                     /// <remarks>
                                     /// The Match operation provides exhaustive pattern matching for Result types, ensuring both success and failure
                                     /// cases are handled. This is a key pattern in railway-oriented programming, forcing explicit handling of both
                                     /// the happy path (success) and error cases. Unlike Option's Match which handles Some/None, Result's Match
                                     /// handles Success/Failure with explicit error information in the failure case.
                                     /// </remarks>
                                     public static TTarget Match<{{types}}, TTarget>(
                                         in this Result<({{types}})> result,
                                         Func<{{types}}, TTarget> success,
                                         Func<Error, TTarget> failure)
                                     {
                                         return result.IsSuccess ? success({{items}}) : failure(result.Error);
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