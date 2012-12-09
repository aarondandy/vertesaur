using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// A square root expression.
	/// </summary>
	public class SquareRootExpression : Expression
	{

		private static readonly MethodInfo MathSqrtMethod;

		static SquareRootExpression() {
			MathSqrtMethod = typeof(Math).GetMethod(
				"Sqrt",
				BindingFlags.Public | BindingFlags.Static,
				null,
				new[] { typeof(double) },
				null);
		}

		public SquareRootExpression(Expression parameter) {
			if(null == parameter) throw new ArgumentNullException("parameter");
			Contract.Ensures(ValueParameter != null);
			Contract.EndContractBlock();
			ValueParameter = parameter;
			Contract.Assume(ValueParameter != null);
		}

		/// <summary>
		/// The value or expression to find the square root of.
		/// </summary>
		public Expression ValueParameter { get; private set; }

		public override bool CanReduce { get { return true; } }

		public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

		public override Expression Reduce() {
			if (ValueParameter is SquareExpression)
				return ((SquareExpression)ValueParameter).ValueParameter;
			return typeof(double) == ValueParameter.Type
				? (Expression)Call(MathSqrtMethod, ValueParameter)
				: Convert(Call(MathSqrtMethod, Convert(ValueParameter, typeof(double))), ValueParameter.Type);
		}

		[ContractInvariantMethod]
		private void CodeContractInvariant() {
			Contract.Invariant(ValueParameter != null);
		}

	}
}
