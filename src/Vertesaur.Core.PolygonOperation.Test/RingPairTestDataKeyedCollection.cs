// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vertesaur.PolygonOperation.Test {
	/// <summary>
	/// A collection of ring pair test data objects keyed on their names.
	/// </summary>
	public class RingPairTestDataKeyedCollection : KeyedCollection<string,RingPairTestData>
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
