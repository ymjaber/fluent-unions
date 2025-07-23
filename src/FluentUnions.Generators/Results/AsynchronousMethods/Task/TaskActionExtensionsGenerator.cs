using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class TaskActionExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.Task.ActionExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                 public static async Task<Result<({{types}})>> OnSuccessAsync<{{types}}>(this Result<({{types}})> result, Func<{{types}}, Task> action)
                                 {
                                     if (result.IsSuccess) await action({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"))}}).ConfigureAwait(false);
                                     return result;
                                 }
                                 
                                 public static async Task<Result<({{types}})>> OnSuccessAsync<{{types}}>(this Task<Result<({{types}})>> result, Action<{{types}}> action)
                                     => (await result.ConfigureAwait(false)).OnSuccess(action);
                                     
                                 public static async Task<Result<({{types}})>> OnSuccessAsync<{{types}}>(this Task<Result<({{types}})>> result, Func<{{types}}, Task> action)
                                     => await (await result.ConfigureAwait(false)).OnSuccessAsync(action).ConfigureAwait(false);
                                 
                                 public static async Task<Result<({{types}})>> OnEitherAsync<{{types}}>(this Result<({{types}})> result, Func<{{types}}, Task> success, Func<Error, Task> failure)
                                 {
                                     if (result.IsSuccess) await success({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"))}}).ConfigureAwait(false);
                                     else await failure(result.Error).ConfigureAwait(false);
                                     return result;
                                 }
                                 
                                 public static async Task<Result<({{types}})>> OnEitherAsync<{{types}}>(this Task<Result<({{types}})>> result, Action<{{types}}> success, Action<Error> failure)
                                     => (await result.ConfigureAwait(false)).OnEither(success, failure);
                                     
                                 public static async Task<Result<({{types}})>> OnEitherAsync<{{types}}>(this Task<Result<({{types}})>> result, Func<{{types}}, Task> success, Func<Error, Task> failure)
                                     => await (await result.ConfigureAwait(false)).OnEitherAsync(success, failure).ConfigureAwait(false);
                                     
                                     
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}