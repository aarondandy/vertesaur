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

	public static class RingOperationTestUtility
	{

		public static readonly string RingPairNameCascadeBoxes = "Cascade Boxes";

		public static IEnumerable<RingPairTestData> GenerateRingPairTestData() {

			var cascadeBoxes = new RingPairTestData(
				"Cascade Boxes",
				new Ring2(false){
					new Point2(0,0),
					new Point2(1,0), 
					new Point2(1,1),
					new Point2(0,1)},
				new Ring2(false) {
					new Point2(0.5,0.5),
					new Point2(1.5,0.5), 
					new Point2(1.5,1.5),
					new Point2(0.5,1.5)}
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1, .5),
					new Point2(.5, 1)
				}
			};
			yield return cascadeBoxes;

			yield return new RingPairTestData(
				"Cascade Boxes: fill and a hole",
				new Ring2(false){
					new Point2(0,0),
					new Point2(1,0), 
					new Point2(1,1),
					new Point2(0,1)},
				new Ring2(cascadeBoxes.B.Reverse(), true)
			) {
				CrossingPoints = new List<Point2>(cascadeBoxes.CrossingPoints)
			};

			yield return new RingPairTestData(
				"Cascade Boxes: dual hole",
				new Ring2(cascadeBoxes.A.Reverse(), true),
				new Ring2(cascadeBoxes.B.Reverse(), true)
			) {
				CrossingPoints = new List<Point2>(cascadeBoxes.CrossingPoints)
			};

			yield return new RingPairTestData(
				"Cascade Boxes: reverse winding A",
				new Ring2(cascadeBoxes.A.Reverse(), false),
				new Ring2(cascadeBoxes.B, false)
			) {
				CrossingPoints = new List<Point2>(cascadeBoxes.CrossingPoints)
			};

			yield return new RingPairTestData(
				"Cascade Boxes: reverse winding B",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(cascadeBoxes.B.Reverse(), false)
			) {
				CrossingPoints = new List<Point2>(cascadeBoxes.CrossingPoints)
			};

			yield return new RingPairTestData(
				"Cascade Boxes: reverse winding both",
				new Ring2(cascadeBoxes.A.Reverse(), false),
				new Ring2(cascadeBoxes.B.Reverse(), false)
			) {
				CrossingPoints = new List<Point2>(cascadeBoxes.CrossingPoints)
			};

			yield return new RingPairTestData(
				"Cascade Boxes: left down box",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(-.5,-.5),
					new Point2(.5,-.5),
					new Point2(.5,.5),
					new Point2(-.5,.5)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(.5,0),
					new Point2(0,.5)
				}
			};

			var leftTriangleInBox = new RingPairTestData(
				"Triangle In Box: side touch",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(0,.25),
					new Point2(.75,.5),
					new Point2(0,.75)
				}, false)
			) {
				CrossingPoints = new[] { new Point2(0, 0.25), new Point2(0, 0.75) }.ToList()
			};
			yield return leftTriangleInBox;

			yield return new RingPairTestData(
				"Triangle In Box: point touch",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new []{
					new Point2(0,.5),
					new Point2(.75,.25),
					new Point2(.75,.75)
				}, false)
			) {
				CrossingPoints = new[] { new Point2(0, .5) }.ToList()
			};

			var triangleOverBox = new RingPairTestData(
				"Triangle Over Box: point segment through",
				new Ring2(leftTriangleInBox.A, false),
				new Ring2(new Ring2(new[]{
					new Point2(0,0),
					new Point2(.5,-.25),
					new Point2(.5,.25)
				}, false), false)
			) {
				CrossingPoints = new[]{Point2.Zero, new Point2(0.5,0)}.ToList()
			};
			yield return triangleOverBox;

			yield return new RingPairTestData(
				"Triangle Over Box: segment through",
				new Ring2(leftTriangleInBox.A, false),
				new Ring2(new Ring2(new[]{
					new Point2(0,.75),
					new Point2(-.25,.25),
					new Point2(.25,.25)
				}, false), false)
			) {
				CrossingPoints = new[] { new Point2(0, .25), new Point2(0, .75) }.ToList()
			};

			var diamondInBox = new RingPairTestData(
				"Diamond In Box: two ends touch sides",
				new Ring2(new[]{
					new Point2(0,0),
					new Point2(1,0),
					new Point2(1,1),
					new Point2(0,1)
				}, false),
				new Ring2(new[]{
					new Point2(0,.5),
					new Point2(.5,.25),
					new Point2(1,.5),
					new Point2(.5,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, .5),
					new Point2(1, .5)
				}
			};
			yield return diamondInBox;

			var trapezoidInBox = new RingPairTestData(
				"Trapezoid In Box: touches side",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(0,.6),
					new Point2(0,.4),
					new Point2(.5,.25),
					new Point2(.5,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, .6),
					new Point2(0, .4)
				}
			};
			yield return trapezoidInBox;

			yield return new RingPairTestData(
				"Trapezoid In Box: with crossing nub",
				new Ring2(trapezoidInBox.A, false),
				new Ring2(new[]{
					new Point2(0,.6),
					new Point2(-.5,.6),
					new Point2(-.5,.4),
					new Point2(0,.4),
					new Point2(.5,.25),
					new Point2(.5,.75)
				}, false)
			) {
				CrossingPoints = trapezoidInBox.CrossingPoints.ToList()
			};

			var zThingInABox = new RingPairTestData(
				"Z-Thing in a Box",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(0,.75),
					new Point2(0,.5),
					new Point2(.25,.5),
					new Point2(0,.25),
					new Point2(1,.25),
					new Point2(1,.5),
					new Point2(.75,.5),
					new Point2(1,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, .75),
					new Point2(0, .5),
					new Point2(0, .25),
					new Point2(1, .25),
					new Point2(1, .5),
					new Point2(1, .75),
				}
			};
			yield return zThingInABox;

			var stackedBoxes = new RingPairTestData(
				"Two Stacked Boxes",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(cascadeBoxes.A.Select(p => p.Add(Vector2.YUnit)), false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, 1),
					new Point2(1, 1)
				}
			};
			yield return stackedBoxes;

			yield return new RingPairTestData(
				"Two Sunk In Boxes",
				new Ring2(stackedBoxes.A, false),
				new Ring2(stackedBoxes.A.Select(p => p.Add(new Vector2(0,0.5))), false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, 1),
					new Point2(0, .5),
					new Point2(1, .5),
					new Point2(1, 1)
				}
			};

			yield return new RingPairTestData(
				"Two Boxes Touching: with one side segment within another",
				new Ring2(stackedBoxes.A, false),
				new Ring2(new[]{
					new Point2(1,.25),
					new Point2(2,.25),
					new Point2(2,.75),
					new Point2(1,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1, .25),
					new Point2(1, .75)
				}
			};

			yield return new RingPairTestData(
				"One Box in Another: with one side segment within another",
				new Ring2(stackedBoxes.A, false),
				new Ring2(new[]{
					new Point2(0,.25),
					new Point2(.5,.25),
					new Point2(.5,.75),
					new Point2(0,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, .25),
					new Point2(0, .75)
				}
			};

			var sameRing = new Ring2(cascadeBoxes.A, false);
			yield return new RingPairTestData(
				"Same Boxes",
				sameRing,
				sameRing
			) {
				CrossingPoints = sameRing.ToList()
			};

			yield return new RingPairTestData(
				"Equal Boxes",
				new Ring2(sameRing, false),
				new Ring2(sameRing, false)
			) {
				CrossingPoints = sameRing.ToList()
			};

			yield return new RingPairTestData(
				"Geometrically Equal Boxes",
				new Ring2(sameRing, false),
				new Ring2(new[]{sameRing[2],sameRing[3],sameRing[0],sameRing[1]}, false)
			) {
				CrossingPoints = sameRing.ToList()
			};

			yield return new RingPairTestData(
				"Boxes Overlapping: within one segment",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(-.25,.25),
					new Point2(.25,.25),
					new Point2(.25,.75),
					new Point2(-.25,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0, .25),
					new Point2(0, .75)
				}
			};

			yield return new RingPairTestData(
				"Boxes Overlapping: through two segments",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(-.25,.25),
					new Point2(1.25,.25),
					new Point2(1.25,.75),
					new Point2(-.25,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1, .25),
					new Point2(1, .75),
					new Point2(0, .75),
					new Point2(0, .25)
				}
			};

			var topHalfBox = new Ring2(new[]{
				new Point2(0,.5),
				new Point2(1,.5),
				new Point2(1,1),
				new Point2(0,1)
			}, false);
			yield return new RingPairTestData(
				"Boxes Overlapping: top half",
				new Ring2(cascadeBoxes.A, false),
				topHalfBox
			) {
				CrossingPoints = topHalfBox.ToList()
			};

			var innerRectangle = new Ring2(new[]{
				new Point2(0,.25),
				new Point2(1,.25),
				new Point2(1,.75),
				new Point2(0,.75)
			}, false);
			yield return new RingPairTestData(
				"Rectangle in Box: touching two sides",
				new Ring2(cascadeBoxes.A, false),
				innerRectangle
			) {
				CrossingPoints = innerRectangle.ToList()
			};

			var fillInAnother = new RingPairTestData(
				"Nested: fill within another, not touching",
				new Ring2(cascadeBoxes.A, false),
				new Ring2(new[]{
					new Point2(.25,.25),
					new Point2(.75,.25),
					new Point2(.75,.75),
					new Point2(.25,.75)
				}, false)
			) {
				CrossingPoints = new List<Point2>(0)
			};
			yield return fillInAnother;

			yield return new RingPairTestData(
				"Nested: hole within a hole, not touching",
				new Ring2(fillInAnother.A.Reverse(), true),
				new Ring2(fillInAnother.B.Reverse(), true)
			) {
				CrossingPoints = new List<Point2>(0)
			};

			yield return new RingPairTestData(
				"Nested: hole within a fill, not touching",
				new Ring2(fillInAnother.A, false),
				new Ring2(fillInAnother.B.Reverse(), true)
			) {
				CrossingPoints = new List<Point2>(0)
			};

			yield return new RingPairTestData(
				"Nested: fill within a hole, not touching",
				new Ring2(fillInAnother.A.Reverse(), true),
				new Ring2(fillInAnother.B, false)
			) {
				CrossingPoints = new List<Point2>(0)
			};

			var diamondInDoubleDiamond = new RingPairTestData(
				"Diamond in Double Diamond: touching sides",
				new Ring2(new[]{
					new Point2(0.0,1.0),
					new Point2(1.0,0.0),
					new Point2(1.5,0.5),
					new Point2(2.0,0.0),
					new Point2(3.0,1.0),
					new Point2(2.0,2.0),
					new Point2(1.5,1.5),
					new Point2(1.0,2.0),
				}, false),
				new Ring2(new[]{
					new Point2(1.0,1.0),
					new Point2(2.0,0.0),
					new Point2(3.0,1.0),
					new Point2(2.0,2.0)
				}, false)
			);
			diamondInDoubleDiamond.CrossingPoints = new List<Point2> {
				new Point2(2.0,0.0),
				new Point2(3.0,1.0),
				new Point2(2.0,2.0),
				new Point2(1.5,1.5),
				new Point2(1.5,0.5)
			};
			yield return diamondInDoubleDiamond;

			yield return new RingPairTestData(
				"Cascade Diamond",
				new Ring2(new[]{
					new Point2(1,0),
					new Point2(2,1),
					new Point2(1,2),
					new Point2(0,1)
				}, false),
				new Ring2(new[]{
					new Point2(2,0),
					new Point2(3,1),
					new Point2(2,2),
					new Point2(1,1)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1.5, 1.5),
					new Point2(1.5, 0.5),
				}
			};

			var zigZag = new RingPairTestData(
				"Zig-zag Thing",
				new Ring2(new[]{
					new Point2(0,0),
					new Point2(.5,0),
					new Point2(.5,-.5),
					new Point2(1.5,-.5),
					new Point2(1.5,.5), 
					new Point2(.5,.5), 
					new Point2(1,1), 
					new Point2(0,1),
				}, false),
				new Ring2(cascadeBoxes.A, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1, 1),
					new Point2(0, 1),
					new Point2(0, 0),
					new Point2(.5, 0),
					new Point2(1, .5),
				}
			};
			yield return zigZag;

			yield return new RingPairTestData(
				"Zig-zag Thing: holes",
				new Ring2(zigZag.A.Reverse(), true),
				new Ring2(cascadeBoxes.A.Reverse(), true)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(1, 1),
					new Point2(0, 1),
					new Point2(0, 0),
					new Point2(.5, 0),
					new Point2(1, .5),
				}
			};

			yield return new RingPairTestData(
				"Under Ledge",
				new Ring2(new[]{
					new Point2(1,1),
					new Point2(2,1),
					new Point2(2,2),
					new Point2(1,2)
				}, false),
				new Ring2(new[]{
					new Point2(3,0),
					new Point2(4,0),
					new Point2(4,4),
					new Point2(0,4),
					new Point2(0,3),
					new Point2(3,3),
				}, false)
			) {
				CrossingPoints = new List<Point2>(0)
			};

			yield return new RingPairTestData(
				"Fuzzed: 1",
				new Ring2(new[]{
					new Point2(0.46987951807228917,0.40367346938775517),
					new Point2(0.44987951807228915,0.38367346938775515),
					new Point2(0.46987951807228917,0.36367346938775513),
					new Point2(0.47074010327022375,0.36453405458568972),
					new Point2(0.47160068846815834,0.36367346938775513),
					new Point2(0.49160068846815835,0.38367346938775515),
					new Point2(0.47160068846815834,0.40367346938775517),
					new Point2(0.47074010327022375,0.40281288418982059),
				}, false),
				new Ring2(new[]{
					new Point2(0.49160068846815835,0.38367346938775515),
					new Point2(0.47160068846815834,0.40367346938775517),
					new Point2(0.45160068846815832,0.38367346938775515),
					new Point2(0.47160068846815834,0.36367346938775513)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0.47074010327022375,0.40281288418982059),
					new Point2(0.47074010327022375,0.36453405458568972),
					new Point2(0.47160068846815834,0.36367346938775513),
					new Point2(0.49160068846815835,0.38367346938775515),
					new Point2(0.47160068846815834,0.40367346938775517),


				}
			};

			yield return new RingPairTestData(
				"Fuzzed: 2",
				new Ring2(new[]{
					new Point2(0.45611015490533563,0.44857142857142862),
					new Point2(0.43611015490533561,0.4285714285714286),
					new Point2(0.45611015490533563,0.40857142857142859),
					new Point2(0.47611015490533565,0.4285714285714286),
					new Point2(0.4742291615441358,0.43045242193262845),
				}, false),
				new Ring2(new[]{
					new Point2(0.47611015490533565,0.4285714285714286),
					new Point2(0.45611015490533563,0.44857142857142862),
					new Point2(0.43611015490533561,0.4285714285714286),
					new Point2(0.45611015490533563,0.40857142857142859)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0.45611015490533563,0.44857142857142862),
					new Point2(0.43611015490533561,0.4285714285714286),
					new Point2(0.45611015490533563,0.40857142857142859),
					new Point2(0.47611015490533565,0.4285714285714286),
					new Point2(0.4742291615441358,0.43045242193262845),
				}
			};

			yield return new RingPairTestData(
				"Fuzzed: 3",
				new Ring2(new[]{
					new Point2(0.5025817555938038	,0.51387755102040822	),
					new Point2(0.48258175559380379	,0.4938775510204082	),
					new Point2(0.5025817555938038	,0.47387755102040818	),
					new Point2(0.50344234079173833	,0.47473813621834277	),
					new Point2(0.504302925989673	,0.47387755102040818	),
					new Point2(0.50516351118760761	,0.47473813621834277	),
					new Point2(0.50602409638554213	,0.47387755102040818	),
					new Point2(0.52602409638554215	,0.4938775510204082	),
					new Point2(0.50602409638554213	,0.51387755102040822	),
					new Point2(0.50516351118760761	,0.51301696582247358	),
					new Point2(0.504302925989673	,0.51387755102040822	),
					new Point2(0.50344234079173833	,0.51301696582247369	)
				}, false),
				new Ring2(new[]{
					new Point2(0.52602409638554215	,0.4938775510204082	),
					new Point2(0.50602409638554213	,0.51387755102040822	),
					new Point2(0.48602409638554211	,0.4938775510204082	),
					new Point2(0.50602409638554213	,0.47387755102040818	)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0.50516351118760761	,0.47473813621834277	),
					new Point2(0.50602409638554213	,0.47387755102040818	),
					new Point2(0.52602409638554215	,0.4938775510204082	),
					new Point2(0.50602409638554213	,0.51387755102040822	),
					new Point2(0.50516351118760761	,0.51301696582247358	),
				}
			};

			yield return new RingPairTestData(
				"Fuzzed: 4",
				new Ring2(new[]{
					new Point2(0.42578313253012046,0.44489795918367347),
					new Point2(0.44492254733218589,0.42575854438160804),
					new Point2(0.46406196213425133,0.44489795918367347),
					new Point2(0.44492254733218589,0.46403737398573891)
				}, false),
				new Ring2(new[]{
					new Point2(0.46406196213425133,0.44489795918367347),
					new Point2(0.44406196213425131,0.46489795918367349),
					new Point2(0.42406196213425129,0.44489795918367347),
					new Point2(0.44406196213425131,0.42489795918367346)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0.44492254733218589,0.42575854438160804),
					new Point2(0.46406196213425133,0.44489795918367347),
					new Point2(0.44492254733218589,0.46403737398573891)
				}
			};

			yield return new RingPairTestData(
				"Fuzzed: 5",
				new Ring2(new[]{
					new Point2(0.32874354561101549,0.36979591836734693),
					new Point2(0.3487435456110155,0.38979591836734695),
					new Point2(0.32874354561101549,0.40979591836734697),
					new Point2(0.3278829604130809,0.40893533316941238),
					new Point2(0.32702237521514632,0.40979591836734697),
					new Point2(0.3070223752151463,0.38979591836734695),
					new Point2(0.32702237521514632,0.36979591836734693),
					new Point2(0.3278829604130809,0.37065650356528151)
				}, false),
				new Ring2(new[]{
					new Point2(0.34702237521514634,0.38979591836734695),
					new Point2(0.32702237521514632,0.40979591836734697),
					new Point2(0.3070223752151463,0.38979591836734695),
					new Point2(0.32702237521514632,0.36979591836734693)
				}, false)
			) {
				CrossingPoints = new List<Point2> {
					new Point2(0.3278829604130809,0.40893533316941238),
					new Point2(0.32702237521514632,0.40979591836734697),
					new Point2(0.3070223752151463,0.38979591836734695),
					new Point2(0.32702237521514632,0.36979591836734693),
					new Point2(0.3278829604130809,0.37065650356528151)
				}
			};

		}

		public static RingPairTestDataKeyedCollection GenerateRingPairTestDataCollection() {
			return new RingPairTestDataKeyedCollection(GenerateRingPairTestData());
		}

	}
}

#pragma warning restore 1591