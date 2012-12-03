using System;
using System.Diagnostics;
using Vertesaur.Contracts;

namespace Vertesaur.Generation
{
	public struct Vector2<TValue> : IVector2<TValue>
	{

		/// <summary>
		/// The x-coordinate of this vector.
		/// </summary>
		public readonly TValue X;
		/// <summary>
		/// The y-coordinate of this vector.
		/// </summary>
		public readonly TValue Y;

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Vector2(TValue x, TValue y) {
			X = x;
			Y = y;
		}
		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="v">The coordinate tuple to copy values from.</param>
		public Vector2(ICoordinatePair<TValue> v) {
			X = v.X;
			Y = v.Y;
		}

		/// <summary>
		/// The x-coordinate of this point.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		TValue ICoordinatePair<TValue>.X {
			get { return X; }
		}
		/// <summary>
		/// The y-coordinate of this point.
		/// </summary>
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

		/// <inheritdoc/>
		public TValue GetMagnitude() {
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public TValue GetMagnitudeSquared() {
			throw new NotImplementedException();
		}
	}
}
