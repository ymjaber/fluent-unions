using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class TaskMatchExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.Task.MatchExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TSource" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         in this Result<({{types}})> result,
                                         Func<{{types}}, Task<TTarget>> success,
                                         Func<Error, Task<TTarget>> failure)
                                     {
                                         return result.IsSuccess
                                             ? success({{items}})
                                             : failure(result.Error);
                                     }

                                     public static async Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this Task<Result<({{types}})>> result,
                                         Func<{{types}}, TTarget> success,
                                         Func<Error, TTarget> failure)
                                         => (await result.ConfigureAwait(false)).Match(success, failure);
                                          
                                     public static async Task<TTarget> MatchAsync<{{types}}, TTarget>(
                                         this Task<Result<({{types}})>> result,
                                         Func<{{types}}, Task<TTarget>> success,
                                         Func<Error, Task<TTarget>> failure)
                                         => await (await result.ConfigureAwait(false)).MatchAsync(success, failure).ConfigureAwait(false);
                                         
                                 
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}