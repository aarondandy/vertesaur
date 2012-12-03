// ===============================================================================
//
// Copyright (c) 2012 Aaron Dandy 
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
using System.Diagnostics.Contracts;

namespace Vertesaur.Periodic
{
	/// <summary>
	/// Provides operations that can be performed on values and ranges that have a periodic nature (wrap around).
	/// </summary>
	public class PeriodicOperations
	{

		private readonly double _periodStart;
		private readonly double _periodEnd;
		private readonly double _periodLength;

		/// <summary>
		/// Constructs a new periodic operator from the given start and with the given length.
		/// </summary>
		/// <param name="periodStart">The start value of the period.</param>
		/// <param name="periodLength">The length of the range.</param>
		public PeriodicOperations(double periodStart, double periodLength) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if(periodLength == 0)
				throw new ArgumentException("The period length must be a non-zero value.", "periodLength");
			if (periodLength <= 0)
				throw new NotSupportedException("Only periods with a positive length are supported.");
			Contract.EndContractBlock();

			_periodStart = periodStart;
			_periodLength = periodLength;
			_periodEnd = _periodStart + _periodLength;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// The starting value of the period.
		/// </summary>
		public double PeriodStart { get { return _periodStart; } }
		/// <summary>
		/// The ending value of the period.
		/// </summary>
		public double PeriodEnd { get { return _periodEnd; } }
		/// <summary>
		/// The length of the period from the start.
		/// </summary>
		public double PeriodLength { get { return _periodLength; } }

		/// <summary>
		/// Calculates the correct value within the periodic range by applying
		/// wrapping logic, excluding the last value in the period range.
		/// </summary>
		/// <param name="value">The value to correct.</param>
		/// <returns>The corrected value within the defined periodic range.</returns>
		/// <remarks>
		/// This method will not wrap the last value in a range to the first value
		/// even though this method relies on division remainder logic. This means
		/// that for a range -180 to +180 the value +180 will be fixed to +180 while
		/// the value 540 may be fixed to -180.
		/// 
		/// Fixing a value that is equal to the end boundary so that is becomes the
		/// start boundary may cause serious grief for some users. For example some
		/// longitude values range from -180 to +180 inclusive even though -180 and
		/// +180 are the same locations. In this case the +180 should be preserved
		/// rather than converted to -180. This is simply because sometimes that is
		/// how our data looks. Also with the case of radians the value of
		/// <c>System.Math.PI * 2</c> is actually less than the actual value of 2pi so
		/// fixing that to 0 may contribute to large accuracy problems or even broken
		/// results. If a value is for example 4pi (for a range 0 to 2pi) or 540 (for
		/// a range -180 to +180) it really should not matter much if it is fixed to
		/// one end of the range or the other as that value is definitely outside of
		/// the expected range and must be adjusted regardless.
		/// </remarks>
		[Pure]
		public double FixExcludingEnd(double value) {
			Contract.Ensures(Contract.Result<double>() >= _periodStart);
			Contract.Ensures(Contract.Result<double>() <= _periodEnd);
			Contract.EndContractBlock();

			// NOTE: if the value is equal to the _periodEnd value it is best to leave it alone in some cases
			if(value < _periodStart || value > _periodEnd) {
				var r = (value - _periodStart) % _periodLength;
				return (r >= 0 ? _periodStart : _periodEnd) + r;
			}
			return value;
		}

		/// <summary>
		/// Calculates the correct value within the periodic range by applying
		/// wrapping logic.
		/// </summary>
		/// <param name="value">The value to correct.</param>
		/// <returns>The corrected value within the defined periodic range.</returns>
		/// <remarks>A value equal to the period end value will be converted to the period start value.</remarks>
		[Pure]
		public double Fix(double value) {
			// ReSharper disable CompareOfFloatsByEqualityOperator
			Contract.Ensures(Contract.Result<double>() >= _periodStart);
			Contract.Ensures(Contract.Result<double>() < _periodEnd);
			Contract.EndContractBlock();

			if (value >= _periodStart && value < _periodEnd)
				return value;
			if (value == _periodEnd)
				return _periodStart;

			var r = (value - _periodStart) % _periodLength;
			return (r >= 0 ? _periodStart : _periodEnd) + r;
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}

		/// <summary>
		/// Calculates the magnitude of a range after fixing the start and end values.
		/// </summary>
		/// <param name="rangeStart">The start value of a range.</param>
		/// <param name="rangeEnd">The end value of a range.</param>
		/// <returns>The calculated magnitude.</returns>
		[Pure]
		public double Magnitude(double rangeStart, double rangeEnd) {
			Contract.Ensures(Contract.Result<double>() >= 0);
			Contract.EndContractBlock();

			rangeStart = Fix(rangeStart);
			rangeEnd = FixExcludingEnd(rangeEnd);

			if (rangeStart <= rangeEnd)
				return rangeEnd - rangeStart;
			return (_periodEnd - rangeStart) + (rangeEnd - _periodStart);
		}

		/// <summary>
		/// Calculates the distance between two values.
		/// </summary>
		/// <param name="a">The first value.</param>
		/// <param name="b">The second value.</param>
		/// <returns>The minimum distance between the two values.</returns>
		[Pure]
		public double Distance(double a, double b) {
			Contract.Ensures(Contract.Result<double>() >= 0);
			Contract.EndContractBlock();

			a = Fix(a);
			b = Fix(b);
			double wrapDistance;
			if (a > b) {
				// NOTE: using wrapDistance as a temporary swap
				wrapDistance = a;
				a = b;
				b = wrapDistance;
			}
			wrapDistance = (_periodEnd - b) + (a - _periodStart);
			return Math.Min(b - a, wrapDistance);
		}

		/// <summary>
		/// Calculates the distance between a range and a value.
		/// </summary>
		/// <param name="rangeStart">The start value of a range.</param>
		/// <param name="rangeEnd">The end value of a range.</param>
		/// <param name="singleValue">The value to calculate distance to.</param>
		/// <returns>The distance from the range to the value.</returns>
		[Pure]
		public double Distance(double rangeStart, double rangeEnd, double singleValue) {
			Contract.Ensures(Contract.Result<double>() >= 0);
			Contract.EndContractBlock();

			rangeStart = Fix(rangeStart);
			rangeEnd = FixExcludingEnd(rangeEnd);
			singleValue = Fix(singleValue);

			if (rangeStart <= rangeEnd) {
				if (rangeStart > singleValue)
					return rangeStart - singleValue;
				if (singleValue > rangeEnd)
					return singleValue - rangeEnd;
				return 0;
			}
			if (rangeStart > singleValue && singleValue > rangeEnd)
				return Math.Min(singleValue - rangeEnd, rangeStart - singleValue);
			return 0;
		}

		/// <summary>
		/// Determines if a value intersects a range.
		/// </summary>
		/// <param name="rangeStart">The start value of a range.</param>
		/// <param name="rangeEnd">The end value of a range.</param>
		/// <param name="testValue">The single value to test intersection with.</param>
		/// <returns>True when the range intersects the value.</returns>
		[Pure]
		public bool Intersects(double rangeStart, double rangeEnd, double testValue) {
			rangeStart = Fix(rangeStart);
			rangeEnd = FixExcludingEnd(rangeEnd);
			testValue = Fix(testValue);

			return rangeStart <= rangeEnd
				? rangeStart <= testValue && testValue <= rangeEnd
				: rangeStart <= testValue || testValue <= rangeEnd;
		}

		/// <summary>
		/// Determines if two ranges intersect.
		/// </summary>
		/// <param name="startA">The start of the first range.</param>
		/// <param name="endA">The end of the first range.</param>
		/// <param name="startB">The start of the second range.</param>
		/// <param name="endB">The end of the second range.</param>
		/// <returns>True when the ranges intersect.</returns>
		[Pure]
		public bool Intersects(double startA, double endA, double startB, double endB) {
			startA = Fix(startA);
			endA = FixExcludingEnd(endA);
			startB = Fix(startB);
			endB = FixExcludingEnd(endB);

			if(startA <= endA) {
				//TODO: this should be a single return
				// both regular
				if(startB <= endB)
					return startA <= endB && startB <= endA;
				// only A is regular
				return startA <= endB || endA >= startB;
			}
			//TODO: this should be a single return
			//TODO: this should be a single path
			// only B is regular
			if (startB <= endB)
				return startB <= endA || endB >= startA;
			// none of them are regular
			return true;
		}

		/// <summary>
		/// Determines if a value is contained by a range.
		/// </summary>
		/// <param name="rangeStart">The start value of a range.</param>
		/// <param name="rangeEnd">The end value of a range.</param>
		/// <param name="testValue">The single value to test.</param>
		/// <returns>True when the range contains the value.</returns>
		[Pure]
		public bool Contains(double rangeStart, double rangeEnd, double testValue) {
			return Intersects(rangeStart, rangeEnd, testValue);
		}

		/// <summary>
		/// Determines if one range contains another.
		/// </summary>
		/// <param name="startA">The start of the first range that may contain the second.</param>
		/// <param name="endA">The end of the first range that may contain the second.</param>
		/// <param name="startB">The start of the second range that may be contained by the first.</param>
		/// <param name="endB">The end of the second range that may be contained by the first.</param>
		/// <returns>True when the first range contains the second.</returns>
		[Pure]
		public bool Contains(double startA, double endA, double startB, double endB) {
			startA = Fix(startA);
			endA = FixExcludingEnd(endA);
			startB = Fix(startB);
			endB = FixExcludingEnd(endB);

			if (startA <= endA) {
				//TODO: this should be a single return
				//TODO: this should be a single path
				// both regular
				if (startB <= endB)
					return startB >= startA && endB <= endA;
				// only A is regular
				return false;
			}
			//TODO: this should be a single return
			// only B is regular
			if (startB <= endB)
				return endB <= endA || startB >= startA;
			// none of them are regular
			return endB <= endA && startB >= startA;
		}

		/// <summary>
		/// Determines the midpoint of a range.
		/// </summary>
		/// <param name="rangeStart">The start of the range.</param>
		/// <param name="rangeEnd">The end of the range.</param>
		/// <returns>The midpoint value of the range.</returns>
		[Pure]
		public double CalculateMidpoint(double rangeStart, double rangeEnd) {
			rangeStart = Fix(rangeStart);
			rangeEnd = FixExcludingEnd(rangeEnd);

			return rangeStart <= rangeEnd
				? (rangeStart + rangeEnd) / 2.0
				: Fix((Magnitude(rangeStart, rangeEnd) / 2.0) + rangeStart);
		}

	}
}
