using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression representing the squared magnitude of a set of expressions representing coordinates.
	/// </summary>
	public class SquaredMagnitudeExpression : ReducableExpressionBase
	{

		public SquaredMagnitudeExpression(IList<Expression> components, IExpressionGenerator reductionExpressionGenerator = null)
			: base(reductionExpressionGenerator)
		{
			if(null == components) throw new ArgumentNullException("components");
			if(components.Count == 0) throw new ArgumentException("Must have at least 1 component.", "components");
			Contract.Requires(components.All(x => null != x));
			Contract.Ensures(Components != null);
			Contract.Ensures(Components.Count > 0);
			Contract.EndContractBlock();

			if (components.Any(x => null == x)) throw new ArgumentException("All components expressions must be non null.", "components");
			Components = Array.AsReadOnly(components.ToArray());
		}

		public ReadOnlyCollection<Expression> Components { get; private set; }

		public override Type Type {
			get { return Components[0].Type; }
		}

		public override Expression Reduce() {
			var result = ReductionExpressionGenerator.GenerateExpression(new FunctionExpressionGenerationRequest(
				ReductionExpressionGenerator, "Square", Components[0]));
			for (int i = 1; i < Components.Count; i++){
				var squaredComponentExpression = ReductionExpressionGenerator.GenerateExpression(
					new FunctionExpressionGenerationRequest(
						ReductionExpressionGenerator, "Square",
						Components[i]
					)
				);
				result = ReductionExpressionGenerator.GenerateExpression(
					new FunctionExpressionGenerationRequest(
						ReductionExpressionGenerator, "Add",
						result, squaredComponentExpression
					)
				);
			}
			return result;
		}

	}
}
