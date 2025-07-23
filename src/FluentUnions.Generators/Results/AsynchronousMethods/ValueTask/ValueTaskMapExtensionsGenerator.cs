using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class ValueTaskMapExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.ValueTask.MapExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"));
                    
                    builder.Append($$"""
                                         public static async ValueTask<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this Result<({{types}})> source,
                                             Func<{{types}}, ValueTask<TTarget>> mapper)
                                         {
                                             if (source.IsFailure) return source.Error;
                                             return await mapper({{items}}).ConfigureAwait(false);
                                         }
                                         
                                         public static async ValueTask<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this ValueTask<Result<({{types}})>> source,
                                             Func<{{types}}, TTarget> mapper)
                                             => (await source.ConfigureAwait(false)).Map(mapper);
                                              
                                         public static async ValueTask<Result<TTarget>> MapAsync<{{types}}, TTarget>(
                                             this ValueTask<Result<({{types}})>> source,
                                             Func<{{types}}, ValueTask<TTarget>> mapper)
                                             => await (await source.ConfigureAwait(false)).MapAsync(mapper).ConfigureAwait(false);
                                             
                                     
                                     """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}