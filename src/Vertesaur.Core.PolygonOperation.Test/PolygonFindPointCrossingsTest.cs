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

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.PolygonOperation.Test {

	/// <summary>
	/// Various tests to verify that the points of intersection for two polygons are correct.
	/// </summary>
	[TestFixture]
	public class PolygonFindPointCrossingsTest {

		private PolygonIntersectionOperation _intersectionOperation;
		private PolyPairTestDataKeyedCollection _polyPairData;

        public PolygonFindPointCrossingsTest() {
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairTestDataCollection();
        }

		protected IEnumerable<object> GenerateTestPolyCrossingsParameters() {
			return _polyPairData.Where(pp => null != pp.CrossingPoints);
		}

		[SetUp]
		public void SetUp() {
			_intersectionOperation = new PolygonIntersectionOperation();
		}

		public static bool PointsAlmostEqual(Point2 a, Point2 b) {
			if (a == b)
				return true;
			var d = a.Difference(b);
			return d.GetMagnitudeSquared() < 0.000000000000000001;
		}


		[Test]
        public void TestPolyPointCrossings([ValueSource("GenerateTestPolyCrossingsParameters")] PolyPairTestData testData)
        {
            if (testData.Name == "Fuzzed: 3")
                Assert.Inconclusive("We must test this one a different way.");

			Console.WriteLine(testData.Name);

			var result = _intersectionOperation.FindPointCrossings(testData.A, testData.B);

            Assert.IsNotNull(result);
            Console.WriteLine("{0} crossing points", result.Count);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));

			result = _intersectionOperation.FindPointCrossings(testData.B, testData.A);
			Assert.IsNotNull(result);

            PolyOperationTestUtility.AssertSame(
                testData.CrossingPoints.OrderBy(x => x),
                result.Select(r => r.Point).OrderBy(x => x),
                (x, y) => Assert.That(PointsAlmostEqual(x, y), "Points not equal."));
		}

		[Test, Explicit("for debug")]
		public void CascadeBoxesTest() {
			var testData = _polyPairData[RingOperationTestUtility.RingPairNameCascadeBoxes];
			TestPolyPointCrossings(testData);
		}

	}
}