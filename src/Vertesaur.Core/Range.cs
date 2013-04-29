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

// ReSharper disable CompareOfFloatsByEqualityOperator

	/// <summary>
	/// A range which encompasses all values between two values, inclusive.
	/// </summary>
	/// <remarks>
	/// A range where the high and low values are equal is a range that covers only one value.
	/// </remarks>
	public struct Range :
		IRange<double>,
		IEquatable<Range>, IEquatable<double>, IEquatable<IRange<double>>,
		ISpatiallyRelatable<Range>, ISpatiallyRelatable<double>,
		IHasDistance<Range, double>, IHasDistance<double, double>
	{

		/// <summary>
		/// Determines if two ranges are equal.
		/// </summary>
		/// <param name="a">A range.</param>
		/// <param name="b">A range.</param>
		/// <returns>True when the ranges are equal.</returns>
		[Pure] public static bool operator ==(Range a, Range b) {
			return a.Equals(b);
		}

		/// <summary>
		/// Determines if two ranges are not equal.
		/// </summary>
		/// <param name="a">A range.</param>
		/// <param name="b">A range.</param>
		/// <returns>True when the ranges are not equal.</returns>
		[Pure] public static bool operator !=(Range a, Range b) {
			return !a.Equals(b);
		}

		/// <summary>
		/// The lowest value of the range.
		/// </summary>
		public readonly double Low;
		/// <summary>
		/// The highest value of the range.
		/// </summary>
		public readonly double High;

		/// <summary>
		/// Constructs a new range encompassing only the given value <paramref name="v"/>.
		/// </summary>
		/// <param name="v">A value.</param>
		public Range(double v) {
			Low = High = v;
		}

		/// <summary>
		/// Constructs a new range from <paramref name="a"/> to <paramref name="b"/>.
		/// </summary>
		/// <param name="a">A value.</param>
		/// <param name="b">A value.</param>
		public Range(double a, double b) {
			if (b < a) {
				Low = b;
				High = a;
			}
			else {
				Low = a;
				High = b;
			}
		}

		/// <summary>
		/// Constructs a new range from the bounding values of <paramref name="range"/>.
		/// </summary>
		/// <param name="range">The bounding values to copy from.</param>
		public Range(IRange<double> range)
		{
			if(null == range) throw new ArgumentNullException("range");
			Contract.EndContractBlock();
			var a = range.Low;
			var b = range.High;
			if (b < a) {
				Low = b;
				High = a;
			}
			else {
				Low = a;
				High = b;
			}
		}

		/// <summary>
		/// Constructs a new range encompassing both <paramref name="r"/> and <paramref name="v"/>.
		/// </summary>
		/// <param name="r">A range.</param>
		/// <param name="v">A value.</param>
		private Range(Range r, double v) {
			if (v < r.Low) {
				Low = v;
				High = r.High;
			}
			else {
				Low = r.Low;
				High = v > r.High ? v : r.High;
			}
		}
		/// <summary>
		/// Constructs a new range encompassing both <paramref name="a"/> and <paramref name="b"/>.
		/// </summary>
		/// <param name="a">A range.</param>
		/// <param name="b">A range.</param>
		private Range(Range a, Range b) {
			Low = a.Low < b.Low ? a.Low : b.Low;
			High = a.High > b.High ? a.High : b.High;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double IRange<double>.Low { get { return Low; } }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		double IRange<double>.High { get { return High; } }

		/// <summary>
		/// The average of the lowest and highest value.
		/// </summary>
		public double Mid {
			[Pure] get { return (Low + High) / 2.0; }
		}

		/// <summary>
		/// Calculates the magnitude of this range.
		/// </summary>
		/// <returns>The magnitude.</returns>
		[Pure] public double GetMagnitude() {			
			return High - Low;
		}

		/// <summary>
		/// Calculates the squared magnitude of this range.
		/// </summary>
		/// <returns>The squared magnitude.</returns>
		[Pure] public double GetMagnitudeSquared() {
			var delta = High - Low;
			return delta * delta;
		}

		/// <summary>
		/// Calculates the distance between this range and <paramref name="value"/>
		/// </summary>
		/// <param name="value">The value to calculate distance to.</param>
		/// <returns>The resulting distance.</returns>
		[Pure] public double Distance(double value) {
			return (
				(value < Low)
				? (Low - value)
				: (
					(High < value)
					? (value - High)
					: 0
				)
			);
		}

		/// <summary>
		/// Calculates the squared distance between this range and <paramref name="value"/>
		/// </summary>
		/// <param name="value">The object to calculate squared distance to.</param>
		/// <returns>The resulting squared distance.</returns>
		[Pure] public double DistanceSquared(double value) {
			var d = (
				(value < Low)
				? (Low - value)
				: (
					(High < value)
					? (value - High)
					: 0
				)
			);
			return d * d;
		}

		/// <summary>
		/// Calculates the distance between this range and <paramref name="range"/>
		/// </summary>
		/// <param name="range">The object to calculate distance to.</param>
		/// <returns>The resulting distance.</returns>
		[Pure] public double Distance(Range range) {
			return (
				(range.High < Low)
				? (Low - range.High)
				: (
					(High < range.Low)
					? (range.Low - High)
					: 0
				)
			);
		}

		/// <summary>
		/// Calculates the squared distance between this range and <paramref name="range"/>
		/// </summary>
		/// <param name="range">The object to calculate squared distance to.</param>
		/// <returns>The resulting squared distance.</returns>
		[Pure] public double DistanceSquared(Range range) {
			var d = Distance(range);
			return d * d;
		}

		/// <summary>
		/// Determines if another <paramref name="value"/> intersects this range.
		/// </summary>
		/// <param name="value">A value to test intersection with.</param>
		/// <returns>True when the given <paramref name="value"/> intersects this range.</returns>
		[Pure] public bool Intersects(double value) {
			return (Low <= value) && (value <= High);
		}

		/// <summary>
		/// Determines if another <paramref name="range"/> intersects this range.
		/// </summary>
		/// <param name="range">A range to test intersection with.</param>
		/// <returns>True when the given <paramref name="range"/> intersects this range.</returns>
		[Pure] public bool Intersects(Range range) {
			return (Low <= range.High) && (range.Low <= High);
		}

		/// <summary>
		/// Determines if another range defined by two values intersects this range.
		/// </summary>
		/// <param name="a">A value defining the range to test intersection with.</param>
		/// <param name="b">A value defining the range to test intersection with.</param>
		/// <returns>True when the given range intersects this range.</returns>
		[Pure] public bool Intersects(double a, double b) {
			return (a <= b)
				? (Low <= b) && (a <= High)
				: (Low <= a) && (b <= High);
		}

		/// <summary>
		/// Determines if the given value contains this range and this range does not intersect the other exterior.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns>True if this range is within the given value.</returns>
		[Pure] public bool Within(double value) {
			return Low == value && High == value;
		}

		/// <summary>
		/// Determines if the given range contains this range and this range does not intersect the other exterior.
		/// </summary>
		/// <param name="range">The range to test.</param>
		/// <returns>True if this range is within the given range.</returns>
		[Pure] public bool Within(Range range) {
			return range.Low <= Low && range.High >= High;
		}

		/// <summary>
		/// Determines if this range and some <paramref name="value"/> share only boundaries.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <returns>True when only boundaries occupy the same space.</returns>
		[Pure] public bool Touches(double value) {
			return (Low == value || High == value) && Low != High;
		}

		/// <summary>
		/// Determines if this range and some <paramref name="range"/> share only boundaries.
		/// </summary>
		/// <param name="range">A range.</param>
		/// <returns>True when only boundaries occupy the same space.</returns>
		[Pure] public bool Touches(Range range) {
			return Low == range.High || High == range.Low;
		}

		/// <summary>
		/// Determines if this range and some <paramref name="value"/> share some space of the same dimension.
		/// </summary>
		/// <param name="value">A range.</param>
		/// <returns>True when this range and a value share space of the same dimension.</returns>
		[Pure] public bool Overlaps(double value) {
			return (Low == value && High == value)
				|| (Low < value && value < High)
			;
		}

		/// <summary>
		/// Determines if this range and some <paramref name="range"/> share some space of the same dimension.
		/// </summary>
		/// <param name="range">A range.</param>
		/// <returns>True when the ranges share space of the same dimension.</returns>
		[Pure] public bool Overlaps(Range range) {
			return (Low < range.High && range.Low < High)
				|| (Low == range.Low && High == range.High);
		}

		/// <summary>
		/// Determines if this range and some <paramref name="other"/> value share no space.
		/// </summary>
		/// <param name="other">A value.</param>
		/// <returns>True if this range and a value do not occupy any of the same space.</returns>
		[Pure] public bool Disjoint(double other) {
			return other < Low || other > High;
		}

		/// <summary>
		/// Determines if this range and some <paramref name="other"/> share no space.
		/// </summary>
		/// <param name="other">A range.</param>
		/// <returns>True if the ranges do not occupy any of the same space.</returns>
		[Pure] public bool Disjoint(Range other) {
			return other.High < Low || other.Low > High;
		}

		/// <summary>
		/// Determines if some <paramref name="other"/> value is within the interior of this range.
		/// </summary>
		/// <param name="other">A value.</param>
		/// <returns>True when this range contains another value.</returns>
		[Pure] public bool Contains(double other) {
			return Low <= other && other <= High;
		}

		/// <summary>
		/// Determines if some <paramref name="other"/> range is within the interior of this range.
		/// </summary>
		/// <param name="other">A range.</param>
		/// <returns>True when this range contains another.</returns>
		[Pure] public bool Contains(Range other) {
			return Low <= other.Low && other.High <= High;
		}

		/// <summary>
		/// Determines if this range crosses some <paramref name="other"/> value.
		/// </summary>
		/// <param name="other">The value to test.</param>
		/// <returns>True if this range crosses the <paramref name="other"/> value.</returns>
		[Pure] public bool Crosses(double other) {
			return other > Low && other < High;
		}

		/// <summary>
		/// Determines if this range crosses some <paramref name="other"/> range.
		/// </summary>
		/// <param name="other">The range to test.</param>
		/// <returns>True if this range crosses the <paramref name="other"/>.</returns>
		[Pure] public bool Crosses(Range other) {
			return (
				Low == High
				? other.Crosses(Low)
				: other.Low == other.High && Crosses(other.Low)
			);
		}

		/// <inheritdoc/>
		bool ISpatiallyEquatable<Range>.SpatiallyEqual(Range other) {
			return Equals(other);
		}

		/// <inheritdoc/>
		[Pure] public bool SpatiallyEqual(double other) {
			return Equals(other);
		}

		/// <summary>
		/// Creates a new range encompassing this range and the given <paramref name="value"/>.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <returns>A range.</returns>
		[Pure] public Range Encompass(double value) {
			return new Range(this, value);
		}

		/// <summary>
		/// Creates a new range encompassing this range and the given <paramref name="range"/>.
		/// </summary>
		/// <param name="range">A range.</param>
		/// <returns>A range.</returns>
		[Pure] public Range Encompass(Range range) {
			return new Range(this, range);
		}

		/// <summary>
		/// Gets a copy of the range at the given <paramref name="center"/> point but with the same size.
		/// </summary>
		/// <param name="center">The center location.</param>
		/// <returns>A range.</returns>
		[Pure] public Range GetCentered(double center) {
			var oldLen = High - Low;
			var high = center + (oldLen / 2.0);
			return new Range(high - oldLen, high);
		}

		/// <summary>
		/// Gets a copy of the range at the same center point but with the given size.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns>A range.</returns>
		[Pure] public Range GetResized(double size) {
			if (size == 0)
				return new Range(Mid);
			if (size < 0)
				size = -size;

			var newHigh = (Low + High + size) / 2.0;
			return new Range(newHigh, newHigh - size);
		}

		/// <summary>
		/// Gets a copy of the range at the same center point but a scaled size.
		/// </summary>
		/// <param name="scale">The scaling factor.</param>
		/// <returns>A range.</returns>
		[Pure] public Range GetScaled(double scale) {
			return GetResized((High - Low) * scale);
		}

		/// <summary>
		/// Determines if this range is equal to some <paramref name="other"/> range.
		/// </summary>
		/// <param name="other">A range to compare with.</param>
		/// <returns>
		/// <c>true</c> if the current range is equal to the <paramref name="other"/> range; otherwise, <c>false</c>.
		/// </returns>
		[Pure] public bool Equals(Range other) {
			return Low == other.Low
				&& High == other.High;
		}

		/// <summary>
		/// Determines if this range is equal to some <paramref name="other"/> range.
		/// </summary>
		/// <param name="other">A range to compare with.</param>
		/// <returns>
		/// <c>true</c> if the current range is equal to the <paramref name="other"/> range; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public bool Equals(IRange<double> other) {
			return !ReferenceEquals(null,other)
				&& Low == other.Low
				&& High == other.High;
		}

		/// <summary>
		/// Indicates whether the current range is equal to another value.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the current range is equal to the <paramref name="other"/> value; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="other">A value.</param>
		[Pure] public bool Equals(double other) {
			return Low == other && High == other;
		}

		/// <summary>
		/// Indicates whether this range and a specified object are equal.
		/// </summary>
		/// <returns>
		/// <c>true</c> if <paramref name="obj"/> and this range are the same type and represent the same value; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="obj">Another object to compare to.</param>
		/// <filterpriority>2</filterpriority>
		[Pure] public override bool Equals(object obj) {
			return null != obj && (
				(obj is Range) ? Equals((Range)obj)
				:
				(obj is IRange<double>) ? Equals((IRange<double>)obj)
				:
				(obj is double && Equals((double)obj))
			);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		[Pure] public override int GetHashCode() {
			return Low.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		[Pure] public override string ToString() {
			Contract.Ensures(Contract.Result<string>() != null);
			return String.Concat(Low, ':', High);
		}

	}

// ReSharper restore CompareOfFloatsByEqualityOperator

}
