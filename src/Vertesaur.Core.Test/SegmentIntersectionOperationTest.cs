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

using NUnit.Framework;
using Vertesaur.Contracts;
using Vertesaur.SegmentOperation;

#pragma warning disable 1591
// ReSharper disable PossibleNullReferenceException

namespace Vertesaur.Core.Test {
	
	[TestFixture]
	public class SegmentIntersectionOperationTest {

		[Test]
		public void IntersectionCrossTest() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(0, 1), new Point2(1, 0));

			var res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(a, b);
			Assert.AreEqual(new Point2(.5, .5), res.P);
			Assert.AreEqual(new Vector2(0.5, 0.5), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);

			res = (SegmentIntersectionOperation.PointResult)SegmentIntersectionOperation.IntersectionDetails(b, a);
			Assert.AreEqual(new Point2(.5, .5), res.P);
			Assert.AreEqual(new Vector2(0.5, 0.5), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);
		}

		[Test]
		public void IntersectionCrossEdgeTest() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(0.5, 1.5), new Point2(1.5, .5));

			var res = (SegmentIntersectionOperation.PointResult) SegmentIntersectionOperation.IntersectionDetails(a, b);
			Assert.AreEqual(new Point2(1, 1), res.P);
			Assert.AreEqual(new Vector2(1, 0.5), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeB);

			res = (SegmentIntersectionOperation.PointResult) SegmentIntersectionOperation.IntersectionDetails(b, a);
			Assert.AreEqual(new Point2(1, 1), res.P);
			Assert.AreEqual(new Vector2(0.5, 1), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Range, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);
		}

		[Test]
		public void IntersectionCrossEnds() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(0.5, 1.5), new Point2(1, 1));

			var res = (SegmentIntersectionOperation.PointResult) SegmentIntersectionOperation.IntersectionDetails(a, b);
			Assert.AreEqual(new Point2(1, 1), res.P);
			Assert.AreEqual(new Vector2(1, 1), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);

			res = (SegmentIntersectionOperation.PointResult) SegmentIntersectionOperation.IntersectionDetails(b, a);
			Assert.AreEqual(new Point2(1, 1), res.P);
			Assert.AreEqual(new Vector2(1, 1), res.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.TypeB);
		}

		[Test]
		public void IntersectionSame() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(1, 1), new Point2(0, 0));

			var res = SegmentIntersectionOperation.IntersectionDetails(a, a) as SegmentIntersectionOperation.SegmentResult;
			AreSpatiallyEqual(a, res.S);
			AreSpatiallyEqual(b, res.S);
			Assert.AreEqual(a.A, res.S.A);
			Assert.AreEqual(a.B, res.S.B);
			Assert.AreEqual(new Vector2(0, 0), res.A.Ratios);
			Assert.AreEqual(new Vector2(1, 1), res.B.Ratios);

			res = SegmentIntersectionOperation.IntersectionDetails(b, b) as SegmentIntersectionOperation.SegmentResult;
			AreSpatiallyEqual(a, res.S);
			AreSpatiallyEqual(b, res.S);
			Assert.AreEqual(b.A, res.S.A);
			Assert.AreEqual(b.B, res.S.B);
			Assert.AreEqual(new Vector2(0, 0), res.A.Ratios);
			Assert.AreEqual(new Vector2(1, 1), res.B.Ratios);

			res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
			AreSpatiallyEqual(a, res.S);
			AreSpatiallyEqual(b, res.S);
			Assert.AreEqual(a.A, res.S.A);
			Assert.AreEqual(a.B, res.S.B);
			Assert.AreEqual(new Vector2(0, 1), res.A.Ratios);
			Assert.AreEqual(new Vector2(1, 0), res.B.Ratios);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Butt, res.A.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.A.TypeB);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Head, res.B.TypeA);
			Assert.AreEqual(SegmentIntersectionOperation.SegmentIntersectionType.Butt, res.B.TypeB);

			res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
			AreSpatiallyEqual(a, res.S);
			AreSpatiallyEqual(b, res.S);
			Assert.AreEqual(b.A, res.S.A);
			Assert.AreEqual(b.B, res.S.B);
			Assert.AreEqual(new Vector2(0, 1), res.A.Ratios);
			Assert.AreEqual(new Vector2(1, 0), res.B.Ratios);

		}

		[Test]
		public void IntersectionOverlap() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(.5, .5), new Point2(2, 2));
			var exp = new Segment2(new Point2(.5, .5), new Point2(1, 1));

			var res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
			Assert.AreEqual(exp, res.S);
			Assert.AreEqual(new Vector2(.5, 0), res.A.Ratios);
			Assert.AreEqual(new Vector2(1, 1/3.0), res.B.Ratios);

			res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
			Assert.AreEqual(exp, res.S);
			Assert.AreEqual(new Vector2(0, .5), res.A.Ratios);
			Assert.AreEqual(new Vector2(1 / 3.0, 1), res.B.Ratios);
		}

		[Test]
		public void IntersectionWithin() {
			var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
			var b = new Segment2(new Point2(-.5, -.5), new Point2(2, 2));

			var res = SegmentIntersectionOperation.IntersectionDetails(a, b) as SegmentIntersectionOperation.SegmentResult;
			Assert.AreEqual(a, res.S);
			Assert.AreEqual(new Vector2(0, .2), res.A.Ratios);
			Assert.AreEqual(1, res.B.S);
			Assert.AreEqual(.6, res.B.T,0.0001);

			res = SegmentIntersectionOperation.IntersectionDetails(b, a) as SegmentIntersectionOperation.SegmentResult;
			Assert.AreEqual(a, res.S);
			Assert.AreEqual(new Vector2(.2, 0), res.A.Ratios);
			Assert.AreEqual(.6, res.B.S, 0.0001);
			Assert.AreEqual(1, res.B.T);
		}

		[Test]
		public void ParallelNoIntersection() {
			var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
			var b = new Segment2(new Point2(1, 1), new Point2(2, 0));

			var res = SegmentIntersectionOperation.IntersectionDetails(a, b);
			Assert.IsNull(res.Geometry);

			res = SegmentIntersectionOperation.IntersectionDetails(b, a);
			Assert.IsNull(res.Geometry);
		}

		[Test]
		public void NoIntersectionPerpendicular() {
			var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
			var b = new Segment2(new Point2(2, 2), new Point2(1, 1));

			var res = SegmentIntersectionOperation.IntersectionDetails(a, b);
			Assert.IsNull(res.Geometry);

			res = SegmentIntersectionOperation.IntersectionDetails(b, a);
			Assert.IsNull(res.Geometry);
		}

		private static void AreSpatiallyEqual(object expected, object result) {
			if (result is ISpatiallyEquatable<Segment2> && expected is Segment2) {
				Assert.That(result is ISpatiallyEquatable<Segment2>);
				Assert.IsTrue(
					((ISpatiallyEquatable<Segment2>)result)
					.SpatiallyEqual(expected as Segment2)
				);
			}
			else {
				Assert.AreEqual(expected, result);
			}
		}

	}
}
