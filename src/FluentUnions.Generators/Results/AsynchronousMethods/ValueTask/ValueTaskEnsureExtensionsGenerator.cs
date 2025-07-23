using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class ValueTaskEnsureExtensionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "ValueResult.ValueTask.EnsureExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class ValueResultExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"result.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static async ValueTask<Result<({{types}})>> EnsureAsync<{{types}}>(
                                         this Result<({{types}})> result,
                                         Func<{{types}}, ValueTask<bool>> predicate,
                                         Error error)
                                     {
                                         if (result.IsFailure) return result;
                                     
                                         return await predicate({{items}}).ConfigureAwait(false) ? result : error;
                                     }

                                     public static async ValueTask<Result<({{types}})>> EnsureAsync<{{types}}>(
                                         this ValueTask<Result<({{types}})>> result,
                                         Func<{{types}}, bool> predicate,
                                         Error error)
                                         => (await result.ConfigureAwait(false)).Ensure(predicate, error);
                                 
                                     public static async ValueTask<Result<({{types}})>> EnsureAsync<{{types}}>(
                                         this ValueTask<Result<({{types}})>> result,
                                         Func<{{types}}, ValueTask<bool>> predicate,
                                         Error error)
                                         => await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);
                                         
                                 
                                 """);
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}