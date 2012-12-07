using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur.Generation
{
	/// <summary>
	/// A vector in 2D space.
	/// </summary>
	/// <typeparam name="TValue">The coordinate value data type.</typeparam>
	/// <remarks>
	/// The type used as the generic argument for a coordinate value should be immutable.
	/// </remarks>
	public struct Vector2<TValue> :
		IVector2<TValue>,
		IEquatable<Vector2<TValue>>
	{

		/// <summary>
		/// The x-coordinate of this vector.
		/// </summary>
		[NotNull] public readonly TValue X;
		/// <summary>
		/// The y-coordinate of this vector.
		/// </summary>
		[NotNull] public readonly TValue Y;

		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Vector2(TValue x, TValue y) {
			if(x == null) throw new ArgumentNullException("x");
			if(y == null) throw new ArgumentNullException("y");
			Contract.EndContractBlock();
			X = x;
			Y = y;
		}
		/// <summary>
		/// Creates a 2D vector.
		/// </summary>
		/// <param name="v">The coordinate tuple to copy values from.</param>
		public Vector2(ICoordinatePair<TValue> v) {
			if(null == v) throw new ArgumentNullException("v");
			if (v.X == null || v.Y  == null) throw new ArgumentException("Null coordinate values are not allowed.","v");
			Contract.EndContractBlock();
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

		public bool Equals(Vector2<TValue> other) {
			throw new NotImplementedException();
		}

		public override bool Equals(object obj) {
			return obj is Vector2<TValue> && Equals((Vector2<TValue>)obj);
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
