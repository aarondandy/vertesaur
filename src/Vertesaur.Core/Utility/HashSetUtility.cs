using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
    internal static class HashSetUtility
    {
        [Pure]
        public static T[] ToArray<T>(this HashSet<T> hashSet) {
            Contract.Requires(hashSet != null);
            Contract.Ensures(Contract.Result<T[]>() != null);
            Contract.Ensures(Contract.Result<T[]>().Length == hashSet.Count);
            var result = new T[hashSet.Count];
            hashSet.CopyTo(result);
            Contract.Assume(result.Length == hashSet.Count);
            return result;
        }

    }
}
