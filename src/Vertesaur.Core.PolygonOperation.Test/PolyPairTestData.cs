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
using System.Linq;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test {

	/// <summary>
	/// An object storing test data that is to be used in other tests.
	/// </summary>
	public class PolyPairTestData {

		public PolyPairTestData(string name, Polygon2 a, Polygon2 b) {
			Name = name;
			A = a;
			B = b;
		}

		public PolyPairTestData(string name, Polygon2 a, Polygon2 b, Polygon2 r) {
			Name = name;
			A = a;
			B = b;
			R = r;
		}

		public PolyPairTestData(RingPairTestData ringData) : this(ringData, null) { }

		public PolyPairTestData(PolyPairTestData data, Polygon2 result) {
			Name = data.Name;
			A = new Polygon2(data.A);
			B = new Polygon2(data.B);
			R = null == result ? data.R : new Polygon2(result);
			CrossingPoints = (data.CrossingPoints ?? Enumerable.Empty<Point2>()).ToList();
		}

		public PolyPairTestData(RingPairTestData data, Polygon2 result) {
			Name = data.Name;
			A = new Polygon2(data.A);
			B = new Polygon2(data.B);
			R = null == result ? null : new Polygon2(result);
			CrossingPoints = (data.CrossingPoints ?? Enumerable.Empty<Point2>()).ToList();
		}

		public string Name { get; private set; }

		public Polygon2 A { get; private set; }

		public Polygon2 B { get; private set; }

		/// <summary>
		/// The expected result.
		/// </summary>
		public Polygon2 R { get; private set; }

		public List<Point2> CrossingPoints { get; set; }

		public override string ToString() {
			return Name;
		}

	}

}

#pragma warning restore 1591