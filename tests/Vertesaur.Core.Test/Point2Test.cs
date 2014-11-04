﻿using NUnit.Framework;

#pragma warning disable 1591

namespace Vertesaur.Core.Test
{

    /// <summary>
    /// Test cases for the 2D point.
    /// </summary>
    [TestFixture]
    public class Point2Test
    {

        [Test]
        public void DefaultConstructorTest() {
            var p = new Point2();
            Assert.AreEqual(0, p.X);
            Assert.AreEqual(0, p.Y);
        }

        [Test]
        public void VectorCastTest() {
            var a = new Point2(2, 3);
            Vector2 b = a;
            Point2 c = b;
            Assert.AreEqual(a.X, c.X);
            Assert.AreEqual(a.Y, c.Y);
        }

        [Test]
        public void ComponentConstructorTest() {
            var p = new Point2(1, 2);
            Assert.AreEqual(1, p.X);
            Assert.AreEqual(2, p.Y);
        }

        [Test]
        public void DistancePoint2Test() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 5);
            Assert.AreEqual(
                System.Math.Sqrt(13),
                a.Distance(b)
            );
            Assert.AreEqual(
                System.Math.Sqrt(13),
                b.Distance(a)
            );
        }

        [Test]
        public void DistanceSquaredPoint2Test() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 5);
            Assert.AreEqual(
                13,
                a.DistanceSquared(b)
            );
            Assert.AreEqual(
                13,
                b.DistanceSquared(a)
            );
        }

        [Test]
        public void CompareToTest() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
            Assert.AreNotEqual(a, b);
            b = new Point2(1, 1);
            Assert.IsTrue(b.CompareTo(a) < 0);
            Assert.IsTrue(a.CompareTo(b) > 0);
            Assert.AreNotEqual(a, b);
            b = new Point2(1, 2);
            Assert.AreEqual(0, a.CompareTo(b));
        }

        [Test]
        public void EqualsOpTest() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            Assert.IsFalse(a == b);
            Assert.IsFalse(a == c);
            Assert.IsFalse(b == a);
            Assert.IsTrue(b == c);
            Assert.IsFalse(c == a);
            Assert.IsTrue(c == b);
        }

        [Test]
        public void NotEqualsOpTest() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            Assert.IsTrue(a != b);
            Assert.IsTrue(a != c);
            Assert.IsTrue(b != a);
            Assert.IsFalse(b != c);
            Assert.IsTrue(c != a);
            Assert.IsFalse(c != b);
        }

        [Test]
        public void EqualsPoint2Test() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(b.Equals(a));
            Assert.IsTrue(b.Equals(c));
            Assert.IsFalse(c.Equals(a));
            Assert.IsTrue(c.Equals(b));
        }

        [Test]
        public void EqualsICoordinatePairTest() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            ICoordinatePair<double> nil = null;
            Assert.IsFalse(a.Equals((ICoordinatePair<double>)b));
            Assert.IsFalse(a.Equals((ICoordinatePair<double>)c));
            Assert.IsFalse(b.Equals((ICoordinatePair<double>)a));
            Assert.IsTrue(b.Equals((ICoordinatePair<double>)c));
            Assert.IsFalse(c.Equals((ICoordinatePair<double>)a));
            Assert.IsTrue(c.Equals((ICoordinatePair<double>)b));
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.IsFalse(a.Equals(nil));
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        [Test]
        public void EqualsObjectTest() {
            var a = new Point2(1, 2);
            var b = new Point2(3, 4);
            var c = new Point2(3, 4);
            Assert.IsFalse(a.Equals((object)(new Vector2(b))));
            Assert.IsFalse(a.Equals((object)(new Vector2(c))));
            Assert.IsFalse(((object)b).Equals(a));
            Assert.IsTrue(((object)b).Equals(new Vector2(c)));
            Assert.IsFalse(c.Equals((object)a));
            Assert.IsTrue(c.Equals((object)b));
        }

        [Test]
        public void IntersectsPointTest() {
            IRelatableIntersects<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Intersects(new Point2(3, 4)));
            Assert.IsTrue(a.Intersects(new Point2(1, 2)));
            Assert.IsFalse(a.Intersects(new Point2(1, 3)));
        }

        [Test]
        public void DisjointPointTest() {
            IRelatableDisjoint<Point2> a = new Point2(1, 2);
            Assert.IsTrue(a.Disjoint(new Point2(3, 4)));
            Assert.IsFalse(a.Disjoint(new Point2(1, 2)));
            Assert.IsTrue(a.Disjoint(new Point2(1, 3)));
        }

        [Test]
        public void TouchesPointTest() {
            IRelatableTouches<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Touches(new Point2(3, 4)));
            Assert.IsFalse(a.Touches(new Point2(1, 2)));
            Assert.IsFalse(a.Touches(new Point2(1, 3)));
        }

        [Test]
        public void CrossesPointTest() {
            IRelatableCrosses<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Crosses(new Point2(3, 4)));
            Assert.IsFalse(a.Crosses(new Point2(1, 2)));
            Assert.IsFalse(a.Crosses(new Point2(1, 3)));
        }

        [Test]
        public void WithinPointTest() {
            IRelatableWithin<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Within(new Point2(3, 4)));
            Assert.IsTrue(a.Within(new Point2(1, 2)));
            Assert.IsFalse(a.Within(new Point2(1, 3)));
        }

        [Test]
        public void ContainsPointTest() {
            IRelatableContains<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Contains(new Point2(3, 4)));
            Assert.IsTrue(a.Contains(new Point2(1, 2)));
            Assert.IsFalse(a.Contains(new Point2(1, 3)));
        }

        [Test]
        public void OverlapsPointTest() {
            IRelatableOverlaps<Point2> a = new Point2(1, 2);
            Assert.IsFalse(a.Overlaps(new Point2(3, 4)));
            Assert.IsFalse(a.Overlaps(new Point2(1, 2)));
            Assert.IsFalse(a.Overlaps(new Point2(1, 3)));
        }

        [Test]
        public void AddTest() {
            var a = new Point2(1, 3);
            var v = new Vector2(2, 4);
            var b = a.Add(v);
            Assert.AreEqual(a.X + v.X, b.X);
            Assert.AreEqual(a.Y + v.Y, b.Y);
        }

        [Test]
        public void DifferenceTest() {
            var a = new Point2(1, 3);
            var b = new Point2(2, 4);
            var d = a.Difference(b);
            Assert.AreEqual(a.X - b.X, d.X);
            Assert.AreEqual(a.Y - b.Y, d.Y);
        }

    }
}

#pragma warning restore 1591