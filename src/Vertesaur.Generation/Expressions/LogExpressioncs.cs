using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Vertesaur.Generation.Utility;
using Vertesaur.Utility;

namespace Vertesaur.Generation.Expressions
{
    public class LogExpression : ReducibleBinaryExpressionBase
    {

        private static readonly MethodInfo MathLogMethod;

        static LogExpression() {
            MathLogMethod = typeof (Math).GetPublicStaticInvokableMethod("Log", typeof (double), typeof (double));
        }

        public LogExpression(Expression value, Expression b, IExpressionGenerator generator = null)
            : base(value, b, generator) {
            Contract.Requires(value != null);
            Contract.Requires(b != null);
        }

        public override Type Type { get { return LeftParameter.Type; } }

        public override Expression Reduce() {
            Contract.Ensures(Contract.Result<Expression>() != null);
            return ReductionExpressionGenerator.BuildConversionCall(
                MathLogMethod,
                Type,
                LeftParameter,
                RightParameter);
        }
    }
}
