using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	public class SquaredMagnitudeExpression : Expression
	{

		public SquaredMagnitudeExpression(IList<Expression> components, IBasicExpressionGenerator basicExpressionGenerator = null) {
			if(null == components) throw new ArgumentNullException("components");
			if(components.Any(x => null == x)) throw new ArgumentException("all components expressions must be non null", "components");
			Contract.Ensures(Components != null);
			Contract.EndContractBlock();
			Components = Array.AsReadOnly(components.ToArray());
			BasicExpressionGenerator = basicExpressionGenerator;
		}

		/// <summary>
		/// An expression generator that can be used to create the required expressions.
		/// </summary>
		public IBasicExpressionGenerator BasicExpressionGenerator { get; private set; }

		public ReadOnlyCollection<Expression> Components { get; private set; }

		public override bool CanReduce { get { return true; } }

		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		public override Expression Reduce() {
			var generator = BasicExpressionGenerator ?? DefaultBasicExpressionGenerator.Default;
			if (0 == Components.Count)
				return generator.GetConstantExpression(BasicConstantOperationType.ValueZero, typeof(double));
			var result = generator.GetUnaryExpression(BasicUnaryOperationType.Square, Components[0].Type, Components[0]);
			for (int i = 1; i < Components.Count; i++) {
				result = generator.GetBinaryExpression(
					BasicBinaryOperationType.Add, result.Type,
					result,
					generator.GetUnaryExpression(BasicUnaryOperationType.Square, Components[i].Type, Components[i])
				);
			}
			return result;
		}

	}
}
