using FluentUnions;

#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FluentUnions.Tests.AnalyzerDemo;

/// <summary>
/// Demonstrates the OptionPreferNone analyzer (FU0004) and its code fix.
/// This file shows examples that trigger the analyzer warning.
/// </summary>
public class OptionPreferNoneDemo
{
    // The following lines would trigger FU0004: Prefer Option.None over default(Option<T>)
    // In an IDE with the analyzer enabled, you would see a warning and a code fix suggestion.
    
    public void ExplicitDefaultExpressions()
    {
        // ⚠️ FU0004: Use Option.None instead of default(Option<int>)
        Option<int> option1 = default(Option<int>);
        
        // ⚠️ FU0004: Use Option.None instead of default(Option<string>)
        var option2 = default(Option<string>);
        
        // After applying the code fix:
        // Option<int> option1 = Option.None;
        // var option2 = Option.None;
    }
    
    public void DefaultLiteralExpressions()
    {
        // ⚠️ FU0004: Use Option.None instead of default(Option<bool>)
        Option<bool> option = default;
        
        // After applying the code fix:
        // Option<bool> option = Option.None;
    }
    
    public Option<T> GetDefaultOption<T>()
    {
        // ⚠️ FU0004: Use Option.None instead of default(Option<T>)
        return default(Option<T>);
        
        // After applying the code fix:
        // return Option.None;
    }
    
    public Option<int> ConditionalDefault(bool condition)
    {
        // ⚠️ FU0004: Use Option.None instead of default(Option<int>)
        return condition ? Option.Some(42) : default(Option<int>);
        
        // After applying the code fix:
        // return condition ? Option.Some(42) : Option.None;
    }
    
    // Examples that do NOT trigger the analyzer (correct usage):
    
    public void CorrectUsage()
    {
        // ✅ Good: Using Option.None explicitly
        Option<int> option1 = Option.None;
        
        // ✅ Good: Using Option.Some for values
        Option<string> option2 = Option.Some("value");
        
        // ✅ Good: Using implicit conversion
        Option<int> option3 = 42;
    }
}