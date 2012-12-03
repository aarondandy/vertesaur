using System;
using System.Diagnostics;
using Vertesaur.Contracts;

namespace Vertesaur.Generation
{
	public struct Point2<TValue> : IPoint2<TValue>
	{
		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		public readonly TValue X;
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
		public readonly TValue Y;

		/// <summary>
		/// Creates a point with the given <paramref name="x"/> and <paramref name="y"/> coordinates.
		/// </summary>
		/// <param name="x">A coordinate.</param>
		/// <param name="y">A coordinate.</param>
		public Point2(TValue x, TValue y) {
			X = x;
			Y = y;
		}
		/// <summary>
		/// Creates a point with the same coordinates as the given <paramref name="point"/>.
		/// </summary>
		/// <param name="point">A coordinate pair.</param>
		public Point2(ICoordinatePair<TValue> point) {
			X = point.X;
			Y = point.Y;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.X {
			get { return X; }
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.Y {
			get { return Y; }
		}

		/// <inheritdoc/>
		public override int GetHashCode() {
			return X.GetHashCode();
		}

		/// <inheritdoc/>
		public override string ToString() {
			return String.Concat(X, ' ', Y);
		}

	}
}
