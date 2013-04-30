using System;
using System.Collections.Generic;

namespace Vertesaur
{
    internal class HashSet<T> : IEnumerable<T>
    {

        private readonly Dictionary<T, T> _core;

        public HashSet() {
            _core = new Dictionary<T, T>();
        }

        public HashSet(IEnumerable<T> items)
            : this() {
            if (null != items) {
                foreach (var item in items) {
                    _core[item] = item;
                }
            }
        }

        public HashSet(IEqualityComparer<T> comparer)
            : this() {
            _core = new Dictionary<T, T>(comparer);
        }

        public int Count { get { return _core.Count; } }

        public bool Remove(T item) {
            return _core.Remove(item);
        }

        public T FirstOrDefault(Func<T, bool> test) {
            foreach (var item in _core.Keys) {
                if (test(item))
                    return item;
            }
            return default(T);
        }

        public bool Contains(T item) {
            return _core.ContainsKey(item);
        }

        public bool Add(T item) {
            if (_core.ContainsKey(item))
                return false;
            _core.Add(item, item);
            return true;
        }

        public IEnumerator<T> GetEnumerator() {
            return _core.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
