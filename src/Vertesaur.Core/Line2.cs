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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Vertesaur.Contracts;

namespace Vertesaur {

	/// <summary>
	/// A straight line of infinite length defined by a point and a direction.
	/// </summary>
	/// <remarks>
	/// There are many ways to define a line but this class defines one by
	/// specifying a single point that lies on that line and one direction vector
	/// from that point which describes the lines orientation. This is a good
	/// compromise to reduce the amount of arithmetic required to work with the
	/// object. Define a line by specifying a single point on the line and one of
	/// the two directions in which it flies off into the vast infiniteness of
	/// space.
	/// </remarks>
	public sealed class Line2 :
		ILine2<double>,
		IEquatable<Line2>,
		IHasMbr<Mbr, double>,
		IHasDistance<Point2,double>,
		IRelatableIntersects<Point2>,
		IRelatableIntersects<Segment2>,
		IRelatableIntersects<Ray2>,
		IRelatableIntersects<Line2>,
		IHasIntersectionOperation<Segment2, IPlanarGeometry>,
		IHasIntersectionOperation<Line2, IPlanarGeometry>,
		IHasIntersectionOperation<Ray2, IPlanarGeometry>,
		IHasIntersectionOperation<Point2, IPlanarGeometry>,
		ICloneable
	{

		/// <summary>
		/// Constructs a line from slope intercept parameters.
		/// </summary>
		/// <param name="m">Slope.</param>
		/// <param name="b">Y-intercept.</param>
		/// <returns>A line.</returns>
		/// <remarks>
		/// <code>y = (m*x) + b</code>
		/// </remarks>
		public static Line2 SlopeIntercept(double m, double b) {
			Contract.Ensures(Contract.Result<Line2>() != null);
			Contract.EndContractBlock();
			return new Line2(new Point2(0, b), new Vector2(1, m));
		}

		/// <summary>
		/// Constructs a line from general form.
		/// </summary>
		/// <param name="a">The x-coefficient.</param>
		/// <param name="b">The y-coefficient.</param>
		/// <param name="c">A constant.</param>
		/// <returns>A line.</returns>
		/// <remarks>
		/// <code>(A*x) + (B*y) + C = 0</code>
		/// </remarks>
		public static Line2 General(double a, double b, double c) {
			Contract.Ensures(Contract.Result<Line2>() != null);
			Contract.EndContractBlock();
			return Standard(a, b, -c);
		}

		/// <summary>
		/// Constructs a line from standard form.
		/// </summary>
		/// <param name="a">The x-coefficient.</param>
		/// <param name="b">The y-coefficient.</param>
		/// <param name="c">A constant.</param>
		/// <returns>A line.</returns>
		/// <remarks>
		/// <code>(A*x) + (B*y) = C</code>
		/// </remarks>
		public static Line2 Standard(double a, double b, double c) {
			Contract.Ensures(Contract.Result<Line2>() != null);
			Contract.EndContractBlock();

			Point2 p;
			Vector2 v;
// ReSharper disable CompareOfFloatsByEqualityOperator
			if (0 == a) {
				v = Vector2.XUnit;
				p = 0 == b
					? Point2.Zero
					: new Point2(0, c / b);
			}
			else {
				p = new Point2(c / a, 0);
				v = 0 == b
					? Vector2.YUnit
					: new Vector2(-p.X, (c / b) - p.Y);
			}
			return new Line2(p, v);
// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// A point which lies on the line.
		/// </summary>
		/// <remarks>
		/// This is a point that intersects the line. While a line intersects an
		/// infinite number of points it is just nit practical to define more than
		/// two. This point represents any arbitrary point really while the second
		/// point that is on this line, which defines its direction and position is
		/// derived from the direction property.
		/// </remarks>
		public readonly Point2 P;

		/// <summary>
		/// The direction of the line from P.
		/// </summary>
		/// <remarks>
		/// While the defined direction goes in only one direction from the defined
		/// point the line actually goes in both the defined direction as well as the
		/// negative of the defined direction from the defined point. If it went in
		/// only one direction it would be a ray but it is a lot easier and more
		/// light-weight to define the line this way.
		/// </remarks>
		public readonly Vector2 Direction;

		/// <summary>
		/// Constructs a line with infinite length passing though <paramref name="p"/> with direction <paramref name="d"/>.
		/// </summary>
		/// <param name="p">A point on the line.</param>
		/// <param name="d">The direction of the line.</param>
		public Line2(Point2 p, Vector2 d) {
			P = p;
			Direction = d;
		}

		/// <summary>
		/// Constructs a line with infinite length passing though <paramref name="p"/> with direction <paramref name="d"/>.
		/// </summary>
		/// <param name="p">A point on the line.</param>
		/// <param name="d">The direction of the line.</param>
		public Line2(IPoint2<double> p, IVector2<double> d)
			: this(new Point2(p), new Vector2(d))
		{
			Contract.Requires(p != null);
			Contract.Requires(d != null);
			Contract.EndContractBlock();
		}

		/// <summary>
		/// Constructs a line with infinite length passing though both points <paramref name="a"/> and <paramref name="b"/>.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		public Line2(Point2 a, Point2 b)
			: this(a, b - a) { }

		/// <summary>
		/// Constructs a line with infinite length passing though both points <paramref name="a"/> and <paramref name="b"/>.
		/// </summary>
		/// <param name="a">A point.</param>
		/// <param name="b">A point.</param>
		public Line2(IPoint2<double> a, IPoint2<double> b)
			: this(new Point2(a), new Point2(b))
		{
			Contract.Requires(a != null);
			Contract.Requires(b != null);
			Contract.EndContractBlock();
		}

		/// <summary>
		/// Constructs a line with infinite length identical to the given <paramref name="line"/> which is defined by the same points and direction..
		/// </summary>
		/// <param name="line">A line.</param>
		public Line2(Line2 line) {
			if(null == line)
				throw new ArgumentNullException("line");
			Contract.EndContractBlock();

			P = line.P;
			Direction = line.Direction;
		}

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IPoint2<double> ILine2<double>.P { get { return P; } }

		/// <inheritdoc/>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IVector2<double> ILine2<double>.Direction { get { return Direction; } }

		/// <summary>
		/// Determines if the line is valid.
		/// </summary>
		public bool IsValid{
			get { return P.IsValid && Direction.IsValid && Direction != Vector2.Zero; }
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(Line2 other) {
			return ReferenceEquals(this,other) || (
				!ReferenceEquals(null, other)
				&& P.Equals(other.P)
				&& Direction.Equals(other.Direction)
			);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj) {
			return Equals(obj as Line2);
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return Direction.GetHashCode() ^ P.Y.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat('(', P, ") (", Direction, ')');
		}

		/// <summary>
		/// Calculates the length of this line.
		/// </summary>
		/// <returns>The length.</returns>
		[Pure]
		public double GetMagnitude() {
			return Vector2.Zero == Direction ? 0 : Double.PositiveInfinity;
		}

		/// <summary>
		/// Calculates the squared length of this line.
		/// </summary>
		/// <returns>The length.</returns>
		[Pure]
		public double GetMagnitudeSquared() {
			return GetMagnitude();
		}

		/// <summary>
		/// Creates a copy of this line.
		/// </summary>
		/// <returns>
		/// A new identical line.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Functions as a deep clone and a shallow clone.</remarks>
		public Line2 Clone() {
			Contract.Ensures(Contract.Result<Line2>() != null);
			return new Line2(this);
		}

		/// <inheritdoc/>
		object ICloneable.Clone() {
			return Clone();
		}

		/// <summary>
		/// Calculates a minimum bounding rectangle for this line.
		/// </summary>
		/// <returns>A minimum bounding rectangle.</returns>
		[Pure]
		public Mbr GetMbr() {
			Contract.Ensures(Contract.Result<Mbr>() != null);
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return (
				(0 == Direction.X)
				? (
					(0 == Direction.Y)
					? new Mbr(P)
					: new Mbr(P.X, Double.NegativeInfinity, P.X, Double.PositiveInfinity)
				)
				: (
					(0 == Direction.Y)
					? new Mbr(Double.NegativeInfinity, P.Y, Double.PositiveInfinity, P.Y)
					: new Mbr(Double.NegativeInfinity, Double.NegativeInfinity, Double.PositiveInfinity, Double.PositiveInfinity)
				)
			);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Calculates the distance between this line and <paramref name="p"/>
		/// </summary>
		/// <param name="p">The point to calculate distance to.</param>
		/// <returns>The distance.</returns>
		[Pure]
		public double Distance(Point2 p) {
			var v0 = p - P;
			var m = Direction.GetMagnitudeSquared();
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (0 != m) {
				var aDot = Direction.Dot(v0);
				return Math.Sqrt(v0.GetMagnitudeSquared() - ((aDot * aDot) / m));
			}
			return v0.GetMagnitude();
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Calculates the squared distance between this line and <paramref name="p"/>
		/// </summary>
		/// <param name="p">The point to calculate squared distance to.</param>
		/// <returns>The squared distance.</returns>
		[Pure]
		public double DistanceSquared(Point2 p) {
			var v0 = p - P;
			var m = Direction.GetMagnitudeSquared();
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (0 != m) {
				var aDot = Direction.Dot(v0);
				return (v0.GetMagnitudeSquared() - ((aDot * aDot) / m));
			}
			return v0.GetMagnitudeSquared();
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Determines if a point intersects this line.
		/// </summary>
		/// <param name="p">A point.</param>
		/// <returns>True when intersecting.</returns>
		[Pure]
		public bool Intersects(Point2 p) {
			var v0 = p - P;
			var m = Direction.GetMagnitudeSquared();
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (0 != m) {
				var aDot = Direction.Dot(v0);
				return (m * v0.GetMagnitudeSquared()) == (aDot * aDot);
			}
			return 0 == v0.X && 0 == v0.Y;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Determines if this line intersect another <paramref name="segment"/>.
		/// </summary>
		/// <param name="segment">A segment.</param>
		/// <returns><c>true</c> when another object intersects this object.</returns>
		public bool Intersects(Segment2 segment) {
			return !ReferenceEquals(null, segment) && segment.Intersects(this);
		}

		/// <summary>
		/// Determines if this line intersect another <paramref name="ray"/>.
		/// </summary>
		/// <param name="ray">A ray.</param>
		/// <returns><c>true</c> when another object intersects this object.</returns>
		public bool Intersects(Ray2 ray) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (ReferenceEquals(null, ray))
				return false;

			Point2 a, c;
			Vector2 d0, d1;
			a = P;
			c = ray.P;
			d0 = Direction;
			d1 = ray.Direction;
			var e = c - a;
			var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
			var magnitudeSquared0 = d0.GetMagnitudeSquared();

			if (cross * cross > d1.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon) {
				// not parallel
				var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;

				var t = ((e.X * d0.Y) - (e.Y * d0.X)) / cross;
				if (t < 0)
					return false; // not intersecting on other ray

				return true; // it must intersect at a point
			}

			// parallel
			cross = (e.X * d0.Y) - (e.Y * d0.X);
			if (cross * cross > e.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return false; // no intersection

			return true;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Determines if this line intersect another line.
		/// </summary>
		/// <param name="other">A line.</param>
		/// <returns><c>true</c> when another object intersects this object.</returns>
		public bool Intersects(Line2 other) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other) || P.Equals(other.P) && Direction.Equals(other.Direction))
				return true;

			Point2 a, c;
			Vector2 d0, d1;
			// order the lines
			var compareResult = P.CompareTo(other.P);
			if (0 < ((compareResult == 0) ? Direction.CompareTo(other.Direction) : compareResult)) {
				a = other.P;
				c = P;
				d0 = other.Direction;
				d1 = Direction;
			}
			else {
				a = P;
				c = other.P;
				d0 = Direction;
				d1 = other.Direction;
			}
			var e = c - a;
			var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
			var magnitudeSquared0 = d0.GetMagnitudeSquared();
			if (cross * cross > d1.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return true; // not parallel

			// parallel
			cross = (e.X * d0.Y) - (e.Y * d0.X);
			if (cross * cross > e.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return false;

			return true; // are same line
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <inheritdoc/>
		public IPlanarGeometry Intersection(Segment2 other) {
			return ReferenceEquals(null, other)
				? null
				: other.Intersection(this);
		}
		/// <inheritdoc/>
		public IPlanarGeometry Intersection(Line2 other) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (ReferenceEquals(null, other))
				return null;
			if (ReferenceEquals(this, other) || P.Equals(other.P) && Direction.Equals(other.Direction))
				return Clone();

			Point2 a, c;
			Vector2 d0, d1;
			// order the lines
			var compareResult = P.CompareTo(other.P);
			if (0 < ((compareResult == 0) ? Direction.CompareTo(other.Direction) : compareResult)) {
				a = other.P;
				c = P;
				d0 = other.Direction;
				d1 = Direction;
			}
			else {
				a = P;
				c = other.P;
				d0 = Direction;
				d1 = other.Direction;
			}
			var e = c - a;
			var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
			var magnitudeSquared0 = d0.GetMagnitudeSquared();
			if (cross * cross > d1.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return a + d0.GetScaled(((e.X * d1.Y) - (e.Y * d1.X)) / cross); // not parallel

			// parallel
			cross = (e.X * d0.Y) - (e.Y * d0.X);
			if (cross * cross > e.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return null; // no intersection

			return Clone(); // are same line
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <inheritdoc/>
		public IPlanarGeometry Intersection(Ray2 ray) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (ReferenceEquals(null, ray))
				return null;

			Point2 a, c;
			Vector2 d0, d1;
			a = P;
			c = ray.P;
			d0 = Direction;
			d1 = ray.Direction;
			var e = c - a;
			var cross = (d0.X * d1.Y) - (d1.X * d0.Y);
			var magnitudeSquared0 = d0.GetMagnitudeSquared();

			if (cross * cross > d1.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon) {
				// not parallel
				var s = ((e.X * d1.Y) - (e.Y * d1.X)) / cross;

				var t = ((e.X * d0.Y) - (e.Y * d0.X)) / cross;
				if (t < 0)
					return null; // not intersecting on other ray

				if (0 == s)
					return a;
				if (0 == t)
					return c;
				return a + d0.GetScaled(s); // it must intersect at a point, so find where
			}

			// parallel
			cross = (e.X * d0.Y) - (e.Y * d0.X);
			if (cross * cross > e.GetMagnitudeSquared() * magnitudeSquared0 * Double.Epsilon)
				return null; // no intersection

			return ray.Clone();
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <inheritdoc/>
		public IPlanarGeometry Intersection(Point2 other) {
			return Intersects(other) ? (IPlanarGeometry)other : null;
		}
	}

}
