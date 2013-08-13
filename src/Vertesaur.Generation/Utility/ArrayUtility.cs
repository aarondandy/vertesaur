using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
    internal static class ArrayUtility
    {

        public static void Sort<T>(this T[] array, Comparison<T> comparison) {
            Contract.Requires(array != null);
            Contract.Requires(comparison != null);
            Array.Sort(array, comparison);
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array) {
            Contract.Requires(array != null);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<T>>() != null);
#if NETFX_CORE
            return new ReadOnlyCollection<T>(array);
#else
            return Array.AsReadOnly(array);
#endif
        }

#if NETFX_CORE || WINDOWS_PHONE || SILVERLIGHT
        public static TTo[] ConvertAll<TFrom, TTo>(this TFrom[] array, Func<TFrom, TTo> converter) {
            Contract.Requires(array != null);
            Contract.Requires(converter != null);
            Contract.Ensures(Contract.Result<TTo[]>() != null);
            var result = new TTo[array.Length];
            for (int i = 0; i < result.Length; i++) {
                result[i] = converter(array[i]);
            }
            return result;
        }
#else
        public static TTo[] ConvertAll<TFrom, TTo>(this TFrom[] array, Converter<TFrom, TTo> converter) {
            Contract.Requires(array != null);
            Contract.Requires(converter != null);
            Contract.Ensures(Contract.Result<TTo[]>() != null);
            Contract.Ensures(Contract.Result<TTo[]>().Length == array.Length);
            return Array.ConvertAll(array, converter);
        }
#endif

    }
}
