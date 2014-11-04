using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vertesaur.PolygonOperation.Test
{
    /// <summary>
    /// A collection of polygon pair test data objects keyed on their names.
    /// </summary>
    public class PolyPairTestDataKeyedCollection : KeyedCollection<string, PolyPairTestData>
    {

        /// <summary>
        /// Default constructor of an empty collection.
        /// </summary>
        public PolyPairTestDataKeyedCollection() { }

        /// <summary>
        /// Creates a new collection with the given elements.
        /// </summary>
        /// <param name="polyPairs">The elements.</param>
        public PolyPairTestDataKeyedCollection(IEnumerable<PolyPairTestData> polyPairs) {
            foreach (var polyPair in polyPairs)
                Add(polyPair);
        }

        /// <inheritdoc/>
        protected override string GetKeyForItem(PolyPairTestData item) {
            return null == item ? null : item.Name;
        }
    }
}
