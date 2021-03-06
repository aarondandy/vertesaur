﻿using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    /// <summary>
    /// A base 10 logarithm expression.
    /// </summary>
    public class Log10Expression : ReducibleUnaryExpressionBase
    {
        private static readonly MethodInfo MathLog10Method;

        static Log10Expression() {
            MathLog10Method = typeof(Math).GetPublicStaticInvokableMethod("Log10", typeof(double));
        }

        /// <summary>
        /// Creates a new base 10 logarithm expression.
        /// </summary>
        /// <param name="input">The expression to calculate the base 10 logarithm of.</param>
        /// <param name="generator">The optional expression generator used during reduction.</param>
        public Log10Expression(Expression input, IExpressionGenerator generator = null)
            : base(input, generator) {
            Contract.Requires(null != input);
        }

        /// <inheritdoc/>
        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            var gen = ReductionExpressionGenerator;
            if (Type == typeof (double) || Type == typeof (float))
                return gen.BuildConversionCall(MathLog10Method, UnaryParameter, Type);

            return gen.GenerateOrThrow("LOG", UnaryParameter, gen.GenerateOrThrow("10", Type));
        }
    }
}
