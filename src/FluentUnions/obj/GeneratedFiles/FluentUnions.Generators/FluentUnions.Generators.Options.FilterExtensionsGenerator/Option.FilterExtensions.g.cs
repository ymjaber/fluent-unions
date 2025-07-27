namespace FluentUnions;

public static partial class OptionExtensions
{
    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 2 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2)> Filter<TValue1, TValue2>(
        in this Option<(TValue1, TValue2)> option,
        Func<TValue1, TValue2, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2) ? option : Option<(TValue1, TValue2)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 3 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3)> Filter<TValue1, TValue2, TValue3>(
        in this Option<(TValue1, TValue2, TValue3)> option,
        Func<TValue1, TValue2, TValue3, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3) ? option : Option<(TValue1, TValue2, TValue3)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 4 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4)> Filter<TValue1, TValue2, TValue3, TValue4>(
        in this Option<(TValue1, TValue2, TValue3, TValue4)> option,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4) ? option : Option<(TValue1, TValue2, TValue3, TValue4)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 5 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 6 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 7 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>.None;
    }

    /// <summary>
    /// Filters an <see cref="Option{T}"/> containing a tuple with 8 elements based on a predicate.
    /// </summary>
    /// <typeparam name="TValue1">The type of the first tuple element.</typeparam>
    /// <typeparam name="TValue2">The type of the second tuple element.</typeparam>
    /// <typeparam name="TValue3">The type of the third tuple element.</typeparam>
    /// <typeparam name="TValue4">The type of the fourth tuple element.</typeparam>
    /// <typeparam name="TValue5">The type of the fifth tuple element.</typeparam>
    /// <typeparam name="TValue6">The type of the sixth tuple element.</typeparam>
    /// <typeparam name="TValue7">The type of the seventh tuple element.</typeparam>
    /// <typeparam name="TValue8">The type of the eighth tuple element.</typeparam>
    /// <param name="option">The source <see cref="Option{T}"/> containing a tuple.</param>
    /// <param name="predicate">A predicate function to test the tuple elements.</param>
    /// <returns>The original Option if the predicate returns true; otherwise, None.</returns>
    /// <remarks>
    /// Filter allows conditional propagation of Option values based on their content.
    /// If the Option is None, the predicate is not executed and None is returned.
    /// If the Option has a value and the predicate returns true, the original Option is returned.
    /// If the predicate returns false, None is returned. This is useful for validation scenarios
    /// where you want to convert invalid values to None while preserving valid ones.
    /// </remarks>
    public static Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> Filter<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        in this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate)
    {
        if (option.IsNone) return option;
    
        return predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>.None;
    }

}