using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EmailMarketing.Common.Extensions
{
    public static class ConvertExtension
    {
        public static string ParseObjectToString(this Object value, string defaultValue = "")
        {
            if (value == null) return defaultValue;
            return value.ToString().Trim();
        }

        public static string ParseNullableDateTimeToString(this DateTime? value, string format)
        {
            if (value == null) return string.Empty;
            return value.Value.ToString(format);
        }

        public static Int32 ParseObjectToInt(this Object value, int defaultValue = 0)
        {
            if (value == null || value.ToString() == string.Empty) return defaultValue;
            return Int32.TryParse(value.ToString(), out var newValue) ? newValue : defaultValue;
        }

        public static Nullable<Int32> ParseObjectToNullableInt(this Object value)
        {
            if (value == null || value.ToString() == string.Empty) return null;
            Int32.TryParse(value.ToString(), out var newValue);
            return newValue;
        }

        public static DateTime ParseObjectToDateTime(this Object value, DateTime defaultValue = default(DateTime))
        {
            if (value == null || value.ToString() == string.Empty) return defaultValue;
            if (DateTime.TryParse(value.ToString(), out var newValue)) return newValue;
            else return defaultValue;
        }

        public static Nullable<DateTime> ParseObjectToNullableDateTime(this Object value)
        {
            if (value == null || value.ToString() == string.Empty) return null;
            if (DateTime.TryParse(value.ToString(), out var newValue)) return newValue;
            else return null;
        }

        public static string FormatStringFromDictionary(this string formatString, IDictionary<string, string> valueDict)
        {
            foreach (var tuple in valueDict)
            {
                formatString = Regex.Replace(formatString, "{" + tuple.Key + "}", valueDict[tuple.Key], RegexOptions.IgnoreCase);
            }
            return formatString;
        }
    }
}
