using System.Text.RegularExpressions;

namespace FluentUnions;

/// <summary>
/// Provides cached compiled regex patterns for common validation scenarios.
/// </summary>
internal static partial class RegexPatterns
{
    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled)]
    internal static partial Regex EmailRegex();
    
    [GeneratedRegex(@"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.Compiled)]
    internal static partial Regex UrlRegex();
    
    [GeneratedRegex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", RegexOptions.Compiled)]
    internal static partial Regex PhoneNumberRegex();
    
    [GeneratedRegex(@"^(\d{1,3}\.){3}\d{1,3}$", RegexOptions.Compiled)]
    internal static partial Regex IpAddressRegex();
    
    [GeneratedRegex(@"^(\{){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled)]
    internal static partial Regex GuidRegex();
}