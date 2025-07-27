using Microsoft.CodeAnalysis;

namespace FluentUnions.Analyzers.Common;

/// <summary>
/// Provides helper methods for identifying FluentUnions types in the Roslyn semantic model.
/// </summary>
/// <remarks>
/// These utilities are used by analyzers to determine if a given type symbol represents
/// one of the FluentUnions types (Option or Result). This is essential for ensuring
/// analyzers only trigger on relevant code.
/// </remarks>
public static class SymbolHelpers
{
    /// <summary>
    /// Determines whether the specified type symbol represents an Option&lt;T&gt; type.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>true if the type symbol represents Option&lt;T&gt;; otherwise, false.</returns>
    /// <remarks>
    /// This method checks that:
    /// - The type name is "Option"
    /// - It belongs to the "FluentUnions" namespace
    /// - It is a generic type with exactly one type argument
    /// </remarks>
    public static bool IsOptionType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;
        
        return typeSymbol.Name == "Option" &&
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 };
    }
    
    /// <summary>
    /// Determines whether the specified type symbol represents any Result type (generic or non-generic).
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>true if the type symbol represents any Result type; otherwise, false.</returns>
    /// <remarks>
    /// This method checks if the type is named "Result" and belongs to the "FluentUnions" namespace,
    /// regardless of whether it's the generic Result&lt;T&gt; or non-generic Result type.
    /// </remarks>
    public static bool IsResultType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;
        
        return typeSymbol.Name == "Result" &&
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions";
    }
    
    /// <summary>
    /// Determines whether the specified type symbol represents a generic Result&lt;T&gt; type with a value.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>true if the type symbol represents Result&lt;T&gt;; otherwise, false.</returns>
    /// <remarks>
    /// This method specifically checks for the generic Result&lt;T&gt; type that can carry a value.
    /// Use this when you need to distinguish between Result&lt;T&gt; and the non-generic Result.
    /// </remarks>
    public static bool IsValueResultType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;
        
        return typeSymbol.Name == "Result" &&
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 };
    }
    
    /// <summary>
    /// Determines whether the specified type symbol represents a non-generic Result type (unit result).
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>true if the type symbol represents non-generic Result; otherwise, false.</returns>
    /// <remarks>
    /// This method checks for the non-generic Result type that represents operations
    /// that can succeed or fail but don't return a value on success.
    /// </remarks>
    public static bool IsUnitResultType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;
        
        return typeSymbol.Name == "Result" &&
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: false };
    }

    /// <summary>
    /// Determines whether a type symbol represents an EnsureBuilder type.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>True if the type is EnsureBuilder&lt;T&gt;; otherwise, false.</returns>
    public static bool IsEnsureBuilderType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;

        return typeSymbol.Name == "EnsureBuilder" && 
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 };
    }

    /// <summary>
    /// Determines whether a type symbol represents a FilterBuilder type.
    /// </summary>
    /// <param name="typeSymbol">The type symbol to check.</param>
    /// <returns>True if the type is FilterBuilder&lt;T&gt;; otherwise, false.</returns>
    public static bool IsFilterBuilderType(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;

        return typeSymbol.Name == "FilterBuilder" && 
               typeSymbol.ContainingNamespace?.ToString() == "FluentUnions" &&
               typeSymbol is INamedTypeSymbol { IsGenericType: true, TypeArguments.Length: 1 };
    }
}