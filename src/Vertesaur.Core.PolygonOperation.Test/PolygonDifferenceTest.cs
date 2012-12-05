// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
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


using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vertesaur.PolygonOperation.Test
{
	[TestFixture]
	public class PolygonDifferenceTest
	{

		private PolygonDifferenceOperation _differenceOperation;
		private PolyPairTestDataKeyedCollection _polyPairData;

		public PolygonDifferenceTest() {
			_polyPairData = PolyOperationTestUtility.GeneratePolyPairDifferenceTestDataCollection();
		}

		protected IEnumerable<object> GenerateTestPolyDifferenceParameters() {
			return _polyPairData;
		}

		[SetUp]
		public void SetUp() {
			_differenceOperation = new PolygonDifferenceOperation();
		}

		public static bool PointsAlmostEqual(Point2 a, Point2 b) {
			if (a == b)
				return true;
			var d = a.Difference(b);
			return d.GetMagnitudeSquared() < 0.000000000000000001;
		}

		private static string PolygonToString(Polygon2 poly) {
			var sb = new StringBuilder();
			for (int index = 0; index < poly.Count; index++) {
				var ring = poly[index];
				sb.AppendFormat("Ring {0}:\n", index);
				sb.AppendLine(RingToString(ring));
			}
			return sb.ToString();
		}

		private static string RingToString(Ring2 ring) {
			var sb = new StringBuilder();
			foreach (var p in ring) {
				sb.AppendFormat("({0},{1})\n", p.X, p.Y);
			}
			return sb.ToString();
		}

		[Test]
		public void TestPolyDifference([ValueSource("GenerateTestPolyDifferenceParameters")]PolyPairTestData testData) {
			Console.WriteLine(testData.Name);

			var result = _differenceOperation.Difference(testData.A, testData.B) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Failed: {0} - {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}
		}

	}
}
