using Xunit;

namespace FluentUnions.PackageValidation;

public class SimpleMapTest
{
    [Fact]
    public void Can_Use_Generated_Map_Extension()
    {
        // Create a tuple option
        var option = Option.Some((1, "test"));
        
        // Try to use the generated Map extension directly
        // This should compile if the generator worked
        Option<int> mapped = option.Map(t => t.Item1 + t.Item2.Length);
        
        Assert.True(mapped.IsSome);
        Assert.Equal(5, mapped.Value);
    }
}