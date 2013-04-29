using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
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
#if NETFX_CORE
            return new ReadOnlyCollection<T>(array);
#else
            return Array.AsReadOnly(array);
#endif
        }

    }
}
