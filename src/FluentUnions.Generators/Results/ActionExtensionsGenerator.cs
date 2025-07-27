using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    /// <summary>
    /// Source generator that creates side-effect action extension methods for Result types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow executing side effects on Result values
    /// without transforming them. It generates overloads for tuples with 2 to MaxElements elements.
    /// 
    /// Generated methods include:
    /// 
    /// OnSuccess - Executes an action only if the Result is successful:
    /// <code>
    /// OnSuccess&lt;T1, T2&gt;(
    ///     this Result&lt;(T1, T2)&gt; result,
    ///     Action&lt;T1, T2&gt; action)
    /// </code>
    /// 
    /// OnEither - Executes different actions based on success or failure:
    /// <code>
    /// OnEither&lt;T1, T2&gt;(
    ///     this Result&lt;(T1, T2)&gt; result,
    ///     Action&lt;T1, T2&gt; success,
    ///     Action&lt;Error&gt; failure)
    /// </code>
    /// 
    /// Both methods return the original Result unchanged, making them useful for:
    /// - Logging
    /// - Debugging
    /// - Triggering side effects (like sending notifications)
    /// - Updating external state
    /// 
    /// These methods maintain the fluent chain while allowing observation of values.
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
                var fileName = "ValueResult.ActionExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string typeParamDocs = string.Join("\n    /// ", Enumerable.Range(1, i).Select(n => $"<typeparam name=\"TValue{n}\">The type of the {GetOrdinal(n)} tuple element.</typeparam>"));
                    
                    builder.Append($$"""
                                 /// <summary>
                                 /// Executes a side effect on the value of a successful <see cref="Result{T}"/> containing a tuple with {{i}} elements.
                                 /// </summary>
                                 /// {{typeParamDocs}}
                                 /// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
                                 /// <param name="action">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
                                 /// <returns>The original Result unchanged, allowing for method chaining.</returns>
                                 /// <remarks>
                                 /// OnSuccess is a side-effect operation that executes only when the Result is successful. Unlike Map, it doesn't
                                 /// transform the value but allows you to perform actions like logging, notifications, or state updates.
                                 /// If the Result is a failure, the action is not executed and the error is propagated unchanged.
                                 /// This maintains the railway-oriented programming flow while enabling observable side effects.
                                 /// </remarks>
                                 public static Result<({{types}})> OnSuccess<{{types}}>(in this Result<({{types}})> result, Action<{{types}}> action)
                                 {
                                     if (result.IsSuccess) action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"))}});
                                     return result;
                                 }
                                 
                                 /// <summary>
                                 /// Executes different side effects based on whether a <see cref="Result{T}"/> containing a tuple with {{i}} elements is successful or failed.
                                 /// </summary>
                                 /// {{typeParamDocs}}
                                 /// <param name="result">The source <see cref="Result{T}"/> containing a tuple.</param>
                                 /// <param name="success">The action to execute if the Result is successful, receiving the tuple elements as separate parameters.</param>
                                 /// <param name="failure">The action to execute if the Result is a failure, receiving the error.</param>
                                 /// <returns>The original Result unchanged, allowing for method chaining.</returns>
                                 /// <remarks>
                                 /// OnEither allows you to execute different side effects for both success and failure cases without
                                 /// transforming the Result. This is useful for comprehensive logging, debugging, or triggering
                                 /// different workflows based on the Result state. The original Result is returned unchanged,
                                 /// preserving the error handling chain while allowing observation of both paths.
                                 /// </remarks>
                                 public static Result<({{types}})> OnEither<{{types}}>(in this Result<({{types}})> result, Action<{{types}}> success, Action<Error> failure)
                                 {
                                     if (result.IsSuccess) success({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"))}});
                                     else failure(result.Error);
                                     return result;
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