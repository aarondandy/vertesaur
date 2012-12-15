using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression to calculate the distance between two points.
	/// </summary>
	public class DistanceExpression : ReducableExpressionBase
	{

		/// <summary>
		/// Creates a new dot product expression.
		/// </summary>
		/// <param name="components">The ordered components of the two vectors in the order of first vectors coordinates then second vectors coordinates (ex: x0,y0,x1,y1).</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator that can be used to produce reduced expressions.</param>
		public DistanceExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
			: base(reductionExpressionGenerator)
		{
			if(null == components) throw new ArgumentNullException("components");
			if (components.Count == 0) throw new ArgumentException("Must have at least 1 component.", "components");
			if ((components.Count % 2) != 0) throw new ArgumentException("Must have an even number of components.", "components");
			Contract.Requires(components.All(x => null != x));
			Contract.Ensures(null != InnerExpression);
			Contract.EndContractBlock();

			if (components.Any(x => null == x)) throw new ArgumentException("All components expressions must be non null.", "components");
			InnerExpression = new SquaredDistanceExpression(components, reductionExpressionGenerator);
		}

		/// <summary>
		/// The internal squared distance expression.
		/// </summary>
		public SquaredDistanceExpression InnerExpression { get; private set; }

		/// <inheritdoc/>
		public override Type Type { get { return InnerExpression.Type; } }

		/// <inheritdoc/>
		public override Expression Reduce() {
			Contract.Ensures(Contract.Result<Expression>() != null);
			// TODO: use some square root utility method that does not take the square root of a square
			return ReductionExpressionGenerator.Generate("SquareRoot", InnerExpression)
				?? new SquareRootExpression(InnerExpression, ReductionExpressionGenerator);
		}

	}
}
