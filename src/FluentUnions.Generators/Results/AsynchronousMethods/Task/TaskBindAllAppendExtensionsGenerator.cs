using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class TaskBindAllAppendExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.Task.BindAllAppendExtensions.g.cs";

                StringBuilder builder =
                    new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    for (int j = 1; j < i; j++)
                    {
                        string sourceTypes = j == 1
                            ? "TSource"
                            : string.Join(", ", Enumerable.Range(1, j).Select(n => "TSource" + n));

                        string tupleSourceTypes = j == 1
                            ? sourceTypes
                            : $"({sourceTypes})";

                        int k = i - j;
                        string targetTypes = k == 1
                            ? "TTarget"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => "TTarget" + n));

                        string tupleTargetTypes = k == 1
                            ? targetTypes
                            : $"({targetTypes})";

                        string sourceItems = j == 1
                            ? "result.Value"
                            : string.Join(", ", Enumerable.Range(1, j).Select(n => $"result.Value.Item{n}"));
                        
                        string targetItems = k == 1
                            ? "binderResult.Value"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => $"binderResult.Value.Item{n}"));

                        builder.Append($$"""
                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAllAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Result<{{tupleSourceTypes}}> result,
                                                 Task<Result<{{tupleTargetTypes}}>> binder)
                                             {
                                                 var binderResult = await binder.ConfigureAwait(false);
                                                 return new ErrorBuilder()
                                                     .AppendOnFailure(result)
                                                     .AppendOnFailure(binderResult)
                                                     .TryBuild(out Error error)
                                                         ? error
                                                         : ({{sourceItems}}, {{targetItems}});
                                             }

                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAllAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Task<Result<{{tupleSourceTypes}}>> result,
                                                 Result<{{tupleTargetTypes}}> binder)
                                                 => (await result.ConfigureAwait(false)).BindAllAppend(binder);
                                         
                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAllAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Task<Result<{{tupleSourceTypes}}>> result,
                                                 Task<Result<{{tupleTargetTypes}}>> binder)
                                                 => await (await result.ConfigureAwait(false)).BindAllAppendAsync(binder).ConfigureAwait(false);
                                                 
                                         
                                         """);
                    }
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}