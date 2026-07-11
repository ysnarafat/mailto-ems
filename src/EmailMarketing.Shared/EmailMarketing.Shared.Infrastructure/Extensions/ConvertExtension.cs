using System.Text.RegularExpressions;

namespace EmailMarketing.Shared.Infrastructure.Extensions;

public static class ConvertExtension
{
    public static string ParseObjectToString(this object? value, string defaultValue = "")
    {
        if (value == null) return defaultValue;
        return value.ToString()?.Trim() ?? defaultValue;
    }

    public static string ParseNullableDateTimeToString(this DateTime? value, string format)
    {
        if (value == null) return string.Empty;
        return value.Value.ToString(format);
    }

    public static int ParseObjectToInt(this object? value, int defaultValue = 0)
    {
        if (value == null || value.ToString() == string.Empty) return defaultValue;
        return int.TryParse(value.ToString(), out var newValue) ? newValue : defaultValue;
    }

    public static int? ParseObjectToNullableInt(this object? value)
    {
        if (value == null || value.ToString() == string.Empty) return null;
        int.TryParse(value.ToString(), out var newValue);
        return newValue;
    }

    public static DateTime ParseObjectToDateTime(this object? value, DateTime defaultValue = default)
    {
        if (value == null || value.ToString() == string.Empty) return defaultValue;
        return DateTime.TryParse(value.ToString(), out var newValue) ? newValue : defaultValue;
    }

    public static string FormatStringFromDictionary(this string formatString, IDictionary<string, string> valueDict)
    {
        foreach (var tuple in valueDict)
            formatString = Regex.Replace(formatString, "{" + tuple.Key + "}", valueDict[tuple.Key], RegexOptions.IgnoreCase);
        return formatString;
    }
}
