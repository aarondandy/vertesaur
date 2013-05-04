using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Expressions;

namespace Vertesaur.Generation.GenericOperations
{
    /// <summary>
    /// Basic generic operations that are generated at run-time.
    /// </summary>
    /// <typeparam name="TValue">The value type the operations are primarily for.</typeparam>
    public class BasicOperations<TValue>
    {

        static BasicOperations() {
            Default = new BasicOperations<TValue>(
#if !NO_MEF
                new MefCombinedExpressionGenerator()
#else
                CombinedExpressionGenerator.GenerateDefaultMefReplacement()
#endif
            );
        }

        /// <summary>
        /// The default instance of the basic operations.
        /// </summary>
        public static BasicOperations<TValue> Default { get; private set; }

        /// <summary>
        /// Defines a method that acts as a unary operation on the generic type.
        /// </summary>
        /// <param name="value">The single input.</param>
        /// <returns>The result of the operation.</returns>
        public delegate TValue UnaryFunc(TValue value);

        /// <summary>
        /// Defines a method that acts as a binary operation on the generic type.
        /// </summary>
        /// <param name="left">The left input.</param>
        /// <param name="right">The right input.</param>
        /// <returns>The result of the operation.</returns>
        public delegate TValue BinaryFunc(TValue left, TValue right);

        /// <summary>
        /// Defines a method that accepts two coordinates in reverse order.
        /// </summary>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <returns>THe result of the operation.</returns>
        public delegate TValue ReverseCoordinates(TValue y, TValue x);

        /// <summary>
        /// A delegate for a comparison test between two values of the same type that returns in a boolean result.
        /// </summary>
        /// <param name="left">The left parameter.</param>
        /// <param name="right">The right parameter.</param>
        /// <returns>The result of invoking the test on the given parameters.</returns>
        public delegate bool ComparisonTest(TValue left, TValue right);

        /// <summary>
        /// Defines a method that should return a constant value.
        /// </summary>
        /// <returns>A constant value.</returns>
        public delegate TValue CreateConstantFunc();

        /// <summary>
        /// A run-time executable generic addition function.
        /// </summary>
        public readonly BinaryFunc Add;
        /// <summary>
        /// A run-time executable generic subtraction function.
        /// </summary>
        public readonly BinaryFunc Subtract;
        /// <summary>
        /// A run-time executable generic multiplication function.
        /// </summary>
        public readonly BinaryFunc Multiply;
        /// <summary>
        /// A run-time executable generic division function.
        /// </summary>
        public readonly BinaryFunc Divide;
        /// <summary>
        /// A run-time executable generic negation function.
        /// </summary>
        public readonly UnaryFunc Negate;
        /// <summary>
        /// A run-time sine generic function.
        /// </summary>
        public readonly UnaryFunc Sin;
        /// <summary>
        /// A run-time cosine generic function.
        /// </summary>
        public readonly UnaryFunc Cos;
        /// <summary>
        /// A run-time tangent generic function.
        /// </summary>
        public readonly UnaryFunc Tan;
        /// <summary>
        /// A run-time arc-sine generic function.
        /// </summary>
        public readonly UnaryFunc Asin;
        /// <summary>
        /// A run-time arc-cosine generic function.
        /// </summary>
        public readonly UnaryFunc Acos;
        /// <summary>
        /// A run-time arc-tangent generic function.
        /// </summary>
        public readonly UnaryFunc Atan;
        /// <summary>
        /// A run-time hyperbolic sine generic function.
        /// </summary>
        public readonly UnaryFunc Sinh;
        /// <summary>
        /// A run-time hyperbolic cosine generic function.
        /// </summary>
        public readonly UnaryFunc Cosh;
        /// <summary>
        /// A run-time hyperbolic tangent generic function.
        /// </summary>
        public readonly UnaryFunc Tanh;
        /// <summary>
        /// A run-time inverse hyperbolic sine generic function.
        /// </summary>
        public readonly UnaryFunc Asinh;
        /// <summary>
        /// A run-time inverse hyperbolic cosine generic function.
        /// </summary>
        public readonly UnaryFunc Acosh;
        /// <summary>
        /// A run-time inverse hyperbolic tangent generic function.
        /// </summary>
        public readonly UnaryFunc Atanh;
        /// <summary>
        /// A run-time ceiling generic function.
        /// </summary>
        public readonly UnaryFunc Ceiling;
        /// <summary>
        /// A run-time floor generic function.
        /// </summary>
        public readonly UnaryFunc Floor;
        /// <summary>
        /// A run-time truncate generic function.
        /// </summary>
        public readonly UnaryFunc Truncate;
        /// <summary>
        /// A run-time natural logarithm generic function.
        /// </summary>
        public readonly UnaryFunc Log;
        /// <summary>
        /// A run-time base 10 logarithm generic function.
        /// </summary>
        public readonly UnaryFunc Log10;
        /// <summary>
        /// A run-time E^n generic function.
        /// </summary>
        public readonly UnaryFunc Exp;
        /// <summary>
        /// A run-time absolute value generic function.
        /// </summary>
        public readonly UnaryFunc Abs;
        /// <summary>
        /// A run-time arc-tangent 2 generic function accepting a y-coordinate and x-coordinate.
        /// </summary>
        public readonly ReverseCoordinates Atan2;
        /// <summary>
        /// A run-time power generic function.
        /// </summary>
        public readonly BinaryFunc Pow;
        /// <summary>
        /// A run-time minimum generic function.
        /// </summary>
        public readonly BinaryFunc Min;
        /// <summary>
        /// A run-time maximum generic function.
        /// </summary>
        public readonly BinaryFunc Max;

        /// <summary>
        /// Converts a generic typed value to the double type.
        /// </summary>
        public readonly Func<TValue, double> ToDouble;
        /// <summary>
        /// Converts a double typed value to the generic type.
        /// </summary>
        public readonly Func<double, TValue> FromDouble;

        /// <summary>
        /// A run-time generated generic equality comparison test.
        /// </summary>
        public readonly ComparisonTest Equal;
        /// <summary>
        /// A run-time generated generic inequality comparison test.
        /// </summary>
        public readonly ComparisonTest NotEqual;
        /// <summary>
        /// A run-time generated generic less-than comparison test.
        /// </summary>
        public readonly ComparisonTest Less;
        /// <summary>
        /// A run-time generated generic less-than or equal comparison test.
        /// </summary>
        public readonly ComparisonTest LessOrEqual;
        /// <summary>
        /// A run-time generated generic greater-than comparison test.
        /// </summary>
        public readonly ComparisonTest Greater;
        /// <summary>
        /// A run-time generated generic greater-than or equal comparison test.
        /// </summary>
        public readonly ComparisonTest GreaterOrEqual;
        /// <summary>
        /// A run-time generated generic comparison test, implemented as a standard <see cref="System.IComparable{T}.CompareTo(T)">CompareTo</see> comparison.
        /// </summary>
        public readonly Comparison<TValue> CompareTo;

        /// <summary>
        /// Creates a new basic generic operations implementation using the given expression generator.
        /// </summary>
        /// <param name="expressionGenerator">The expression generator that will be used to build the generic methods at run-time.</param>
        public BasicOperations(IExpressionGenerator expressionGenerator) {
            if(null == expressionGenerator) throw new ArgumentNullException("expressionGenerator");
            Contract.EndContractBlock();
            ExpressionGenerator = expressionGenerator;

            Add = BuildBinaryFunc("Add");
            Subtract = BuildBinaryFunc("Subtract");
            Multiply = BuildBinaryFunc("Multiply");
            Divide = BuildBinaryFunc("Divide");
            Negate = BuildUnaryFunc("Negate");
            Sin = BuildUnaryFunc("Sin");
            Cos = BuildUnaryFunc("Cos");
            Tan = BuildUnaryFunc("Tan");
            Asin = BuildUnaryFunc("Asin");
            Acos = BuildUnaryFunc("Acos");
            Atan = BuildUnaryFunc("Atan");
            Sinh = BuildUnaryFunc("Sinh");
            Cosh = BuildUnaryFunc("Cosh");
            Tanh = BuildUnaryFunc("Tanh");
            Asinh = BuildUnaryFunc("Asinh");
            Acosh = BuildUnaryFunc("Acosh");
            Atanh = BuildUnaryFunc("Atanh");
            Log = BuildUnaryFunc("Log");
            Log10 = BuildUnaryFunc("Log10");
            Exp = BuildUnaryFunc("Exp");
            Abs = BuildUnaryFunc("Abs");
            Atan2 = BuildBinaryFunc<ReverseCoordinates>("Atan2");
            Pow = BuildBinaryFunc("Pow");
            Ceiling = BuildUnaryFunc("Ceiling");
            Floor = BuildUnaryFunc("Floor");
            Truncate = BuildUnaryFunc("Truncate");
            Min = BuildBinaryFunc("Min");
            Max = BuildBinaryFunc("Max");

            Equal = BuildBinaryFunc<ComparisonTest>("Equal");
            NotEqual = BuildBinaryFunc<ComparisonTest>("NotEqual");
            Less = BuildBinaryFunc<ComparisonTest>("Less");
            LessOrEqual = BuildBinaryFunc<ComparisonTest>("LessEqual");
            Greater = BuildBinaryFunc<ComparisonTest>("Greater");
            GreaterOrEqual = BuildBinaryFunc<ComparisonTest>("GreaterOrEqual");
            CompareTo = BuildBinaryFunc<Comparison<TValue>>("CompareTo");

            ToDouble = BuildConversion<TValue, double>();
            FromDouble = BuildConversion<double, TValue>();
            _zeroValueGenerator = BuildConstant("0");
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void CodeContractInvariant() {
            Contract.Invariant(ExpressionGenerator != null);
        }

        private Func<TFrom, TTo> BuildConversion<TFrom, TTo>() {
            var inputParam = Expression.Parameter(typeof(TFrom));
            var expression = ExpressionGenerator.GenerateConversionExpression(typeof(TTo), inputParam);
            if (null == expression)
                return null;
            return Expression.Lambda<Func<TFrom, TTo>>(expression, inputParam).Compile();
        }

        private CreateConstantFunc BuildConstant(string expressionName) {
            Contract.Requires(!String.IsNullOrEmpty(expressionName));
            Contract.EndContractBlock();
            var expression = ExpressionGenerator.Generate(expressionName, typeof(TValue));
            if (null == expression)
                return null;
            return Expression.Lambda<CreateConstantFunc>(expression).Compile();
        }

        private UnaryFunc BuildUnaryFunc(string expressionName) {
            Contract.Requires(!string.IsNullOrEmpty(expressionName));
            var tParam = Expression.Parameter(typeof(TValue));
            var expression = ExpressionGenerator.Generate(expressionName, tParam);
            if (null == expression)
                return null;
            return Expression.Lambda<UnaryFunc>(expression, tParam).Compile();
        }

        private BinaryFunc BuildBinaryFunc(string expressionName) {
            Contract.Requires(!string.IsNullOrEmpty(expressionName));
            return BuildBinaryFunc<BinaryFunc>(expressionName);
        }

        private TResult BuildBinaryFunc<TResult>(string expressionName) where TResult : class {
            Contract.Requires(!string.IsNullOrEmpty(expressionName));
            var tParam0 = Expression.Parameter(typeof(TValue));
            var tParam1 = Expression.Parameter(typeof(TValue));
            var expression = ExpressionGenerator.Generate(expressionName, tParam0, tParam1);
            if (null == expression)
                return null;
            return Expression.Lambda<TResult>(expression, tParam0, tParam1).Compile();
        }

        /// <summary>
        /// The expression generator that was used to create the run-time executable generic functions.
        /// </summary>
        public IExpressionGenerator ExpressionGenerator { get; private set; }

        private readonly CreateConstantFunc _zeroValueGenerator;

        /// <summary>
        /// A new constant with a value of zero.
        /// </summary>
        public TValue ZeroValue { get { return _zeroValueGenerator(); } }

    }
}
