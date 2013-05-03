using System.Collections.Generic;

namespace Vertesaur.Utility
{
    internal static class HashSetUtility
    {

        public static T[] ToArray<T>(this HashSet<T> hashSet) {
            var result = new T[hashSet.Count];
            hashSet.CopyTo(result);
            return result;
        }

    }
}
