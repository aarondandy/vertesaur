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

namespace Vertesaur.PolygonOperation.Test {
	
	/// <summary>
	/// Utility class to support polygon operation testing.
	/// </summary>
	public static class PolyOperationTestUtility {

        public static void AssertSame<T>(IEnumerable<T> expected, IEnumerable<T> actual, Action<T, T> assertEquals)
        {
            if (ReferenceEquals(expected, actual)) return;
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            using (var expectedEnumerator = expected.GetEnumerator())
            using (var actualEnumerator = actual.GetEnumerator())
            {
                bool expectedMoved, actualMoved;
                do
                {
                    expectedMoved = expectedEnumerator.MoveNext();
                    actualMoved = actualEnumerator.MoveNext();
                    Assert.AreEqual(
                        expectedMoved,
                        actualMoved,
                        "Element count mismatch.");

                    assertEquals(expectedEnumerator.Current, actualEnumerator.Current);

                } while (expectedMoved && actualMoved);
            }

        }


		/// <summary>
		/// Generates a list of polygon pairs for testing.
		/// </summary>
		/// <returns>Pairs of polygons designed for testing.</returns>
		public static IEnumerable<PolyPairTestData> GeneratePolyPairTestData() {
			foreach(var ringPair in RingOperationTestUtility.GenerateRingPairTestData()) {
				yield return new PolyPairTestData(ringPair);
			}
			var chessBoard4 = GenerateChessboardData("Chess 4 Holes", 2, 2, true, true);
			yield return chessBoard4;
			var chessBoard9 = GenerateChessboardData("Chess 9 Holes", 3, 3, true, true);
			yield return chessBoard9;
			var chessBoard4FillHole = GenerateChessboardData("Chess 4 (2 Fills and 2 Holes)", 2, 2, false, true);
			yield return chessBoard4FillHole;
			var chessBoard9FillHole = GenerateChessboardData("Chess 9 (5 Fills and 4 Holes)", 3, 3, false, true);
			yield return chessBoard9FillHole;
			var threePartTriangle = new PolyPairTestData(
				"Three Part Triangle",
				new Polygon2(new[]{
					new Ring2(new[]{
						new Point2(0, 0),
						new Point2(1, 0),
						new Point2(1, 1)
					}, false),
					new Ring2(new[]{
						new Point2(1, 1),
						new Point2(2, 1),
						new Point2(2, 2)
					}, false)
				}), 
				new Polygon2(new[]{
					new Point2(1,0),
					new Point2(2,0), 
					new Point2(2,1), 
					new Point2(1,1), 
				}, false)
			);
			yield return threePartTriangle;
			yield return new PolyPairTestData(
				"Three Part Triangle Holes",
				new Polygon2(threePartTriangle.A.Select(r => new Ring2(r.Reverse(), true))),
				new Polygon2(threePartTriangle.B.Select(r => new Ring2(r.Reverse(), true)))
			);
			yield return new PolyPairTestData(
				"Three Part Triangle Hole Fill",
				new Polygon2(threePartTriangle.A.Select(r => new Ring2(r.Reverse(), true))),
				threePartTriangle.B.Clone()
			);
			yield return new PolyPairTestData(
				"Three Part Triangle Fill Hole",
				threePartTriangle.A.Clone(),
				new Polygon2(threePartTriangle.B.Select(r => new Ring2(r.Reverse(), true)))
			);
			yield return GenerateEightTriangleSquareData("Eight Triangle Fills", false, false);
			yield return GenerateEightTriangleSquareData("Eight Triangle Holes", true, true);
			yield return GenerateEightTriangleSquareData("Eight Triangle Fills and Holes", false, true);
			yield return GenerateSixTriangleLData("Six Triangle Fills", false, false);
			yield return GenerateSixTriangleLData("Six Triangle Holes", true, true);
			yield return GenerateSixTriangleLData("Six Triangle Fills and Holes", false, true);

		}

		/// <summary>
		/// Generates a list of polygon pairs for testing.
		/// </summary>
		/// <returns>Pairs of polygons designed for testing.</returns>
		public static PolyPairTestDataKeyedCollection GeneratePolyPairTestDataCollection() {
			return new PolyPairTestDataKeyedCollection(GeneratePolyPairTestData());
		}

		/// <summary>
		/// Generates a list of polygon pairs and their intersection results for testing.
		/// </summary>
		/// <returns>Pairs of polygons with expected intersection results.</returns>
		public static PolyPairTestDataKeyedCollection GeneratePolyPairIntersectionTestDataCollection() {
			return new PolyPairTestDataKeyedCollection(
				GeneratePolyPairTestData()
				.Select(ToIntersectionData)
				.Where(d => null != d)
			);
		}

		public static PolyPairTestData GenerateChessboardData(string name, int r, int c, bool aHole, bool bHole) {
			var a = new Polygon2();
			var b = new Polygon2();
			for(int i = 0; i < r; i++) {
				double yOffset = i;
				bool selectPolyA = 0 == i % 2;
				for(int j = 0; j < c; j++) {
					double xOffset = j;
					IEnumerable<Point2> points = new[]{
						new Point2(0 + xOffset, 0 + yOffset),
						new Point2(1 + xOffset, 0 + yOffset),
						new Point2(1 + xOffset, 1 + yOffset),
						new Point2(0 + xOffset, 1 + yOffset),
					};
					var isHole = selectPolyA ? aHole : bHole;
					if(isHole)
						points = points.Reverse();

					var ring = new Ring2(points, isHole);
					(selectPolyA ? a : b).Add(ring);

					selectPolyA = !selectPolyA;
				}
			}
			return new PolyPairTestData(name, a, b);
		}

		private static PolyPairTestData GenerateEightTriangleSquareData(string name, bool aHole, bool bHole) {
			var a = new Polygon2(){
				new Ring2(new[]{new Point2(0,0),new Point2(1,0),new Point2(1,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(0,1),new Point2(-1,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,0),new Point2(-1,-1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(0,-1),new Point2(1,-1)}, false),
			};
			var b = new Polygon2(){
				new Ring2(new[]{new Point2(0,0),new Point2(1,1),new Point2(0,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,1),new Point2(-1,0)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,-1),new Point2(0,-1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(1,-1),new Point2(1,0)}, false),
			};
			if(aHole) {
				foreach(var r in a) {
					r.Hole = true;
					var rev = r.Reverse().ToList();
					r.Clear();
					r.AddRange(rev);
				}
			}
			if (bHole) {
				foreach (var r in b) {
					r.Hole = true;
					var rev = r.Reverse().ToList();
					r.Clear();
					r.AddRange(rev);
				}
			}
			return new PolyPairTestData(name, a, b);
		}

		private static PolyPairTestData GenerateSixTriangleLData(string name, bool aHole, bool bHole) {
			var a = new Polygon2(){
				new Ring2(new[]{new Point2(0,0),new Point2(1,0),new Point2(1,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(0,1),new Point2(-1,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,0),new Point2(-1,-1)}, false),
			};
			var b = new Polygon2(){
				new Ring2(new[]{new Point2(0,0),new Point2(1,1),new Point2(0,1)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,1),new Point2(-1,0)}, false),
				new Ring2(new[]{new Point2(0,0),new Point2(-1,-1),new Point2(0,-1)}, false),
			};
			if (aHole) {
				foreach (var r in a) {
					r.Hole = true;
					var rev = r.Reverse().ToList();
					r.Clear();
					r.AddRange(rev);
				}
			}
			if (bHole) {
				foreach (var r in b) {
					r.Hole = true;
					var rev = r.Reverse().ToList();
					r.Clear();
					r.AddRange(rev);
				}
			}
			return new PolyPairTestData(name, a, b);
		}

		/// <summary>
		/// Converts a ring pair to polygon intersection test data.
		/// </summary>
		private static PolyPairTestData ToIntersectionData(PolyPairTestData data) {
			if (data.Name == "Eight Triangle Fills" || data.Name == "Six Triangle Fills") {
				return new PolyPairTestData(data, null);
			}
			if (data.Name == "Eight Triangle Holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(1,1),
					new Point2(1,-1), 
					new Point2(-1,-1), 
					new Point2(-1,1), 
				},true));
			}
			if (data.Name == "Six Triangle Holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(1,1),
					new Point2(1,0), 
					new Point2(0,0), 
					new Point2(0,-1), 
					new Point2(-1,-1), 
					new Point2(-1,1), 
				}, true));
			}
			if (data.Name == "Eight Triangle Fills and Holes" || data.Name == "Six Triangle Fills and Holes") {
				return new PolyPairTestData(data, data.A.Clone());
			}
			if (data.Name == "Three Part Triangle") {
				return new PolyPairTestData(data, null);
			}
			if (data.Name == "Three Part Triangle Holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0),
					new Point2(2,2),
					new Point2(2,0)
				}));
			}
			if (data.Name == "Three Part Triangle Hole Fill") {
				return new PolyPairTestData(data, data.B.Clone());
			}
			if (data.Name == "Three Part Triangle Fill Hole") {
				return new PolyPairTestData(data, data.A.Clone());
			}
			if (data.Name == "Chess 4 Holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0),
					new Point2(0,2),
					new Point2(2,2),
					new Point2(2,0)
				}, true));
			}
			if (data.Name == "Chess 9 Holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0),
					new Point2(0,3),
					new Point2(3,3),
					new Point2(3,0)
				}, true));
			}
			if (data.Name == "Chess 9 (5 Fills and 4 Holes)") {
				return new PolyPairTestData(data, data.A.Clone());
			}
			if (data.Name == "Chess 4 (2 Fills and 2 Holes)") {
				return new PolyPairTestData(data, data.A.Clone());
			}
			if (data.Name == "Cascade Boxes") {
				return new PolyPairTestData(data, new Polygon2(new []{
					new Point2(0.5,0.5),
					new Point2(1,0.5),
					new Point2(1,1),
					new Point2(0.5,1)
				}, false));
			}
			if (data.Name == "Cascade Boxes: reverse winding both") {
				return new PolyPairTestData(data, new Polygon2(new []{
					new Point2(0.5,0.5),
					new Point2(1,0.5),
					new Point2(1,1),
					new Point2(0.5,1)
				}.Reverse(), false));
			}
			if (data.Name == "Cascade Boxes: left down box") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0), 
					new Point2(.5,0), 
					new Point2(.5,.5), 
					new Point2(0,.5), 
				}, false));
			}
			if (data.Name == "Cascade Boxes: fill and a hole") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0), 
					new Point2(1,0), 
					new Point2(1,.5), 
					new Point2(.5,.5), 
					new Point2(.5,1), 
					new Point2(0,1), 
				}, false));
			}
			if (data.Name == "Cascade Boxes: dual hole") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0), 
					new Point2(0,1), 
					new Point2(.5,1), 
					new Point2(.5,1.5), 
					new Point2(1.5,1.5), 
					new Point2(1.5,.5), 
					new Point2(1, .5), 
					new Point2(1, 0), 
				}, true));
			}
			if (
				data.Name == "Triangle In Box: side touch"
				|| data.Name == "Triangle In Box: point touch"
				|| data.Name == "Diamond In Box: two ends touch sides"
				|| data.Name == "Trapezoid In Box: touches side"
				|| data.Name == "Z-Thing in a Box"
				|| data.Name == "One Box in Another: with one side segment within another"
				|| data.Name == "Same Boxes"
				|| data.Name == "Equal Boxes"
				|| data.Name == "Geometrically Equal Boxes"
				|| data.Name == "Boxes Overlapping: top half"
				|| data.Name == "Rectangle in Box: touching two sides"
				|| data.Name == "Nested: fill within another, not touching"
				|| data.Name == "Diamond in Double Diamond: touching sides"
				|| data.Name == "Fuzzed: 1"
				|| data.Name == "Fuzzed: 2"
				|| data.Name == "Fuzzed: 3"
				|| data.Name == "Fuzzed: 5"
			) {
				return new PolyPairTestData(data, new Polygon2(data.B));
			}
			if (
				data.Name == "Nested: hole within a hole, not touching"
				|| data.Name == "Fuzzed: 4"
			) {
				return new PolyPairTestData(data, new Polygon2(data.A));
			}
			if (data.Name == "Triangle Over Box: point segment through") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0), 
					new Point2(.5,0), 
					new Point2(.5,.25)
				}, false));
			}
			if (data.Name == "Triangle Over Box: segment through") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0.75),
					new Point2(0,.25),
					new Point2(.25,.25)
				}, false));
			}
			if (data.Name == "Trapezoid In Box: with crossing nub") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,.6),
					new Point2(0,.4),
					new Point2(.5,.25),
					new Point2(.5,.75)
				}, false));
			}
			if (
				data.Name == "Two Stacked Boxes"
				|| data.Name == "Two Boxes Touching: with one side segment within another"
				|| data.Name == "Nested: fill within a hole, not touching"
				|| data.Name == "Under Ledge"
			) {
				return new PolyPairTestData(data, null);
			}
			if (data.Name == "Two Sunk In Boxes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,.5),
					new Point2(1,.5),
					new Point2(1,1),
					new Point2(0,1),
				}, false));
			}
			if(data.Name == "Boxes Overlapping: within one segment") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,.75),
					new Point2(0,.25),
					new Point2(.25,.25),
					new Point2(.25,.75)
				}, false));
			}
			if(data.Name == "Boxes Overlapping: through two segments") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,.75),
					new Point2(0,.25),
					new Point2(1,.25),
					new Point2(1,.75)
				}, false));
			}
			if (data.Name == "Nested: hole within a fill, not touching") {
				return new PolyPairTestData(data, new Polygon2(
					new[]{data.A,data.B}
					.SelectMany(p => p)
					.Select(r => new Ring2(r))
				));
			}
			if (data.Name == "Cascade Diamond") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(1.5,.5),
					new Point2(2,1),
					new Point2(1.5,1.5),
					new Point2(1,1)
				}));
			}
			if (data.Name == "Zig-zag Thing") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(.5,0),
					new Point2(1,0),
					new Point2(1,.5),
					new Point2(.5,.5),
					new Point2(1,1),
					new Point2(0,1),
					new Point2(0,0)
				}));
			}
			if (data.Name == "Zig-zag Thing: holes") {
				return new PolyPairTestData(data, new Polygon2(new[]{
					new Point2(0,0),
					new Point2(0,1),
					new Point2(1,1),
					new Point2(1,.5),
					new Point2(1.5,.5), 
					new Point2(1.5,-.5), 
					new Point2(.5,-.5), 
					new Point2(.5,0),
				}, true));
			}
			return null;
		}

		/// <summary>
		/// Generates a list of polygon pairs and their union results for testing.
		/// </summary>
		/// <returns>Pairs of polygons with expected union results.</returns>
		public static PolyPairTestDataKeyedCollection GeneratePolyPairUnionTestDataCollection() {
			return new PolyPairTestDataKeyedCollection(
				RingOperationTestUtility.GenerateRingPairTestData()
				.Select(ToUnionData)
				.Where(d => null != d)
			);
		}

		/// <summary>
		/// Converts a ring pair to polygon union test data.
		/// </summary>
		private static PolyPairTestData ToUnionData(RingPairTestData data)
		{
			return null;
		}

	}

}
