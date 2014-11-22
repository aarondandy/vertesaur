using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Test
{
    public class Segment2Facts
    {
        [Fact]
        public void constructor_elements() {
            var s = new Segment2(new Point2(0, 0), new Point2(2, 3));
            Assert.Equal(new Point2(0, 0), s.A);
            Assert.Equal(new Point2(2, 3), s.B);
        }

        [Fact]
        public void constructor_copy() {
            var i = new Segment2(new Point2(1, 2), new Point2(4, 5));

            var j = new Segment2(i);

            Assert.Equal(new Point2(1, 2), j.A);
            Assert.Equal(new Point2(4, 5), j.B);
        }

        [Fact]
        public void direction() {
            var u = new Segment2(new Point2(2, 1), new Point2(-1, 3));
            Assert.Equal(new Vector2(-3, 2), u.Direction);
        }

        [Fact]
        public void equal() {
            var z = new Segment2(new Point2(1, 3), new Point2(2, 5));
            var q = new Segment2(new Point2(1, 3), new Point2(2, 5));
            Assert.Equal(z, q);
            Assert.Equal(q, z);
            q = new Segment2(new Point2(0, 2), new Point2(5, 6));
            Assert.NotEqual(z, q);
            Assert.NotEqual(q, z);
        }

        [Fact]
        public void distance_point() {
            var s = new Segment2(Point2.Zero, new Point2(4, 2));

            Assert.Equal(3, s.Distance(new Point2(-3, 0)));
            Assert.Equal(Math.Sqrt(18), s.Distance(new Point2(7, 5)));
            Assert.Equal(Math.Sqrt(5), s.Distance(new Point2(1, 3)));
        }

        [Fact]
        public void distance_squared_point() {
            var s = new Segment2(new Point2(), new Point2(4, 2));

            Assert.Equal(9, s.DistanceSquared(new Point2(-3, 0)));
            Assert.Equal(18, s.DistanceSquared(new Point2(7, 5)));
            Assert.Equal(5, s.DistanceSquared(new Point2(1, 3)));
        }

        [Fact]
        public void intersect_point_test() {
            var s = new Segment2(new Point2(), new Point2(4, 2));

            Assert.False(s.Intersects(new Point2(-3, 0)));
            Assert.False(s.Intersects(new Point2(7, 5)));
            Assert.False(s.Intersects(new Point2(1, 3)));
            Assert.True(s.Intersects(new Point2()));
            Assert.True(s.Intersects(new Point2(4, 2)));
            Assert.True(s.Intersects(new Point2(2, 1)));
        }

        [Fact]
        public void magnitude() {
            var s = new Segment2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(Math.Sqrt(9 + 16), s.GetMagnitude());
        }

        [Fact]
        public void magnitude_squared() {
            var s = new Segment2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(9 + 16, s.GetMagnitudeSquared());
        }

        [Fact]
        public void centroid() {
            var s = new Segment2(new Point2(1, 2), new Vector2(3, 4));

            Assert.Equal(new Point2(1 + (3.0 / 2.0), 2 + (4 / 2)), s.GetCentroid());
        }

        [Fact]
        public void distance_point_reliability_test() {
            var a = new Point2(BitConverter.Int64BitsToDouble(4604914652251415285), BitConverter.Int64BitsToDouble(605372236616115020));
            var b = new Point2(BitConverter.Int64BitsToDouble(4597194308594501564), BitConverter.Int64BitsToDouble(595495711103841793));
            var c = new Point2(BitConverter.Int64BitsToDouble(4600243336160129506), BitConverter.Int64BitsToDouble(606560426639725722));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598185498284921522), BitConverter.Int64BitsToDouble(585718899494321686));
            b = new Point2(BitConverter.Int64BitsToDouble(4604929952535543498), BitConverter.Int64BitsToDouble(604109661965789872));
            c = new Point2(BitConverter.Int64BitsToDouble(4601225085229312940), BitConverter.Int64BitsToDouble(605721095959846367));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602678972841459784), BitConverter.Int64BitsToDouble(602930345403337094));
            b = new Point2(BitConverter.Int64BitsToDouble(4604035258510844761), BitConverter.Int64BitsToDouble(606837598738812103));
            c = new Point2(BitConverter.Int64BitsToDouble(4606080449305389959), BitConverter.Int64BitsToDouble(604233694275374157));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4600442574156602443), BitConverter.Int64BitsToDouble(598207236037163581));
            b = new Point2(BitConverter.Int64BitsToDouble(4594502304813934338), BitConverter.Int64BitsToDouble(606104723491674031));
            c = new Point2(BitConverter.Int64BitsToDouble(4604774769151108227), BitConverter.Int64BitsToDouble(602799617759828923));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606663833178362035), BitConverter.Int64BitsToDouble(603250972966523070));
            b = new Point2(BitConverter.Int64BitsToDouble(4601480025126435688), BitConverter.Int64BitsToDouble(596774618449448018));
            c = new Point2(BitConverter.Int64BitsToDouble(4603452121619463841), BitConverter.Int64BitsToDouble(603812641245761129));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602900123144524429), BitConverter.Int64BitsToDouble(598130168765066766));
            b = new Point2(BitConverter.Int64BitsToDouble(4602249248711176862), BitConverter.Int64BitsToDouble(607165635300942201));
            c = new Point2(BitConverter.Int64BitsToDouble(4599216937047123680), BitConverter.Int64BitsToDouble(606879582652324133));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598496169291827150), BitConverter.Int64BitsToDouble(593785135637253756));
            b = new Point2(BitConverter.Int64BitsToDouble(4605980578044933597), BitConverter.Int64BitsToDouble(604432066785604900));
            c = new Point2(BitConverter.Int64BitsToDouble(4586666280293906781), BitConverter.Int64BitsToDouble(605343492497141507));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4598846179330278520), BitConverter.Int64BitsToDouble(606199123660047206));
            b = new Point2(BitConverter.Int64BitsToDouble(4599493010557328652), BitConverter.Int64BitsToDouble(585727097513151743));
            c = new Point2(BitConverter.Int64BitsToDouble(4603015160794342349), BitConverter.Int64BitsToDouble(605551305890752774));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4592419707721030345), BitConverter.Int64BitsToDouble(4606370670874082160));
            b = new Point2(BitConverter.Int64BitsToDouble(4602826327758802001), BitConverter.Int64BitsToDouble(4600543439500661700));
            c = new Point2(BitConverter.Int64BitsToDouble(4602463323412133892), BitConverter.Int64BitsToDouble(4601203408898458687));

            Assert.Equal(
                new Segment2(a, b).Distance(c),
                new Segment2(b, a).Distance(c)
            );
        }

        [Fact]
        public void intersection_point_reliability_test() {

            var a = new Point2(BitConverter.Int64BitsToDouble(4604031280725203997), BitConverter.Int64BitsToDouble(603767128270027679));
            var b = new Point2(BitConverter.Int64BitsToDouble(4599461212347507509), BitConverter.Int64BitsToDouble(592115755133497830));
            var c = new Point2(BitConverter.Int64BitsToDouble(4601157578736873697), BitConverter.Int64BitsToDouble(598204225474802883));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4601380850531485442), BitConverter.Int64BitsToDouble(589188805350663631));
            b = new Point2(BitConverter.Int64BitsToDouble(4586336351710601530), BitConverter.Int64BitsToDouble(606703828463294835));
            c = new Point2(BitConverter.Int64BitsToDouble(4587218570205865465), BitConverter.Int64BitsToDouble(606577244964107058));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606030312587645013), BitConverter.Int64BitsToDouble(594736814910574996));
            b = new Point2(BitConverter.Int64BitsToDouble(4594874457891834867), BitConverter.Int64BitsToDouble(598893481231194755));
            c = new Point2(BitConverter.Int64BitsToDouble(4601180880956119876), BitConverter.Int64BitsToDouble(597846328687307200));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4607123179444343871), BitConverter.Int64BitsToDouble(597539282599246653));
            b = new Point2(BitConverter.Int64BitsToDouble(4587595002547589811), BitConverter.Int64BitsToDouble(605930046567684595));
            c = new Point2(BitConverter.Int64BitsToDouble(4599436376715884040), BitConverter.Int64BitsToDouble(604316376715993779));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4594904703957517048), BitConverter.Int64BitsToDouble(604506264379915290));
            b = new Point2(BitConverter.Int64BitsToDouble(4606426535291429134), BitConverter.Int64BitsToDouble(590923593376627038));
            c = new Point2(BitConverter.Int64BitsToDouble(4599222167419711587), BitConverter.Int64BitsToDouble(603414706481609362));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4607087178592244418), BitConverter.Int64BitsToDouble(602721354135915871));
            b = new Point2(BitConverter.Int64BitsToDouble(4601476038364984360), BitConverter.Int64BitsToDouble(590931826761828440));
            c = new Point2(BitConverter.Int64BitsToDouble(4606962274043588495), BitConverter.Int64BitsToDouble(602576263058507124));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4602350326177966714), BitConverter.Int64BitsToDouble(604956798367051679));
            b = new Point2(BitConverter.Int64BitsToDouble(4605541086351611503), BitConverter.Int64BitsToDouble(572436839363321385));
            c = new Point2(BitConverter.Int64BitsToDouble(4604583547719485614), BitConverter.Int64BitsToDouble(597873959906434051));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4606427041690722810), BitConverter.Int64BitsToDouble(603384191280022288));
            b = new Point2(BitConverter.Int64BitsToDouble(4603168764381592373), BitConverter.Int64BitsToDouble(606547930499481055));
            c = new Point2(BitConverter.Int64BitsToDouble(4603755297686136346), BitConverter.Int64BitsToDouble(605978415323378158));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

            a = new Point2(BitConverter.Int64BitsToDouble(4592419707721030345), BitConverter.Int64BitsToDouble(4606370670874082160));
            b = new Point2(BitConverter.Int64BitsToDouble(4602826327758802001), BitConverter.Int64BitsToDouble(4600543439500661700));
            c = new Point2(BitConverter.Int64BitsToDouble(4602463323412133892), BitConverter.Int64BitsToDouble(4601203408898458687));

            Assert.Equal(
                new Segment2(a, b).Intersects(c),
                new Segment2(b, a).Intersects(c)
            );

        }

        [Fact]
        public void intersection_cross() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var res = a.Intersection(b);
            Assert.Equal(new Point2(.5, .5), res);
            res = b.Intersection(a);
            Assert.Equal(new Point2(.5, .5), res);
        }

        [Fact]
        public void intersection_cross_edge() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1.5, .5));
            var res = a.Intersection(b);
            Assert.Equal(new Point2(1, 1), res);
            res = b.Intersection(a);
            Assert.Equal(new Point2(1, 1), res);
        }

        [Fact]
        public void intersection_cross_ends() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(0.5, 1.5), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.Equal(new Point2(1, 1), res);
            res = b.Intersection(a);
            Assert.Equal(new Point2(1, 1), res);
        }

        private static void AreSpatiallyEqual(object expected, object result) {
            if (result is ISpatiallyEquatable<Segment2> && expected is Segment2) {
                Assert.True(result is ISpatiallyEquatable<Segment2>);
                Assert.True(
                    ((ISpatiallyEquatable<Segment2>)result)
                    .SpatiallyEqual(expected as Segment2)
                );
            }
            else {
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void intersection_same() {
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

        [Fact]
        public void intersection_overlap() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(.5, .5), new Point2(2, 2));
            var exp = new Segment2(new Point2(.5, .5), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.Equal(exp, res);
            res = b.Intersection(a);
            Assert.Equal(exp, res);
        }

        [Fact]
        public void intersection_within() {
            var a = new Segment2(new Point2(0, 0), new Point2(1, 1));
            var b = new Segment2(new Point2(-.5, -.5), new Point2(2, 2));
            var res = a.Intersection(b);
            Assert.Equal(a, res);
            res = b.Intersection(a);
            Assert.Equal(a, res);
        }

        [Fact]
        public void parallel_not_intersecting() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(1, 1), new Point2(2, 0));
            var res = a.Intersection(b);
            Assert.Null(res);
            res = b.Intersection(a);
            Assert.Null(res);
        }

        [Fact]
        public void perpendicular_not_intersecting() {
            var a = new Segment2(new Point2(0, 1), new Point2(1, 0));
            var b = new Segment2(new Point2(2, 2), new Point2(1, 1));
            var res = a.Intersection(b);
            Assert.Null(res);
            res = b.Intersection(a);
            Assert.Null(res);
        }

        [Fact]
        public void line_intersection_result() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Line2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Line2(new Point2(1, 1), new Vector2(0, 1));
            var d = new Line2(new Point2(0, 0), new Vector2(0, 1));

            Assert.Equal(new Point2(0, 0), a.Intersection(b));
            Assert.Equal(null, a.Intersection(c));
            Assert.Equal(a, a.Intersection(d));
        }

        [Fact]
        public void line_intersects_test() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Line2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Line2(new Point2(1, 1), new Vector2(0, 1));
            var d = new Line2(new Point2(0, 0), new Vector2(0, 1));
            var e = new Line2(new Point2(0, -2), new Vector2(1, 1));

            Assert.True(a.Intersects(b));
            Assert.False(a.Intersects(c));
            Assert.True(a.Intersects(d));
            Assert.False(a.Intersects(e));
        }

        [Fact]
        public void raw_intersection_result() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Ray2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Ray2(new Point2(1, 1), new Vector2(-1, -1));
            var d = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var e = new Ray2(new Point2(0, -1), new Vector2(0, -1));
            var f = new Ray2(new Point2(0, 0), new Vector2(0, 3));

            Assert.Equal(null, a.Intersection(b));
            Assert.Equal(new Point2(0, 0), a.Intersection(c));
            Assert.Equal(null, a.Intersection(new Ray2(new Point2(0, 2), new Vector2(0, 1))));
            Assert.Equal(null, a.Intersection(new Ray2(new Point2(0, -2), new Vector2(0, -1))));
            Assert.Equal(new Point2(0, 1), a.Intersection(d));
            Assert.Equal(a, a.Intersection(d.GetReverse()));
            Assert.Equal(new Point2(0, -1), a.Intersection(e));
            Assert.Equal(a, a.Intersection(e.GetReverse()));
            Assert.Equal(new Segment2(f.P, a.B), a.Intersection(f));
            Assert.Equal(new Segment2(a.A, f.P), a.Intersection(f.GetReverse()));
        }

        [Fact]
        public void ray_intersects_test() {
            var a = new Segment2(new Point2(0, -1), new Point2(0, 1));
            var b = new Ray2(new Point2(1, 1), new Vector2(1, 1));
            var c = new Ray2(new Point2(1, 1), new Vector2(-1, -1));
            var d = new Ray2(new Point2(0, 1), new Vector2(0, 1));
            var e = new Ray2(new Point2(0, -1), new Vector2(0, -1));
            var f = new Ray2(new Point2(0, 0), new Vector2(0, 3));

            Assert.False(a.Intersects(b));
            Assert.True(a.Intersects(c));
            Assert.False(a.Intersects(new Ray2(new Point2(0, 2), new Vector2(0, 1))));
            Assert.False(a.Intersects(new Ray2(new Point2(0, -2), new Vector2(0, -1))));
            Assert.True(a.Intersects(d));
            Assert.True(a.Intersects(d.GetReverse()));
            Assert.True(a.Intersects(e));
            Assert.True(a.Intersects(e.GetReverse()));
            Assert.True(a.Intersects(f));
            Assert.True(a.Intersects(f.GetReverse()));
        }

        [Fact]
        public void constructor_argument_type_conversion_tests() {
            var zeroPoint = new Point2(0, 0);
            var zeroVector = new Vector2(0, 0);
            var fivePoint = new Point2(5, 0);
            var fiveVector = new Vector2(5, 0);
            var tenPoint = new Point2(10, 0);
            var tenVector = new Vector2(10, 0);

            Assert.Equal(5, new Segment2(zeroVector, fivePoint).GetMagnitude());
            Assert.Equal(5, new Segment2(zeroVector, fiveVector).GetMagnitude());
            Assert.Equal(5, new Segment2(fiveVector, tenPoint).GetMagnitude());
            Assert.Equal(10, new Segment2(fiveVector, tenVector).GetMagnitude());
            Assert.Equal(0, new Segment2(tenVector, zeroVector).GetMagnitude());
            Assert.Equal(10, new Segment2(tenVector, zeroPoint).GetMagnitude());
        }

    }
}

#pragma warning restore 1591