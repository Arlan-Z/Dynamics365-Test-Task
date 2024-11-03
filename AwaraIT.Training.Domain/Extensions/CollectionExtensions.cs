using System;
using System.Collections.Generic;
using System.Linq;

namespace AwaraIT.Training.Domain.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return source?.Any() ?? false;
        }

        public static void AddRange<T, K>(this IDictionary<T, K> target, IDictionary<T, K> source)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var element in source)
            {
                if (!target.ContainsKey(element.Key))
                    target.Add(element.Key, element.Value);
            }
        }
    }
}
