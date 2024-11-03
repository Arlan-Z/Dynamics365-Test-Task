using System.Linq;

namespace AwaraIT.Training.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string Crop(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || length < 0 || value.Length <= length)
            {
                return value;
            }
            return value.Substring(0, length);
        }

        public static string OnlyDigits(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return new string(value.Where(ch => char.IsDigit(ch)).ToArray());
        }
    }
}
