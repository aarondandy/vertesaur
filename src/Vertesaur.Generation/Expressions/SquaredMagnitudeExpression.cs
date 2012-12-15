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
	/// An expression representing the squared magnitude of a set of expressions representing coordinates.
	/// </summary>
	public class SquaredMagnitudeExpression : ReducableExpressionBase
	{

		/// <summary>
		/// Creates a new squared magnitude expression.
		/// </summary>
		/// <param name="components">The coordinate expressions.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
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

		/// <summary>
		/// The coordinate expressions.
		/// </summary>
		public ReadOnlyCollection<Expression> Components { get; private set; }

		/// <inheritdoc/>
		public override Type Type {
			get { return Components[0].Type; }
		}

		/// <inheritdoc/>
		public override Expression Reduce() {
			// make sure everything going into the expression can be evaluated multiple times
			var squareInputs = new List<Expression>(Components.Count);
			var blockBuilder = new BlockExpressionBuilder();
			foreach (var component in Components) {
				if (component.IsMemoryLocationOrConstant()) {
					squareInputs.Add(component);
				}
				else {
					var local = blockBuilder.LocalManager.GetVariable(component.Type);
					blockBuilder.Add(Expression.Assign(local, component));
					squareInputs.Add(local);
				}
			}

			// build the equation
			var result = ReductionExpressionGenerator.Generate("Square", squareInputs[0]);
			for (int i = 1; i < squareInputs.Count; i++) {
				var squaredComponentExpression = ReductionExpressionGenerator.Generate(
					"Square", squareInputs[i]);
				result = ReductionExpressionGenerator.Generate(
					"Add", result, squaredComponentExpression);
			}

			// return the block if it is needed
			if (blockBuilder.Count > 0) {
				blockBuilder.Add(result);
				return blockBuilder.GetExpression();
			}
			else {
				return result;
			}
		}

	}
}
