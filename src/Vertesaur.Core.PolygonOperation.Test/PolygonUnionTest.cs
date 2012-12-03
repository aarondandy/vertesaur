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
using NUnit.Framework;

namespace Vertesaur.PolygonOperation.Test
{
	[TestFixture]
	public class PolygonUnionTest
	{

		private PolygonUnionOperation _union;
		private PolyPairTestDataKeyedCollection _polyPairData;

        public PolygonUnionTest(){
            _polyPairData = PolyOperationTestUtility.GeneratePolyPairUnionTestDataCollection();
        }

		protected IEnumerable<object> GenerateTestPolyUnionParameters() {
			return _polyPairData;
		}

		[SetUp]
		public void SetUp() {
			_union = new PolygonUnionOperation();
		}

		[Test]
		public void TestPolyUnion([ValueSource("GenerateTestPolyUnionParameters")]PolyPairTestData testData) {
			Console.WriteLine(testData.Name);

			var result = _union.Union(testData.A, testData.B) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Forward case failed: {0} u {1} ≠ {2}", testData.A, testData.B, result);
			}
			else {
				Assert.IsNull(result);
			}

			result = _union.Union(testData.B, testData.A) as Polygon2;
			if (null != testData.R) {
				Assert.IsNotNull(result);
				Assert.IsTrue(testData.R.SpatiallyEqual(result), "Reverse case failed: {0} u {1} ≠ {2}", testData.B, testData.A, result);
			}
			else {
				Assert.IsNull(result);
			}
		}

	}
}
