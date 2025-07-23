namespace FluentUnions;

public static partial class OptionExtensions
{
    public static async Task<Option<(TValue1, TValue2)>> FilterAsync<TValue1, TValue2>(
        this Option<(TValue1, TValue2)> option,
        Func<TValue1, TValue2, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2).ConfigureAwait(false) ? option : Option<(TValue1, TValue2)>.None;
    }

    public static async Task<Option<(TValue1, TValue2)>> FilterAsync<TValue1, TValue2>(
        this Task<Option<(TValue1, TValue2)>> option,
        Func<TValue1, TValue2, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2)>> FilterAsync<TValue1, TValue2>(
        this Task<Option<(TValue1, TValue2)>> option,
        Func<TValue1, TValue2, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3)>> FilterAsync<TValue1, TValue2, TValue3>(
        this Option<(TValue1, TValue2, TValue3)> option,
        Func<TValue1, TValue2, TValue3, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3)>> FilterAsync<TValue1, TValue2, TValue3>(
        this Task<Option<(TValue1, TValue2, TValue3)>> option,
        Func<TValue1, TValue2, TValue3, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3)>> FilterAsync<TValue1, TValue2, TValue3>(
        this Task<Option<(TValue1, TValue2, TValue3)>> option,
        Func<TValue1, TValue2, TValue3, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> FilterAsync<TValue1, TValue2, TValue3, TValue4>(
        this Option<(TValue1, TValue2, TValue3, TValue4)> option,
        Func<TValue1, TValue2, TValue3, TValue4, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3, TValue4)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> FilterAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4)>> FilterAsync<TValue1, TValue2, TValue3, TValue4>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Option<(TValue1, TValue2, TValue3, TValue4, TValue5)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>> predicate)
    {
        if (option.IsNone) return option;
    
        return await predicate(option.Value.Item1, option.Value.Item2, option.Value.Item3, option.Value.Item4, option.Value.Item5, option.Value.Item6, option.Value.Item7, option.Value.Item8).ConfigureAwait(false) ? option : Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>.None;
    }

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, bool> predicate)
        => (await option.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> FilterAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Task<Option<(TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8)>> option,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<bool>> predicate)
        => await (await option.ConfigureAwait(false)).FilterAsync(predicate).ConfigureAwait(false);
        
}