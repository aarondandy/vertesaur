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

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{
    [TestFixture]
    public class Ring2Test
    {

        [Test]
        public void RingConstructorTest() {
            var a = new Ring2();
            Assert.IsEmpty(a);
            Assert.IsNull(a.Hole);
            a = new Ring2(false);
            Assert.IsEmpty(a);
            Assert.AreEqual(false, a.Hole);
            a = new Ring2(true);
            Assert.IsEmpty(a);
            Assert.AreEqual(true, a.Hole);
            a = new Ring2(12);
            Assert.IsEmpty(a);
            Assert.IsNull(a.Hole);
            a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            Assert.AreEqual(4, a.Count);
            Assert.AreEqual(new Point2(0, 0), a[0]);
            Assert.AreEqual(new Point2(1, 0), a[1]);
            Assert.AreEqual(new Point2(0, 1), a[2]);
            Assert.AreEqual(new Point2(0, 0), a[3]);
        }

        [Test]
        public void SegmentTest() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            Assert.AreEqual(4, a.SegmentCount);
            Assert.AreEqual(new Segment2(new Point2(0, 0), new Point2(1, 0)), a.GetSegment(0));
            Assert.AreEqual(new Segment2(new Point2(1, 0), new Point2(0, 1)), a.GetSegment(1));
            Assert.AreEqual(new Segment2(new Point2(0, 1), new Point2(0, 0)), a.GetSegment(2));
            Assert.AreEqual(new Segment2(new Point2(0, 0), new Point2(0, 0)), a.GetSegment(3));
            a = new Ring2();
            Assert.AreEqual(0, a.SegmentCount);
            a = new Ring2(new[] { new Point2(0, 0) });
            Assert.AreEqual(0, a.SegmentCount);
            a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1) });
            Assert.AreEqual(1, a.SegmentCount);
            a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0) });
            Assert.AreEqual(3, a.SegmentCount);
        }

        [Test]
        public void RedundantTest() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0) });
            Assert.IsFalse(a.HasRedundantEndPoint);
            a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 1), new Point2(1, 0), new Point2(0, 0) });
            Assert.IsTrue(a.HasRedundantEndPoint);
        }

        [Test]
        public void MagnitudeTest() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            Assert.AreEqual(2 + System.Math.Sqrt(2), a.GetMagnitude());
        }

        [Test]
        public void AreaTest() {
            var a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) });
            Assert.AreEqual(0.5, a.GetArea());
            a = new Ring2(new[] { new Point2(0, 0), new Point2(1, 0), new Point2(0, 1), new Point2(0, 0) }, true);
            Assert.AreEqual(-0.5, a.GetArea());
        }

        [Test]
        public void CentroidTest() {
            var a = new Ring2(new[] {
				new Point2(0,0), 
				new Point2(1,0), 
				new Point2(1,.5), 
				new Point2(.5,.5), 
				new Point2(.5,.75), 
				new Point2(0,.75)
			});
            Assert.AreEqual(new Point2(9 / 20.0, 13 / 40.0), a.GetCentroid());
        }

        [Test]
        public void DetermineWindingTest() {
            var a = new Ring2(new[] {
				new Point2(0,0), 
				new Point2(1,0), 
				new Point2(1,.5), 
				new Point2(.5,.5), 
				new Point2(.5,.75), 
				new Point2(0,.75)
			});
            Assert.AreEqual(PointWinding.CounterClockwise, a.DetermineWinding());
            a = new Ring2(new[] { new Point2(0, 1), new Point2(1, 0), new Point2(0, 0) });
            Assert.AreEqual(PointWinding.Clockwise, a.DetermineWinding());
            a = new Ring2(new[] { new Point2(0, 0) });
            Assert.AreEqual(PointWinding.Unknown, a.DetermineWinding());
            a = new Ring2(true);
            Assert.AreEqual(PointWinding.Unknown, a.DetermineWinding());
            a = new Ring2(false);
            Assert.AreEqual(PointWinding.Unknown, a.DetermineWinding());
            a = new Ring2();
            Assert.AreEqual(PointWinding.Unknown, a.DetermineWinding());
        }

    }
}

#pragma warning restore 1591
