using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Vertesaur.Generation.Utility
{
    internal static class ArrayUtility<T>
    {
        private static readonly T[] _empty = new T[0];

        public static T[] Empty {
            get {
                Contract.Ensures(Contract.Result<T[]>() != null);
                Contract.Ensures(Contract.Result<T[]>().Length == 0);
                Contract.Assume(_empty.Length == 0);
                return _empty;
            }
        }
    }

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
            Contract.Ensures(Contract.Result<ReadOnlyCollection<T>>().Count == array.Length);
#if DEBUG
            var result = new ReadOnlyCollection<T>(array);
            Contract.Assume(result.Count == array.Length);
            return result;
#else
            return new ReadOnlyCollection<T>(array);
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
#if DEBUG
            var result = Array.ConvertAll(array, converter);
            Contract.Assume(result.Length == array.Length);
            return result;
#else
            return Array.ConvertAll(array, converter);
#endif
        }
#endif

    }
}
