using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vertesaur.PolygonOperation.Test
{
    /// <summary>
    /// A collection of ring pair test data objects keyed on their names.
    /// </summary>
    public class RingPairTestDataKeyedCollection : KeyedCollection<string, RingPairTestData>
    {

        /// <summary>
        /// Default constructor of an empty collection.
        /// </summary>
        public RingPairTestDataKeyedCollection() : base() { }

        /// <summary>
        /// Creates a new collection with the given elements.
        /// </summary>
        /// <param name="ringPairs">The elements.</param>
        public RingPairTestDataKeyedCollection(IEnumerable<RingPairTestData> ringPairs) {
            foreach (var ringPair in ringPairs)
                Add(ringPair);
        }

        /// <inheritdoc/>
        protected override string GetKeyForItem(RingPairTestData item) {
            return null == item ? null : item.Name;
        }
    }
}
