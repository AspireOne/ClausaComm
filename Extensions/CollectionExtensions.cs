using System;
using System.Collections.Generic;
using System.Linq;

namespace ClausaComm.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T elem in enumerable)
                action(elem);

            return enumerable;
        }
    }
}
