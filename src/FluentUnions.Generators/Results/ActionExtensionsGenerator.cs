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
                    builder.Append($$"""
                                 public static Result<({{types}})> OnSuccess<{{types}}>(in this Result<({{types}})> result, Action<{{types}}> action)
                                 {
                                     if (result.IsSuccess) action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"))}});
                                     return result;
                                 }
                                 
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