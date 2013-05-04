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
using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{

    /// <summary>
    /// Tests for Segment2
    /// </summary>
    [TestFixture]
    public class Segment2Test
    {
        [Test]
        public void FieldValueTest() {
            var s = new Segment2(new Point2(0, 0), new Point2(2, 3));
            Assert.AreEqual(new Point2(0, 0), s.A);
            Assert.AreEqual(new Point2(2, 3), s.B);
        }

        [Test]
        public void CopyConstructorTest() {
            var i = new Segment2(new Point2(1, 2), new Point2(4, 5));
            var j = new Segment2(i);
            Assert.AreEqual(new Point2(1, 2), j.A);
            Assert.AreEqual(new Point2(4, 5), j.B);

        }

        [Test]
        public void DirectionTest() {
            var u = new Segment2(new Point2(2, 1), new Point2(-1, 3));
            Assert.AreEqual(new Vector2(-3, 2), u.Direction);
        }

        [Test]
        public void EqualsTest() {
            var z = new Segment2(new Point2(1, 3), new Point2(2, 5));
            var q = new Segment2(new Point2(1, 3), new Point2(2, 5));
            Assert.AreEqual(z, q);
            Assert.AreEqual(q, z);
            q = new Segment2(new Point2(0, 2), new Point2(5, 6));
            Assert.AreNotEqual(z, q);
            Assert.AreNotEqual(q, z);
        }

        [Test]
        public void DistancePointTest() {
            var s = new Segment2(Point2.Zero, new Point2(4, 2));
            Assert.AreEqual(3, s.Distance(new Point2(-3, 0)));
            Assert.AreEqual(Math.Sqrt(18), s.Distance(new Point2(7, 5)));
            Assert.AreEqual(Math.Sqrt(5), s.Distance(new Point2(1, 3)));
        }

        [Test]
        public void DistanceSquaredPointTest() {
            var s = new Segment2(new Point2(), new Point2(4, 2));
            Assert.AreEqual(9, s.DistanceSquared(new Point2(-3, 0)));
            Assert.AreEqual(18, s.DistanceSquared(new Point2(7, 5)));
            Assert.AreEqual(5, s.DistanceSquared(new Point2(1, 3)));
        }

        [Test]
        public void IntersectsPointTest() {
            var s = new Segment2(new Point2(), new Point2(4, 2));
            Assert.IsFalse(s.Intersects(new Point2(-3, 0)));
            Assert.IsFalse(s.Intersects(new Point2(7, 5)));
            Assert.IsFalse(s.Intersects(new Point2(1, 3)));
            Assert.IsTrue(s.Intersects(new Point2()));
            Assert.IsTrue(s.Intersects(new Point2(4, 2)));
            Assert.IsTrue(s.Intersects(new Point2(2, 1)));
        }

        [Test]
        public void MagnitudeTest() {
            var l = new Segment2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(Math.Sqrt(9 + 16), l.GetMagnitude());
        }

        [Test]
        public void MagnitudeSquaredTest() {
            var l = new Segment2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(9 + 16, l.GetMagnitudeSquared());
        }

        [Test]
        public void GetCentroidTest() {
            var l = new Segment2(new Point2(1, 2), new Vector2(3, 4));
            Assert.AreEqual(new Point2(1 + (3.0 / 2.0), 2 + (4 / 2)), l.GetCentroid());
        }

        [Test]
        public void DistancePointReliabilityTest() {
            var a = new Point2(BitConverter.Int64BitsToDouble(4604914652251415285), BitConverter.Int64BitsToDouble(605372236616115020));
            var b = new Point2(BitConverter.Int64BitsToDouble(4597194308594501564), BitConverter.Int64BitsToDouble(595495711103841793));
            var c = new Point2(BitConverter.Int64BitsToDouble(4600243336160129506), BitConverter.Int64BitsToDouble(606560426639725722));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598185498284921522), BitConverter.Int64BitsToDouble(585718899494321686));
            b = new Point2(BitConverter.Int64BitsToDouble(4604929952535543498), BitConverter.Int64BitsToDouble(604109661965789872));
            c = new Point2(BitConverter.Int64BitsToDouble(4601225085229312940), BitConverter.Int64BitsToDouble(605721095959846367));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602678972841459784), BitConverter.Int64BitsToDouble(602930345403337094));
            b = new Point2(BitConverter.Int64BitsToDouble(4604035258510844761), BitConverter.Int64BitsToDouble(606837598738812103));
            c = new Point2(BitConverter.Int64BitsToDouble(4606080449305389959), BitConverter.Int64BitsToDouble(604233694275374157));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4600442574156602443), BitConverter.Int64BitsToDouble(598207236037163581));
            b = new Point2(BitConverter.Int64BitsToDouble(4594502304813934338), BitConverter.Int64BitsToDouble(606104723491674031));
            c = new Point2(BitConverter.Int64BitsToDouble(4604774769151108227), BitConverter.Int64BitsToDouble(602799617759828923));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606663833178362035), BitConverter.Int64BitsToDouble(603250972966523070));
            b = new Point2(BitConverter.Int64BitsToDouble(4601480025126435688), BitConverter.Int64BitsToDouble(596774618449448018));
            c = new Point2(BitConverter.Int64BitsToDouble(4603452121619463841), BitConverter.Int64BitsToDouble(603812641245761129));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602900123144524429), BitConverter.Int64BitsToDouble(598130168765066766));
            b = new Point2(BitConverter.Int64BitsToDouble(4602249248711176862), BitConverter.Int64BitsToDouble(607165635300942201));
            c = new Point2(BitConverter.Int64BitsToDouble(4599216937047123680), BitConverter.Int64BitsToDouble(606879582652324133));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598496169291827150), BitConverter.Int64BitsToDouble(593785135637253756));
            b = new Point2(BitConverter.Int64BitsToDouble(4605980578044933597), BitConverter.Int64BitsToDouble(604432066785604900));
            c = new Point2(BitConverter.Int64BitsToDouble(4586666280293906781), BitConverter.Int64BitsToDouble(605343492497141507));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598846179330278520), BitConverter.Int64BitsToDouble(606199123660047206));
            b = new Point2(BitConverter.Int64BitsToDouble(4599493010557328652), BitConverter.Int64BitsToDouble(585727097513151743));
            c = new Point2(BitConverter.Int64BitsToDouble(4603015160794342349), BitConverter.Int64BitsToDouble(605551305890752774));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4592419707721030345), BitConverter.Int64BitsToDouble(4606370670874082160));
            b = new Point2(BitConverter.Int64BitsToDouble(4602826327758802001), BitConverter.Int64BitsToDouble(4600543439500661700));
            c = new Point2(BitConverter.Int64BitsToDouble(4602463323412133892), BitConverter.Int64BitsToDouble(4601203408898458687));

            Assert.AreEqual(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );
        }

        [Test]
        public void IntersectionPointReliabilityTest() {

            var a = new Point2(BitConverter.Int64BitsToDouble(4604031280725203997), BitConverter.Int64BitsToDouble(603767128270027679));
            var b = new Point2(BitConverter.Int64BitsToDouble(4599461212347507509), BitConverter.Int64BitsToDouble(592115755133497830));
            var c = new Point2(BitConverter.Int64BitsToDouble(4601157578736873697), BitConverter.Int64BitsToDouble(598204225474802883));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4601380850531485442), BitConverter.Int64BitsToDouble(589188805350663631));
            b = new Point2(BitConverter.Int64BitsToDouble(4586336351710601530), BitConverter.Int64BitsToDouble(606703828463294835));
            c = new Point2(BitConverter.Int64BitsToDouble(4587218570205865465), BitConverter.Int64BitsToDouble(606577244964107058));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606030312587645013), BitConverter.Int64BitsToDouble(594736814910574996));
            b = new Point2(BitConverter.Int64BitsToDouble(4594874457891834867), BitConverter.Int64BitsToDouble(598893481231194755));
            c = new Point2(BitConverter.Int64BitsToDouble(4601180880956119876), BitConverter.Int64BitsToDouble(597846328687307200));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4607123179444343871), BitConverter.Int64BitsToDouble(597539282599246653));
            b = new Point2(BitConverter.Int64BitsToDouble(4587595002547589811), BitConverter.Int64BitsToDouble(605930046567684595));
            c = new Point2(BitConverter.Int64BitsToDouble(4599436376715884040), BitConverter.Int64BitsToDouble(604316376715993779));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4594904703957517048), BitConverter.Int64BitsToDouble(604506264379915290));
            b = new Point2(BitConverter.Int64BitsToDouble(4606426535291429134), BitConverter.Int64BitsToDouble(590923593376627038));
            c = new Point2(BitConverter.Int64BitsToDouble(4599222167419711587), BitConverter.Int64BitsToDouble(603414706481609362));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4607087178592244418), BitConverter.Int64BitsToDouble(602721354135915871));
            b = new Point2(BitConverter.Int64BitsToDouble(4601476038364984360), BitConverter.Int64BitsToDouble(590931826761828440));
            c = new Point2(BitConverter.Int64BitsToDouble(4606962274043588495), BitConverter.Int64BitsToDouble(602576263058507124));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602350326177966714), BitConverter.Int64BitsToDouble(604956798367051679));
            b = new Point2(BitConverter.Int64BitsToDouble(4605541086351611503), BitConverter.Int64BitsToDouble(572436839363321385));
            c = new Point2(BitConverter.Int64BitsToDouble(4604583547719485614), BitConverter.Int64BitsToDouble(597873959906434051));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606427041690722810), BitConverter.Int64BitsToDouble(603384191280022288));
            b = new Point2(BitConverter.Int64BitsToDouble(4603168764381592373), BitConverter.Int64BitsToDouble(606547930499481055));
            c = new Point2(BitConverter.Int64BitsToDouble(4603755297686136346), BitConverter.Int64BitsToDouble(605978415323378158));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4592419707721030345), BitConverter.Int64BitsToDouble(4606370670874082160));
            b = new Point2(BitConverter.Int64BitsToDouble(4602826327758802001), BitConverter.Int64BitsToDouble(4600543439500661700));
            c = new Point2(BitConverter.Int64BitsToDouble(4602463323412133892), BitConverter.Int64BitsToDouble(4601203408898458687));

            Assert.AreEqual(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

        }

        [Test]
        public void IntersectionCrossTest() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var res = a.Intersection(b);
            Assert.AreEqual(new Point2(.5, .5), res);
            res = b.Intersection(a);
            Assert.AreEqual(new Point2(.5, .5), res);
        }

        [Test]
        public void IntersectionCrossEdgeTest() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1.5, .5));
            var res = a.Intersection(b);
            Assert.AreEqual(new Point2(1, 1), res);
            res = b.Intersection(a);
            Assert.AreEqual(new Point2(1, 1), res);
        }

        [Test]
        public void IntersectionCrossEnds() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.AreEqual(new Point2(1, 1), res);
            res = b.Intersection(a);
            Assert.AreEqual(new Point2(1, 1), res);
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

        [Test]
        public void IntersectionSame() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(1, 1), new Point2(0, 0));
            var res = a.Intersection(a);
            AreSpatiallyEqual(a, res);
            AreSpatiallyEqual(b, res);
            res = b.Intersection(b);
            AreSpatiallyEqual(a, res);
            AreSpatiallyEqual(b, res);
            res = a.Intersection(b);
            AreSpatiallyEqual(a, res);
            AreSpatiallyEqual(b, res);
            res = b.Intersection(a);
            AreSpatiallyEqual(a, res);
            AreSpatiallyEqual(b, res);
        }

        [Test]
        public void IntersectionOverlap() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(.5, .5), new Point2(2, 2));
            var exp = new Segment2(new Point2(.5, .5), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.AreEqual(exp, res);
            res = b.Intersection(a);
            Assert.AreEqual(exp, res);
        }

        [Test]
        public void IntersectionWithin() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(-.5, -.5), new Point2(2, 2));
            var res = a.Intersection(b);
            Assert.AreEqual(a, res);
            res = b.Intersection(a);
            Assert.AreEqual(a, res);
        }

        [Test]
        public void ParallelNoIntersection() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(1, 1), new Point2(2, 0));
            var res = a.Intersection(b);
            Assert.IsNull(res);
            res = b.Intersection(a);
            Assert.IsNull(res);
        }

        [Test]
        public void NoIntersectionPerpendicular() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(2, 2), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.IsNull(res);
            res = b.Intersection(a);
            Assert.IsNull(res);
        }

        [Test]
        public void LineIntersectionResultTest() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Line2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Line2(new Point2(1, 1), new Vector2(0, 1));
            var d = new Line2(new Point2(0, 0), new Vector2(0, 1));

            Assert.AreEqual(new Point2(0, 0), a.Intersection(b));
            Assert.AreEqual(null, a.Intersection(c));
            Assert.AreEqual(a, a.Intersection(d));
        }

        [Test]
        public void LineIntersectionCheckTest() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Line2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Line2(new Point2(1, 1), new Vector2(0, 1));
            var d = new Line2(new Point2(0, 0), new Vector2(0, 1));

            Assert.IsTrue(a.Intersects(b));
            Assert.IsFalse(a.Intersects(c));
            Assert.IsTrue(a.Intersects(d));
        }

        [Test]
        public void RayIntersectionResultTest() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Ray2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Ray2(new Point2(1, 1), new Vector2(-1, -1));
            var d = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var e = new Ray2(new Point2(0, -1), new Vector2(0, -1));
            var f = new Ray2(new Point2(0, 0), new Vector2(0, 3));

            Assert.AreEqual(null, a.Intersection(b));
            Assert.AreEqual(new Point2(0, 0), a.Intersection(c));

            Assert.AreEqual(null, a.Intersection(new Ray2(new Point2(0, 2), new Vector2(0, 1))));
            Assert.AreEqual(null, a.Intersection(new Ray2(new Point2(0, -2), new Vector2(0, -1))));

            Assert.AreEqual(new Point2(0, 1), a.Intersection(d));
            Assert.AreEqual(a, a.Intersection(d.GetReverse()));
            Assert.AreEqual(new Point2(0, -1), a.Intersection(e));
            Assert.AreEqual(a, a.Intersection(e.GetReverse()));
            Assert.AreEqual(new Segment2(f.P, a.B), a.Intersection(f));
            Assert.AreEqual(new Segment2(a.A, f.P), a.Intersection(f.GetReverse()));
        }

        [Test]
        public void RayIntersectionCheckTest() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Ray2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Ray2(new Point2(1, 1), new Vector2(-1, -1));
            var d = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var e = new Ray2(new Point2(0, -1), new Vector2(0, -1));
            var f = new Ray2(new Point2(0, 0), new Vector2(0, 3));

            Assert.IsFalse(a.Intersects(b));
            Assert.IsTrue(a.Intersects(c));

            Assert.IsFalse(a.Intersects(new Ray2(new Point2(0, 2), new Vector2(0, 1))));
            Assert.IsFalse(a.Intersects(new Ray2(new Point2(0, -2), new Vector2(0, -1))));

            Assert.IsTrue(a.Intersects(d));
            Assert.IsTrue(a.Intersects(d.GetReverse()));
            Assert.IsTrue(a.Intersects(e));
            Assert.IsTrue(a.Intersects(e.GetReverse()));
            Assert.IsTrue(a.Intersects(f));
            Assert.IsTrue(a.Intersects(f.GetReverse()));
        }

    }
}

#pragma warning restore 1591