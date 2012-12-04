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
using System.Text;
using NUnit.Framework;

namespace Vertesaur.PolygonOperation.Test {

	[TestFixture]
	public class PolygonIntersectionTest {

		private PolygonIntersectionOperation _intersectionOperation;
		private PolyPairTestDataKeyedCollection _polyPairData;

		public PolygonIntersectionTest(){
			_polyPairData = PolyOperationTestUtility.GeneratePolyPairIntersectionTestDataCollection();
		}

		protected IEnumerable<object> GenerateTestPolyIntersectionParameters() {
			return _polyPairData;
		}

		[TestFixtureSetUp]
		public void FixtureSetUp() {
			
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
			foreach(var p in ring) {
				sb.AppendFormat("({0},{1})\n", p.X, p.Y);
			}
			return sb.ToString();
		}

		[Test]
		public void TestPolyIntersection([ValueSource("GenerateTestPolyIntersectionParameters")]PolyPairTestData testData) {
			Console.WriteLine(testData.Name);

			if (testData.Name == "Fuzzed: 3") {
				Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
				return;
			}

			var result = _intersectionOperation.Intersect(testData.A, testData.B) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Forward case failed: {0} ∩ {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}

			result = _intersectionOperation.Intersect(testData.B, testData.A) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Reverse case failed: {0} ∩ {1} ≠ {2}", testData.B, testData.A, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}
		}

		public Polygon2 ReverseWinding(Polygon2 p) {
			if (null == p)
				return null;
			return new Polygon2(p.Select(ReverseWinding));
		}

		public Ring2 ReverseWinding(Ring2 r) {
			if (null == r)
				return null;
			return r.Hole.HasValue
				? new Ring2(r.Reverse(), r.Hole.Value)
				: new Ring2(r.Reverse());
		}

		[Test]
		public void TestPolyReverseWindingIntersection([ValueSource("GenerateTestPolyIntersectionParameters")]PolyPairTestData testData) {
			Console.WriteLine(testData.Name);

			var a = ReverseWinding(testData.A);
			var b = ReverseWinding(testData.B);
			var r = ReverseWinding(testData.R);

			if (testData.Name == "Fuzzed: 3") {
				Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
				return;
			}

			var result = _intersectionOperation.Intersect(a, b) as Polygon2;
			if (null != r) {
				Assert.IsNotNull(result);
				Assert.IsTrue(r.SpatiallyEqual(result), "Forward case failed: {0} ∩ {1} ≠ {2}", a, b, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}

			result = _intersectionOperation.Intersect(b, a) as Polygon2;
			if (null != r) {
				Assert.IsNotNull(result);
				Assert.IsTrue(r.SpatiallyEqual(result), "Reverse case failed: {0} ∩ {1} ≠ {2}", b, a, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}
		}

		[Test, Explicit("for debug")]
		public void BoxAndDonutTest() {
			var donut = new Polygon2 {
				// outer boundary
				new Ring2(
					Enumerable.Range(0,8)
					.Select(i => i * 45 / 180.0 * Math.PI)
					.Select(t => new Point2(Math.Cos(t),Math.Sin(t)))
				){Hole = false},
				// inner hole
				new Ring2(
					Enumerable.Range(0,8)
					.Reverse()
					.Select(i => i * 45/ 180.0 * Math.PI)
					.Select(t => new Point2(Math.Cos(t)*0.5,Math.Sin(t)*0.5))
				) {Hole = true}
			};

			var box = new Polygon2(new Ring2(new[]{
				new Point2(0,0),
				new Point2(1,0),
				new Point2(1,1),
				new Point2(0,1)
			}) { Hole = false });

			var intersectionOperation = new PolygonIntersectionOperation();
			var result = intersectionOperation.Intersect(box, donut);
			Assert.Inconclusive();
		}

		[Test, Explicit("for debug")]
		public void CascadeBoxesTest() {
			var data = _polyPairData["Cascade Boxes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = data.A;
			var b = data.B;
			var r = data.R;

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

		}

		[Test, Explicit("for debug")]
		public void ReverseCascadeBoxesTest() {
			var data = _polyPairData["Cascade Boxes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = ReverseWinding(data.A);
			var b = ReverseWinding(data.B);
			var r = ReverseWinding(data.R);

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

		}

		[Test, Explicit("for debug")]
		public void ReverseSixTriangleHolesTest() {
			var data = _polyPairData["Six Triangle Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = ReverseWinding(data.A);
			var b = ReverseWinding(data.B);
			var r = ReverseWinding(data.R);

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

		}

		[Test, Explicit("for debug")]
		public void ReverseSixTriangleFillsHolesTest() {
			var data = _polyPairData["Six Triangle Fills and Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = ReverseWinding(data.A);
			var b = ReverseWinding(data.B);
			var r = ReverseWinding(data.R);

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

		}

		[Test, Explicit("for debug")]
		public void ReverseTwoStackedBoxesTest() {
			var data = _polyPairData["Two Stacked Boxes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = ReverseWinding(data.A);
			var b = ReverseWinding(data.B);
			var r = ReverseWinding(data.R);

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsNull(result);

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsNull(result);

		}

		[Test, Explicit("for debug")]
		public void ReverseZThingInBoxTest() {
			var data = _polyPairData["Z-Thing in a Box"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var a = ReverseWinding(data.A);
			var b = ReverseWinding(data.B);
			var r = ReverseWinding(data.R);

			var result = intersectionOperation.Intersect(a, b) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));

			result = intersectionOperation.Intersect(b, a) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(r));
		}

		[Test, Explicit("for debug")]
		public void TriangleInBoxSideTouchTest() {
			var data = _polyPairData["Triangle In Box: side touch"];
			var intersectionOperation = new PolygonIntersectionOperation();
			
			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));
			
			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void TwoStackedBoxesTest() {
			var data = _polyPairData["Two Stacked Boxes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsNull(result);

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsNull(result);

		}

		[Test, Explicit("for debug")]
		public void SameBoxesTest() {
			var data = _polyPairData["Same Boxes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void NestedFillWithinAnotherTest() {
			var data = _polyPairData["Nested: fill within another, not touching"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void HoleInHoleNoIntersectionTest() {
			var data = _polyPairData["Nested: hole within a hole, not touching"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void DiamondInDoubleDiamondTouchingSidesTest() {
			var data = _polyPairData["Diamond in Double Diamond: touching sides"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void ZigZagThingHolesTest() {
			var data = _polyPairData["Zig-zag Thing: holes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void CascadeFillHoleTest() {
			var data = _polyPairData["Cascade Boxes: fill and a hole"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test]
		public void Fuzzed3Test() {
			var data = _polyPairData["Fuzzed: 3"];
			var intersectionOperation = new PolygonIntersectionOperation();
			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result
				.SelectMany(r => r)
				.Select(p => data.R.DistanceSquared(p))
				.All(d => d < 0.000000000000000001)
			);
			var result2 = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result2
				.SelectMany(r => r)
				.Select(p => data.R.DistanceSquared(p))
				.All(d => d < 0.000000000000000001)
			);
		}

		[Test, Explicit("for debug")]
		public void ZThingInBoxTest() {
			var data = _polyPairData["Z-Thing in a Box"];
			var intersectionOperation = new PolygonIntersectionOperation();
			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));
			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));
		}

		[Test, Explicit("for debug")]
		public void Chess4HoleTest() {
			var data = _polyPairData["Chess 4 Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void Chess9HoleTest() {
			var data = _polyPairData["Chess 9 Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();

			var result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R));

		}

		[Test, Explicit("for debug")]
		public void Chess4FillHolesTest() {
			var data = _polyPairData["Chess 4 (2 Fills and 2 Holes)"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;

			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

		[Test, Explicit("for debug")]
		public void Chess9FillHolesTest() {
			var data = _polyPairData["Chess 9 (5 Fills and 4 Holes)"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;
			
			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

		[Test, Explicit("for debug")]
		public void ThreePartTrigFillHoleTest() {
			var data = _polyPairData["Three Part Triangle Fill Hole"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;

			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

		[Test, Explicit("for debug")]
		public void ThreePartTrigHolesTest() {
			var data = _polyPairData["Three Part Triangle Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;

			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

		[Test, Explicit("for debug")]
		public void EightTriangleFillsAndHolesTest() {
			var data = _polyPairData["Eight Triangle Fills and Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;

			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

		[Test, Explicit("for debug")]
		public void SixTriangleHolesTest() {
			var data = _polyPairData["Six Triangle Holes"];
			var intersectionOperation = new PolygonIntersectionOperation();
			Polygon2 result;

			result = intersectionOperation.Intersect(data.A, data.B) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Forward Result:\n {0}", PolygonToString(result));

			result = intersectionOperation.Intersect(data.B, data.A) as Polygon2;
			Assert.IsTrue(result.SpatiallyEqual(data.R), "Reverse Result:\n {0}", PolygonToString(result));

		}

	}
}
