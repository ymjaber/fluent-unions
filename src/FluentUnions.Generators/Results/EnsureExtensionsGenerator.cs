using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates Ensure extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that add validation to successful Result values.
    /// It generates overloads for tuples with 2 to MaxElements elements, allowing post-success validation.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Ensure&lt;T1, T2&gt;(
    ///     this Result&lt;(T1, T2)&gt; result,
    ///     Func&lt;T1, T2, bool&gt; predicate,
    ///     Error error)
    /// </code>
    /// 
    /// The Ensure operation:
    /// - If the Result is already a failure, propagates the error unchanged
    /// - If the Result is successful, applies the predicate to validate the values
    /// - If the predicate returns true, returns the original successful Result
    /// - If the predicate returns false, returns a failure Result with the provided error
    /// 
    /// This is useful for adding business rule validations after successful operations,
    /// maintaining the Result chain while ensuring values meet specific criteria.
    /// </remarks>
    [Generator]
    public class EnsureExtensionsGenerator : IIncrementalGenerator
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
                var fileName = "ValueResult.EnsureExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    
                    builder.Append($$"""
                                     /// <summary>
                                     /// Validates that a successful <see cref="Result{T}"/> containing a tuple with {{i}} elements satisfies a predicate, converting to failure if not.
                                     /// </summary>
                                     /// {{typeParamDocs}}
                                     /// <param name="result">The source <see cref="Result{T}"/> containing a tuple to validate.</param>
                                     /// <param name="predicate">A function that validates the tuple elements, returning true if valid.</param>
                                     /// <param name="error">The error to return if the predicate returns false.</param>
                                     /// <returns>The original Result if already failed or if the predicate returns true; otherwise, a failure Result with the specified error.</returns>
                                     /// <remarks>
                                     /// Ensure is a validation operation in the railway-oriented programming pattern. It adds a guard clause
                                     /// to the success track, allowing you to validate business rules and constraints on successful values.
                                     /// If the Result is already a failure, the validation is skipped and the existing error is propagated.
                                     /// This differs from Option's Filter operation as it explicitly provides error information when validation fails,
                                     /// maintaining the rich error context that Result types provide.
                                     /// </remarks>
                                     public static Result<({{types}})> Ensure<{{types}}>(
                                         in this Result<({{types}})> result,
                                         Func<{{types}}, bool> predicate,
                                         Error error)
                                     {
                                         if (result.IsFailure) return result;
                                     
                                         return predicate({{items}}) ? result : error;
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