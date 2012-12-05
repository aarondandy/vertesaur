using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur.PolygonOperation
{
	/// <summary>
	/// An operation that will find the geometric difference of one polygon from another.
	/// </summary>
	public class PolygonDifferenceOperation
	{

		private static readonly PolygonBinaryOperationOptions DefaultInverseRightIntersectionOptions;
		private static readonly PolygonIntersectionOperation DefaultInverseRightIntersectionOperation;

		static PolygonDifferenceOperation() {
			DefaultInverseRightIntersectionOptions = new PolygonBinaryOperationOptions {
				InvertLeftHandSide = false,
				InvertRightHandSide = true,
				InvertResult = false
			};
			DefaultInverseRightIntersectionOperation = new PolygonIntersectionOperation(DefaultInverseRightIntersectionOptions);
		}

		private readonly PolygonIntersectionOperation _rightInverseIntersectionOperation;

		/// <summary>
		/// Creates a default polygon difference operation.
		/// </summary>
		public PolygonDifferenceOperation() : this(new PolygonIntersectionOperation()) { } // TODO: null

		internal PolygonDifferenceOperation([CanBeNull] PolygonIntersectionOperation inverseRightOperation) {
			_rightInverseIntersectionOperation = inverseRightOperation ?? DefaultInverseRightIntersectionOperation;
		}

		/// <summary>
		/// Calculates the resulting difference of polygon <paramref name="b"/> subtracted from polygon <paramref name="a"/>.
		/// </summary>
		/// <param name="a">The polygon to be subracted from.</param>
		/// <param name="b">The polygon used to subtract from a.</param>
		/// <returns>The difference resulting from subtracting <paramref name="b"/> from <paramref name="a"/>.</returns>
		[ContractAnnotation("a:null=>null;a:notnull,b:null=>notnull"),CanBeNull]
		public IPlanarGeometry Difference([CanBeNull] Polygon2 a, [CanBeNull] Polygon2 b) {
			var inverseB = PolygonInverseOperation.Invert(b);
			var result = _rightInverseIntersectionOperation.Intersect(a, inverseB);
			return result;
		}

	}
}
