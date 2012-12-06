using JetBrains.Annotations;
using Vertesaur.Contracts;

namespace Vertesaur.PolygonOperation
{
	/// <summary>
	/// An operation that will find the symmetric difference for two polygons.
	/// Also known as exclusive-or (XOR).
	/// </summary>
	public class PolygonXorOperation
	{

		private static readonly PolygonDifferenceOperation DefaultDifferenceOperation;
		private static readonly PolygonUnionOperation DefaultUnionOperation;

		static PolygonXorOperation() {
			DefaultDifferenceOperation = new PolygonDifferenceOperation();
			DefaultUnionOperation = new PolygonUnionOperation();
		}

		private readonly PolygonDifferenceOperation _differenceOperation;
		private readonly PolygonUnionOperation _unionOperation;

		/// <summary>
		/// Constructs a default symmetric difference operation.
		/// </summary>
		public PolygonXorOperation()
			: this(null, null) { }

		internal PolygonXorOperation([CanBeNull] PolygonDifferenceOperation differenceOperation, [CanBeNull] PolygonUnionOperation unionOperation) {
			_differenceOperation = differenceOperation ?? DefaultDifferenceOperation;
			_unionOperation = unionOperation ?? DefaultUnionOperation;
		}

		/// <summary>
		/// Calculates the symmetric difference between two polygons.
		/// </summary>
		/// <param name="a">A polygon.</param>
		/// <param name="b">A polygon.</param>
		/// <returns>The symetric difference of <paramref name="a"/> and <paramref name="b"/>.</returns>
		[ContractAnnotation("a:null,b:null=>null;a:null,b:notnull=>notnull;a:notnull,b:null=>notnull;a:notnull,b:notnull=>canbenull"), CanBeNull]
		public IPlanarGeometry Xor([CanBeNull] Polygon2 a, [CanBeNull] Polygon2 b) {
			if (null == a)
				return b;
			if (null == b)
				return a;

			var removedFromA = _differenceOperation.Difference(a, b) as Polygon2;
			var removedFromB = _differenceOperation.Difference(b, a) as Polygon2;
			var unionedLeftovers = _unionOperation.Union(removedFromA, removedFromB);
			return unionedLeftovers;
		}

	}
}
