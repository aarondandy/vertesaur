using System;
using System.Collections.Generic;
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
        /// <param name="input">The requested input for the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression Generate(this IExpressionGenerator generator, string expressionName, Expression input) {
            Contract.Requires(generator != null);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(input != null);
            var request = NewRequest(generator, expressionName, input);
            return generator.Generate(request);
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="input0">The first requested input for the expression.</param>
        /// <param name="input1">The second requested input for the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression Generate(this IExpressionGenerator generator, string expressionName, Expression input0, Expression input1) {
            Contract.Requires(generator != null);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(input0 != null);
            Contract.Requires(input1 != null);
            var request = NewRequest(generator, expressionName, input0, input1);
            return generator.Generate(request);
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
        /// <param name="input0">The first input argument.</param>
        /// <param name="input1">The second input argument.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression GenerateOrThrow(this IExpressionGenerator generator, string expressionName, Expression input0, Expression input1) {
            Contract.Requires(null != generator);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(input0 != null);
            Contract.Requires(input1 != null);
            Contract.Ensures(Contract.Result<Expression>() != null);
            var inputs = new[] {input0, input1};
            var request = NewRequest(generator, expressionName, inputs);
            var result = generator.Generate(request);
            if (result == null)
                throw InvalidOp(expressionName, inputs);
            return result;
        }

        /// <summary>
        /// Creates a new expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="inputExpressions">The requested inputs for the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression GenerateOrThrow(this IExpressionGenerator generator, string expressionName, params Expression[] inputExpressions) {
            Contract.Requires(null != generator);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != inputExpressions);
            Contract.Requires(inputExpressions.Length != 0);
            Contract.Requires(Contract.ForAll(inputExpressions, x => x != null));
            Contract.Ensures(Contract.Result<Expression>() != null);
            var request = NewRequest(generator, expressionName, inputExpressions);
            var result = generator.Generate(request);
            if (result == null)
                throw InvalidOp(expressionName, inputExpressions);
            return result;
        }

        private static InvalidOperationException InvalidOp(string expressionName, IEnumerable<Expression> inputExpressions) {
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != inputExpressions);
            Contract.Ensures(Contract.Result<InvalidOperationException>() != null);
            return new InvalidOperationException(String.Format(
                "Failed to create expression \"{0}\" for {1}.",
                expressionName,
                String.Join(",",inputExpressions)
            ));
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
        /// Creates a new expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="expressionName">The name of the requested expression.</param>
        /// <param name="resultType">The desired result type of the expression.</param>
        /// <returns>A new requested expression or null if one could not be generated.</returns>
        public static Expression GenerateOrThrow(this IExpressionGenerator generator, string expressionName, Type resultType) {
            Contract.Requires(null != generator);
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != resultType);
            Contract.Ensures(Contract.Result<Expression>() != null);
            var request = NewRequest(generator, expressionName, resultType);
            var result = generator.Generate(request);
            if (result == null)
                throw InvalidOp(expressionName, resultType);
            return result;
        }

        private static InvalidOperationException InvalidOp(string expressionName, Type type) {
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.Requires(null != type);
            Contract.Ensures(Contract.Result<InvalidOperationException>() != null);
            return new InvalidOperationException(String.Format(
                "Failed to create expression \"{0}\" for {1}.",
                expressionName,
                type.FullName
            ));
        }

        /// <summary>
        /// Creates a new conversion expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="resultType">The desired result type of the expression to convert to.</param>
        /// <param name="input">The input expression to convert from.</param>
        /// <returns>A new requested conversion expression or null if one could not be generated.</returns>
        public static Expression GenerateConversion(this IExpressionGenerator generator, Type resultType, Expression input) {
            Contract.Requires(null != generator);
            Contract.Requires(null != resultType);
            Contract.Requires(null != input);
            var request = NewConversionRequest(generator, resultType, input);
            return generator.Generate(request);
        }

        /// <summary>
        /// Creates a new conversion expression.
        /// </summary>
        /// <param name="generator">The expression generator to use.</param>
        /// <param name="resultType">The desired result type of the expression to convert to.</param>
        /// <param name="input">The input expression to convert from.</param>
        /// <returns>A new requested conversion expression or null if one could not be generated.</returns>
        public static Expression GenerateConversionOrThrow(this IExpressionGenerator generator, Type resultType, Expression input) {
            Contract.Requires(null != generator);
            Contract.Requires(null != resultType);
            Contract.Requires(null != input);
            var request = NewConversionRequest(generator, resultType, input);
            var result = generator.Generate(request);
            if (result == null)
                throw InvalidOpConversion(input.Type, resultType);
            return result;
        }

        private static InvalidOperationException InvalidOpConversion(Type from, Type to) {
            Contract.Requires(from != null);
            Contract.Requires(to != null);
            Contract.Ensures(Contract.Result<InvalidOperationException>() != null);
            return new InvalidOperationException(String.Format(
                "Failed to create conversion from \"{0}\" to {1}.",
                from.FullName,
                to.FullName
            ));
        }

    }
}
