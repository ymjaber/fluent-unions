using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Results
{
    [Generator]
    public class ValueTaskBindFactoryGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var className = "ValueResult.ValueTask.BindFactory";
                var fileName = $"{className}.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic readonly partial struct Result\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    builder.Append(Tab(1) + $"public static async ValueTask<Result<({types})>> BindAsync<{types}>(\n");
                    builder.Append(string.Join(",\n",
                        Enumerable.Range(1, i).Select(n => $"{Tab(2)}Func<ValueTask<Result<TValue{n}>>> result{n}")));
                    builder.Append($")\n{Tab(1)}{{\n");

                    for (int j = 1; j <= i; j++)
                    {
                        builder.Append($"{Tab(2)}Result<TValue{j}> r{j} = await result{j}().ConfigureAwait(false);\n");
                        builder.Append($"{Tab(2)}if(r{j}.IsFailure) return r{j}.Error;\n\n");
                    }

                    builder.Append($"{Tab(2)}return ({string.Join(", ", Enumerable.Range(1, i).Select(n => $"r{n}.Value"))});\n");

                    builder.Append(Tab(1) + "}\n\n");
                }

                builder.Append("}");

                string source = builder.ToString();
                ctx.AddSource(fileName, SourceText.From(source, Encoding.UTF8));
            });
        }
    }
}