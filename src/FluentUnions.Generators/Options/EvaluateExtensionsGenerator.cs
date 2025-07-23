using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static FluentUnions.Generators.Invariants;

namespace FluentUnions.Generators.Options
{
    /// <summary>
    /// Source generator that creates Filter extension methods for Option types with tuple values.
    /// </summary>
    /// <remarks>
    /// This generator produces extension methods that allow filtering Option values based on predicates.
    /// It generates overloads for tuples with 2 to MaxElements elements, enabling developers
    /// to apply predicates to tuple values with automatic destructuring.
    /// 
    /// Generated methods follow the pattern:
    /// <code>
    /// Filter&lt;T1, T2&gt;(
    ///     this Option&lt;(T1, T2)&gt; option,
    ///     Func&lt;T1, T2, bool&gt; predicate)
    /// </code>
    /// 
    /// The Filter operation:
    /// - If the Option is None, returns None without executing the predicate
    /// - If the Option has a value, applies the predicate to the destructured tuple elements
    /// - If the predicate returns true, returns the original Option
    /// - If the predicate returns false, returns None
    /// 
    /// This allows conditional propagation of Option values based on their content,
    /// useful for validation and filtering scenarios while maintaining Option semantics.
    /// </remarks>
    [Generator]
    public class FilterExtensionsGenerator : IIncrementalGenerator
    {
        /// <summary>
        /// Initializes the generator and registers the source generation logic.
        /// </summary>
        /// <param name="context">The context for incremental generation.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                var fileName = "Option.FilterExtensions.g.cs";

                StringBuilder builder = new("namespace FluentUnions;\n\npublic static partial class OptionExtensions\n{\n");

                for (int i = 2; i <= MaxElements; i++)
                {
                    string types = string.Join(", ", Enumerable.Range(1, i).Select(n => "TValue" + n));
                    string items = string.Join(", ", Enumerable.Range(1, i).Select(n => $"option.Value.Item{n}"));
                    
                    builder.Append($$"""
                                     public static Option<({{types}})> Filter<{{types}}>(
                                         in this Option<({{types}})> option,
                                         Func<{{types}}, bool> predicate)
                                     {
                                         if (option.IsNone) return option;
                                     
                                         return predicate({{items}}) ? option : Option<({{types}})>.None;
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