using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vertesaur.PolygonOperation.Test
{
	[TestFixture]
	public class PolygonXorTest
	{

		private PolygonXorOperation _xorOperation;
		private PolyPairTestDataKeyedCollection _polyPairData;

		public PolygonXorTest() {
			_polyPairData = PolyOperationTestUtility.GeneratePolyPairXorTestDataCollection();
		}

		protected IEnumerable<object> GenerateTestPolyXorParameters() {
			return _polyPairData;
		}

		[SetUp]
		public void SetUp() {
			_xorOperation = new PolygonXorOperation();
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
		public void TestPolyIntersection([ValueSource("GenerateTestPolyXorParameters")]PolyPairTestData testData) {
			Console.WriteLine(testData.Name);

			if (testData.Name == "Fuzzed: 3") {
				Console.WriteLine("Skipping " + testData.Name + " ...need to test this one another way.");
				return;
			}

			var result = _xorOperation.Xor(testData.A, testData.B) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Forward case failed: {0} ∩ {1} ≠ {2}", testData.A, testData.B, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}

			result = _xorOperation.Xor(testData.B, testData.A) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Reverse case failed: {0} ∩ {1} ≠ {2}", testData.B, testData.A, PolygonToString(result));
			}
			else {
				Assert.IsNull(result);
			}
		}

	}
}
