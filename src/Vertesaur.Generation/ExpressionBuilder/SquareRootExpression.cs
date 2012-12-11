﻿using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// An expression representing the square-root of another expression. SquareRootExpression(expression) = (expression)^(1/2).
	/// </summary>
	public class SquareRootExpression : ReducableUnaryExpressionBase
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

		/// <summary>
		/// Creates a new square root expression.
		/// </summary>
		/// <param name="parameter">The expression to find the square root of.</param>
		/// <param name="reductionExpressionGenerator">The optional expression generator used for reduction.</param>
		public SquareRootExpression(Expression parameter, IExpressionGenerator reductionExpressionGenerator = null)
			: base(parameter, reductionExpressionGenerator)
		{
			Contract.Requires(null != parameter);
			Contract.EndContractBlock();
		}

		/// <inheritdoc/>
		public override Expression Reduce() {
			if (UnaryParameter is SquareExpression)
				return ((SquareExpression)UnaryParameter).UnaryParameter;
			return typeof(double) == Type
				? (Expression)Call(MathSqrtMethod, UnaryParameter)
				: Convert(Call(MathSqrtMethod, Convert(UnaryParameter, typeof(double))), Type);
		}

	}
}
