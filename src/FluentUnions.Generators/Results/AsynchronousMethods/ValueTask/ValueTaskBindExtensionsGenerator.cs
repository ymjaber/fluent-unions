using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class ValueTaskBindExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.ValueTask.BindExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append($$"""
                                         public static ValueTask<Result<TTarget>> BindAsync<{{types}}, TTarget>(
                                             in this Result<({{types}})> source,
                                             Func<{{types}}, ValueTask<Result<TTarget>>> binder)
                                         {
                                             if (source.IsFailure) return ValueTask.FromResult(Result.Failure<TTarget>(source.Error));
                                             return binder({{string.Join(", ", Enumerable.Range(1, i).Select(n => $"source.Value.Item{n}"))}});
                                         }
                                         
                                         public static async ValueTask<Result<TTarget>> BindAsync<{{types}}, TTarget>(
                                             this ValueTask<Result<({{types}})>> source,
                                             Func<{{types}}, Result<TTarget>> binder)
                                             => (await source.ConfigureAwait(false)).Bind(binder);
                                     
                                         public static async ValueTask<Result<TTarget>> BindAsync<{{types}}, TTarget>(
                                             this ValueTask<Result<({{types}})>> source,
                                             Func<{{types}}, ValueTask<Result<TTarget>>> binder)
                                             => await (await source.ConfigureAwait(false)).BindAsync(binder).ConfigureAwait(false);
                                             
                                     
                                     """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}