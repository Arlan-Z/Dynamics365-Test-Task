using AwaraIT.Training.Plugins.PluginExtensions.Attributes;
using System;
using System.ComponentModel;
using System.Globalization;

namespace AwaraIT.Training.Plugins.PluginExtensions.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string value, string otherValue)
        {
            return string.Equals(value, otherValue, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string value, string pattern)
        {
            Argument.NotNullOrEmpty(value, "String to be compared is required.");
            Argument.NotNullOrEmpty(pattern, "String pattern is required.");
            return value.ToUpper(CultureInfo.CurrentCulture).Contains(pattern.ToUpper(CultureInfo.CurrentCulture));
        }

        public static bool TryParse<TValue>(this string str, out TValue value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
                value = (TValue)converter.ConvertFromInvariantString(str);
                return true;
            }
            catch
            {
                value = default(TValue);
                return false;
            }
        }
    }
}
