using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Vertesaur.Generation.Expressions
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
            Contract.Requires(Contract.ForAll(inputExpressions, x => x != null));
            Contract.Ensures(Contract.Result<IExpressionGenerationRequest>() != null);
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
            Contract.Ensures(Contract.Result<IExpressionGenerationRequest>() != null);
            return new ConstantExpressionGenerationRequest(generator, expressionName, resultType);
        }

        /// <summary>
        /// Creates a new conversion expression request.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="resultType">The desired result type of the conversion expression.</param>
        /// <param name="input">The input expression to be converted.</param>
        /// <returns>A new expression generation request.</returns>
        public static IExpressionGenerationRequest NewConversionRequest(this IExpressionGenerator generator, Type resultType, Expression input) {
            Contract.Requires(null != generator);
            Contract.Requires(null != resultType);
            Contract.Requires(null != input);
            Contract.Ensures(Contract.Result<IExpressionGenerationRequest>() != null);
            return new ConversionExpressionRequest(generator, input, resultType);
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="inputExpressions">The requested inputs for the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression Generate(this IExpressionGenerator generator, string expressionName, params Expression[] inputExpressions) {
            Contract.Requires(null != generator);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != inputExpressions);
            Contract.Requires(inputExpressions.Length != 0);
            Contract.Requires(Contract.ForAll(inputExpressions, x => x != null));
            var request = NewRequest(generator, expressionName, inputExpressions);
            return generator.Generate(request);
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="resultType">The desired result type of the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression Generate(this IExpressionGenerator generator, string expressionName, Type resultType) {
            Contract.Requires(null != generator);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != resultType);
            var request = NewRequest(generator, expressionName, resultType);
            return generator.Generate(request);
        }

        /// <summary>
        /// Creates a new conversion expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="resultType">The desired result type of the expression to convert to.</param>
        /// <param name="input">The input expression to convert from.</param>
        /// <returns>A new requested conversion expression or null if one could not be generated.</returns>
        public static Expression GenerateConversionExpression(this IExpressionGenerator generator, Type resultType, Expression input) {
            Contract.Requires(null != generator);
            Contract.Requires(null != resultType);
            Contract.Requires(null != input);
            var request = NewConversionRequest(generator, resultType, input);
            return generator.Generate(request);
        }

    }
}
