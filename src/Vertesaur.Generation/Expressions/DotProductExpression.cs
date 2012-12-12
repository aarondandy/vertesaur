using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.Expressions
{
	/// <summary>
	/// An expression representing the dot product of two vectors.
	/// </summary>
	public class DotProductExpression : ReducableExpressionBase
	{

		/// <summary>
		/// Creates a new dot product expression.
		/// </summary>
		/// <param name="components">The ordered components of the two vectors in the order of first vectors coordinates then second vectors coordinates (ex: x0,y0,x1,y1).</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator that can be used to produce reduced expressions.</param>
		public DotProductExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
			: base(reductionExpressionGenerator)
		{
			if(null == components) throw new ArgumentNullException("components");
			if (components.Count == 0) throw new ArgumentException("Must have at least 1 component.", "components");
			if ((components.Count % 2) != 0) throw new ArgumentException("Must have an even number of components.", "components");
			Contract.Requires(components.All(x => null != x));
			Contract.Ensures(Components != null);
			Contract.Ensures(Components.Count > 0);
			Contract.EndContractBlock();

			if (components.Any(x => null == x)) throw new ArgumentException("All components expressions must be non null.", "components");
			Components = Array.AsReadOnly(components.ToArray());
		}

		/// <summary>
		/// The coordinate expressions to find the dot product of.
		/// </summary>
		public ReadOnlyCollection<Expression> Components { get; private set; }

		/// <inheritdoc/>
		public override Type Type {
			get { return Components[0].Type; }
		}

		/// <inheritdoc/>
		public override Expression Reduce(){
			var halfCount = Components.Count / 2;
			var result = ReductionExpressionGenerator.GenerateExpression(
				"Multiply", Components[0], Components[halfCount]);
			for (int i = 1; i < halfCount; i++){
				var mulComponentExpression = ReductionExpressionGenerator.GenerateExpression(
					"Multiply", Components[i], Components[i + halfCount]);
				result = ReductionExpressionGenerator.GenerateExpression(
					"Add", result, mulComponentExpression);
			}
			return result;
		}

	}
}
