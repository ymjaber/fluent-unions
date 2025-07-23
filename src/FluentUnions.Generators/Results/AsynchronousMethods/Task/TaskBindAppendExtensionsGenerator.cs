using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class TaskBindAppendExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.Task.BindAppendExtensions.g.cs";

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
                            ? "target"
                            : string.Join(", ", Enumerable.Range(1, k).Select(n => $"target.Item{n}"));

                        builder.Append($$"""
                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Result<{{tupleSourceTypes}}> result,
                                                 Func<{{sourceTypes}}, Task<Result<{{tupleTargetTypes}}>>> binder)
                                             {
                                                 if (result.IsFailure) return result.Error;
                                             
                                                 var targetResult = await binder({{sourceItems}}).ConfigureAwait(false);
                                                 if (targetResult.IsFailure) return targetResult.Error;
                                                 
                                                 var target = targetResult.Value;
                                                 return ({{sourceItems}}, {{targetItems}});
                                             }

                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Task<Result<{{tupleSourceTypes}}>> result,
                                                 Func<{{sourceTypes}}, Result<{{tupleTargetTypes}}>> binder)
                                                 => (await result.ConfigureAwait(false)).BindAppend(binder);
                                                 

                                             public static async Task<Result<({{sourceTypes}}, {{targetTypes}})>> BindAppendAsync<{{sourceTypes}}, {{targetTypes}}>(
                                                 this Task<Result<{{tupleSourceTypes}}>> result,
                                                 Func<{{sourceTypes}}, Task<Result<{{tupleTargetTypes}}>>> binder)
                                                 => await (await result.ConfigureAwait(false)).BindAppendAsync(binder).ConfigureAwait(false);
                                             
                                         
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