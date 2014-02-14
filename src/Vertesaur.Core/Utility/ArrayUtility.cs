using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{

    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }

    internal static class ArrayUtility
    {

        public static void Sort<T>(this T[] array, Comparison<T> comparison) {
            Contract.Requires(null != array);
            Contract.Requires(null != comparison);
            Array.Sort(array, comparison);
        }

        public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array) {
            Contract.Requires(null != array);
            Contract.Ensures(Contract.Result<ReadOnlyCollection<T>>() != null);
            return new ReadOnlyCollection<T>(array);
        }

    }
}
