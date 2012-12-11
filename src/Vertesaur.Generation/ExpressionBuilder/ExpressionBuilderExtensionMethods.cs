using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{
	/// <summary>
	/// Utility and extension methods to support expression generation.
	/// </summary>
	public static class ExpressionBuilderExtensionMethods
	{
		/// <summary>
		/// Creates a new expression request.
		/// </summary>
		/// <param name="generator">The expression generator to use.</param>
		/// <param name="expressionName">The name of the requested expression.</param>
		/// <param name="inputExpressions">The requested inputs for the expression.</param>
		/// <returns>A new expression generation request.</returns>
		public static IExpressionGenerationRequest NewRequest(this IExpressionGenerator generator, string expressionName, params Expression[] inputExpressions) {
			Contract.Requires(null != generator);
			Contract.Requires(!String.IsNullOrEmpty(expressionName));
			Contract.Requires(null != inputExpressions);
			Contract.Requires(inputExpressions.Length != 0);
			Contract.Ensures(Contract.Result<IExpressionGenerationRequest>() != null);
			Contract.EndContractBlock();
			return new FunctionExpressionGenerationRequest(generator, expressionName, inputExpressions);
		}

		/// <summary>
		/// Creates a new expression request.
		/// </summary>
		/// <param name="generator">The expression generator to use.</param>
		/// <param name="expressionName">The name of the requested expression.</param>
		/// <param name="resultType">The desired result type of the expression.</param>
		/// <returns>A new expression generation request.</returns>
		public static IExpressionGenerationRequest NewRequest(this IExpressionGenerator generator, string expressionName, Type resultType) {
			Contract.Requires(null != generator);
			Contract.Requires(!String.IsNullOrEmpty(expressionName));
			Contract.Requires(null != resultType);
			Contract.Requires(!(typeof(void) == resultType));
			Contract.Ensures(Contract.Result<IExpressionGenerationRequest>() != null);
			Contract.EndContractBlock();
			return new ConstantExpressionGenerationRequest(generator, expressionName, resultType);
		}

		/// <summary>
		/// Creates a new expression.
		/// </summary>
		/// <param name="generator">The expression generator to use.</param>
		/// <param name="expressionName">The name of the requested expression.</param>
		/// <param name="inputExpressions">The requested inputs for the expression.</param>
		/// <returns>A new requested expression or null if one could not be generated.</returns>
		public static Expression GenerateExpression(this IExpressionGenerator generator, string expressionName, params Expression[] inputExpressions) {
			Contract.Requires(null != generator);
			Contract.Requires(!String.IsNullOrEmpty(expressionName));
			Contract.Requires(null != inputExpressions);
			Contract.Requires(inputExpressions.Length != 0);
			Contract.EndContractBlock();
			var request = NewRequest(generator, expressionName, inputExpressions);
			return generator.GenerateExpression(request);
		}

		/// <summary>
		/// Creates a new expression.
		/// </summary>
		/// <param name="generator">The expression generator to use.</param>
		/// <param name="expressionName">The name of the requested expression.</param>
		/// <param name="resultType">The desired result type of the expression.</param>
		/// <returns>A new reqiested expression or null if one could not be generated.</returns>
		public static Expression GenerateExpression(this IExpressionGenerator generator, string expressionName, Type resultType) {
			Contract.Requires(null != generator);
			Contract.Requires(!String.IsNullOrEmpty(expressionName));
			Contract.Requires(null != resultType);
			Contract.Requires(typeof(void) != resultType);
			Contract.EndContractBlock();
			var request = NewRequest(generator, expressionName, resultType);
			return generator.GenerateExpression(request);
		}

	}
}
