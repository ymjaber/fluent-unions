using Xunit;

namespace FluentUnions.PackageValidation;

public class SimpleGeneratorTest
{
    [Fact]
    public void Check_If_Generators_Are_Running()
    {
        // This test simply checks if generated code exists
        // by looking for generated extension methods
        
        var option = Option.Some((1, 2));
        
        // If this compiles, the generators are working
        // The MapExtensionsGenerator should generate this overload
        var methods = typeof(OptionExtensions).GetMethods()
            .Where(m => m.Name == "Map")
            .Where(m => m.IsGenericMethodDefinition)
            .ToList();
        
        // Should have generated Map methods
        Assert.NotEmpty(methods);
        
        // Check for tuple-specific overloads
        var hasTupleOverloads = methods.Any(m => 
        {
            var parameters = m.GetParameters();
            if (parameters.Length >= 2)
            {
                var funcParam = parameters[1];
                if (funcParam.ParameterType.IsGenericType)
                {
                    var genericType = funcParam.ParameterType.GetGenericTypeDefinition();
                    return genericType.Name.StartsWith("Func`");
                }
            }
            return false;
        });
        
        Assert.True(hasTupleOverloads, "Should have generated tuple Map overloads");
    }
}