using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Vertesaur.Utility
{
    internal static class ListUtility
    {

#if NO_LIST_REMOVE_ALL
        public static int RemoveAll<T>(this List<T> list, Predicate<T> predicate) {
            Contract.Requires(null != list);
            Contract.Requires(null != predicate);
            Contract.Ensures(Contract.Result<int>() >= 0);
            int c = 0;
            for (int i = list.Count - 1; i >= 0; i--) {
                if (predicate(list[i])) {
                    list.RemoveAt(i);
                    c++;
                }
            }
            return c;
        }
#endif

#if NO_LIST_FIND_ALL
        public static List<T> FindAll<T>(this List<T> list, Predicate<T> predicate){
            Contract.Requires(list != null);
            Contract.Requires(predicate != null);
            Contract.Ensures(Contract.Result<List<T>>() != null);
            Contract.Ensures(Contract.Result<List<T>>().Count < list.Count);
            var result = new List<T>();
            foreach(var item in list){
                if(predicate(item))
                    result.Add(item);
            }
            return result;
        }
#endif


    }
}
