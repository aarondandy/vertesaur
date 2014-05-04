using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Utility;

namespace Vertesaur.Generation.Expressions
{

    /// <summary>
    /// The core expression generator for basic arithmetic and comparisons.
    /// </summary>
#if !NO_MEF
    [System.ComponentModel.Composition.Export(typeof(IExpressionGenerator))]
#endif
    public class CoreExpressionGenerator : IExpressionGenerator
    {

        /// <summary>
        /// These methods are used through reflection.
        /// </summary>
        private static class SpecializedOperations
        {

            // ReSharper disable UnusedMember.Local
            public static byte AddChecked(byte leftHandSide, byte rightHandSide) {
                return checked((byte)(leftHandSide + rightHandSide));
            }

            public static byte AddUnchecked(byte leftHandSide, byte rightHandSide) {
                return unchecked((byte)(leftHandSide + rightHandSide));
            }

            public static sbyte AddChecked(sbyte leftHandSide, sbyte rightHandSide) {
                return checked((sbyte)(leftHandSide + rightHandSide));
            }

            public static sbyte AddUnchecked(sbyte leftHandSide, sbyte rightHandSide) {
                return unchecked((sbyte)(leftHandSide + rightHandSide));
            }

            public static char AddChecked(char leftHandSide, char rightHandSide) {
                return checked((char)(leftHandSide + rightHandSide));
            }

            public static char AddUnchecked(char leftHandSide, char rightHandSide) {
                return unchecked((char)(leftHandSide + rightHandSide));
            }

            public static byte SubtractChecked(byte leftHandSide, byte rightHandSide) {
                return checked((byte)(leftHandSide - rightHandSide));
            }

            public static byte SubtractUnchecked(byte leftHandSide, byte rightHandSide) {
                return unchecked((byte)(leftHandSide - rightHandSide));
            }

            public static sbyte SubtractChecked(sbyte leftHandSide, sbyte rightHandSide) {
                return checked((sbyte)(leftHandSide - rightHandSide));
            }

            public static sbyte SubtractUnchecked(sbyte leftHandSide, sbyte rightHandSide) {
                return unchecked((sbyte)(leftHandSide - rightHandSide));
            }

            public static char SubtractChecked(char leftHandSide, char rightHandSide) {
                return checked((char)(leftHandSide - rightHandSide));
            }

            public static char SubtractUnchecked(char leftHandSide, char rightHandSide) {
                return unchecked((char)(leftHandSide - rightHandSide));
            }

            public static byte MultiplyChecked(byte leftHandSide, byte rightHandSide) {
                return checked((byte)(leftHandSide * rightHandSide));
            }

            public static byte MultiplyUnchecked(byte leftHandSide, byte rightHandSide) {
                return unchecked((byte)(leftHandSide * rightHandSide));
            }

            public static sbyte MultiplyChecked(sbyte leftHandSide, sbyte rightHandSide) {
                return checked((sbyte)(leftHandSide * rightHandSide));
            }

            public static sbyte MultiplyUnchecked(sbyte leftHandSide, sbyte rightHandSide) {
                return unchecked((sbyte)(leftHandSide * rightHandSide));
            }

            public static char MultiplyChecked(char leftHandSide, char rightHandSide) {
                return checked((char)(leftHandSide * rightHandSide));
            }

            public static char MultiplyUnchecked(char leftHandSide, char rightHandSide) {
                return unchecked((char)(leftHandSide * rightHandSide));
            }

            public static byte DivideChecked(byte leftHandSide, byte rightHandSide) {
                return checked((byte)(leftHandSide / rightHandSide));
            }

            public static byte DivideUnchecked(byte leftHandSide, byte rightHandSide) {
                return unchecked((byte)(leftHandSide / rightHandSide));
            }

            public static sbyte DivideChecked(sbyte leftHandSide, sbyte rightHandSide) {
                return checked((sbyte)(leftHandSide / rightHandSide));
            }

            public static sbyte DivideUnchecked(sbyte leftHandSide, sbyte rightHandSide) {
                return unchecked((sbyte)(leftHandSide / rightHandSide));
            }

            public static char DivideChecked(char leftHandSide, char rightHandSide) {
                return checked((char)(leftHandSide / rightHandSide));
            }

            public static char DivideUnchecked(char leftHandSide, char rightHandSide) {
                return unchecked((char)(leftHandSide / rightHandSide));
            }

            // ReSharper restore UnusedMember.Local

        }

        /// <summary>
        /// The default unchecked expression generator.
        /// </summary>
        public static CoreExpressionGenerator Default { get; private set; }

        /// <summary>
        /// The default checked expression generator.
        /// </summary>
        public static CoreExpressionGenerator DefaultChecked { get; private set; }

        static CoreExpressionGenerator() {
            Default = new CoreExpressionGenerator(false);
            DefaultChecked = new CoreExpressionGenerator(true);
        }

        private readonly Dictionary<string, Func<IExpressionGenerationRequest, Expression, Expression>> _standardUnaryExpressionGeneratorLookup;
        private readonly Dictionary<string, Func<IExpressionGenerationRequest, Expression, Expression, Expression>> _standardBinaryExpressionGeneratorLookup;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// Operations that can be, are non-checked operations by default.
        /// </remarks>
        public CoreExpressionGenerator() : this(false) { }

        /// <summary>
        /// Constructs a new expression generator.
        /// </summary>
        /// <param name="isChecked">Flag to determine if operations that can be, are checked operations.</param>
        public CoreExpressionGenerator(bool isChecked) {
            Checked = isChecked;
            _standardUnaryExpressionGeneratorLookup = new Dictionary<string, Func<IExpressionGenerationRequest, Expression, Expression>>(StringComparer.OrdinalIgnoreCase){
                {"SQUARE", GenerateSquare},
                {"SQUAREROOT", GenerateSquareRoot},
                {"NEGATE", GenerateNegation},
                {"SIN", (g,i) => new SinExpression(i,g.TopLevelGenerator)},
                {"COS", (g,i) => new CosExpression(i,g.TopLevelGenerator)},
                {"TAN", (g,i) => new TanExpression(i,g.TopLevelGenerator)},
                {"ASIN", (g,i) => new AsinExpression(i,g.TopLevelGenerator)},
                {"ACOS", (g,i) => new AcosExpression(i,g.TopLevelGenerator)},
                {"ATAN", (g,i) => new AtanExpression(i,g.TopLevelGenerator)},
                {"CEILING", (g,i) => new CeilingExpression(i,g.TopLevelGenerator)},
                {"FLOOR", (g,i) => new FloorExpression(i,g.TopLevelGenerator)},
                {"TRUNCATE",(g,i) => new TruncateExpression(i,g.TopLevelGenerator)},
                {"COSH", (g,i) => new CoshExpression(i,g.TopLevelGenerator)},
                {"SINH", (g,i) => new SinhExpression(i,g.TopLevelGenerator)},
                {"TANH", (g,i) => new TanhExpression(i,g.TopLevelGenerator)},
                {"LOG", (g,i) => new NaturalLogExpression(i,g.TopLevelGenerator)},
                {"LN", (g,i) => new NaturalLogExpression(i,g.TopLevelGenerator)},
                {"LOG10", (g,i) => new Log10Expression(i,g.TopLevelGenerator)},
                {"EXP", (g,i) => new ExpExpression(i,g.TopLevelGenerator)},
                {"ABS", (g,i) => new AbsExpression(i,g.TopLevelGenerator)},
                {"ASINH", (g,i) => new AsinhExpression(i,g.TopLevelGenerator)},
                {"ACOSH", (g,i) => new AcoshExpression(i,g.TopLevelGenerator)},
                {"ATANH", (g,i) => new AtanhExpression(i,g.TopLevelGenerator)}
            };
            _standardBinaryExpressionGeneratorLookup = new Dictionary<string, Func<IExpressionGenerationRequest, Expression, Expression, Expression>>(StringComparer.OrdinalIgnoreCase){
                {"ADD", GenerateArithmetic},
                {"SUBTRACT", GenerateArithmetic},
                {"MULTIPLY", GenerateArithmetic},
                {"DIVIDE", GenerateArithmetic},
                {"EQUAL", (_,left,right) => Expression.Equal(left, right)},
                {"NOTEQUAL",(_,left,right) => Expression.NotEqual(left, right)},
                {"LESS", (_,left,right) => Expression.LessThan(left, right)},
                {"LESSEQUAL", (_,left,right) => Expression.LessThanOrEqual(left, right)},
                {"GREATER", (_,left,right) => Expression.GreaterThan(left, right)},
                {"GREATEREQUAL", (_,left,right) => Expression.GreaterThanOrEqual(left, right)},
                {"MIN", (g,left,right) => new MinExpression(left,right,g.TopLevelGenerator)},
                {"MAX", (g,left,right) => new MaxExpression(left,right,g.TopLevelGenerator)},
                {"COMPARETO", GenerateCompareTo},
                {"ATAN2", (g,left,right) => new Atan2Expression(left,right,g.TopLevelGenerator)},
                {"POW", CreatePow},
                {"LOG", (g,v,b) => new LogExpression(v,b,g.TopLevelGenerator)}
            };
        }

        [ContractInvariantMethod]
        private void ObjectInvariants() {
            Contract.Invariant(_standardUnaryExpressionGeneratorLookup != null);
            Contract.Invariant(_standardBinaryExpressionGeneratorLookup != null);
        }

        private Expression CreatePow(IExpressionGenerationRequest request, Expression left, Expression right) {
            Contract.Requires(null != request);
            Contract.Requires(null != left);
            Contract.Requires(null != right);

            if (left.Type == typeof(double) && right.Type == typeof(double))
                return Expression.Power(left, right);

            var lhsDouble = request.TopLevelGenerator.GenerateConversion(typeof (double), left);
            var rhsDouble = request.TopLevelGenerator.GenerateConversion(typeof (double), right);
            if (lhsDouble == null || rhsDouble == null)
                return null;

            Expression result = Expression.Power(lhsDouble, rhsDouble);

            if (request.DesiredResultType != null && request.DesiredResultType != result.Type)
                result = request.TopLevelGenerator.GenerateConversion(request.DesiredResultType, result);

            return result;
        }

        private Expression GenerateSquare(IExpressionGenerationRequest request, Expression parameter) {
            Contract.Requires(null != request);
            Contract.Requires(null != parameter);
            Contract.Ensures(Contract.Result<Expression>() != null);
            return new SquareExpression(parameter, request.TopLevelGenerator);
        }

        private Expression GenerateSquareRoot(IExpressionGenerationRequest request, Expression parameter) {
            Contract.Requires(null != request);
            Contract.Requires(null != parameter);
            Contract.Ensures(Contract.Result<Expression>() != null);
            return new SquareRootExpression(parameter, request.TopLevelGenerator);
        }

        private Expression GenerateNegation(IExpressionGenerationRequest request, Expression parameter) {
            Contract.Requires(null != request);
            Contract.Requires(null != parameter);
            var resultType = parameter.Type;
            if (resultType == typeof(ulong) || resultType == typeof(uint) || resultType == typeof(ushort) || resultType == typeof(byte) || resultType == typeof(sbyte)) {
                var zeroConstant = GenerateConstantExpression("0", resultType);
                Contract.Assume(null != zeroConstant);
                return request.TopLevelGenerator.Generate("Subtract", zeroConstant, parameter);
            }
            return Checked ? Expression.NegateChecked(parameter) : Expression.Negate(parameter);
        }

        private static string ToTitleCase(string text) {
            Contract.Ensures(Contract.Result<string>() != null);
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            if (text.Length == 1)
                return text.ToUpperInvariant();
            return String.Concat(
                Char.ToUpperInvariant(text[0]),
                text.Substring(1).ToLowerInvariant()
            );
        }

        private Expression GenerateArithmetic(IExpressionGenerationRequest request, IList<Expression> inputs) {
            Contract.Requires(request != null);
            Contract.Requires(inputs != null);
            Contract.Requires(inputs.Count >= 1);
            Contract.Requires(Contract.ForAll(inputs, x => x != null));
            Contract.Assume(inputs[0] != null);
            var result = inputs[0]; // GenerateArithmetic(request, inputs[0], inputs[1]);
            if (null == result)
                return null;
            for (int i = 1; i < inputs.Count; i++) {
                Contract.Assume(inputs[i] != null);
                result = GenerateArithmetic(request, result, inputs[i]);
                if (null == result)
                    return null;
            }
            return result;
        }

        private Expression GenerateArithmetic(IExpressionGenerationRequest request, Expression left, Expression right) {
            Contract.Requires(null != request);
            Contract.Requires(null != left);
            Contract.Requires(null != right);
            if ((left.Type == right.Type) && (left.Type == typeof(byte) || left.Type == typeof(char) || left.Type == typeof(sbyte))) {
                var methodName = ToTitleCase(request.ExpressionName) + (Checked ? "Checked" : "Unchecked");
                Contract.Assume(!String.IsNullOrEmpty(methodName));
                var method = typeof(SpecializedOperations).GetPublicStaticInvokableMethod(
                    methodName,
                    new[] { left.Type, left.Type }
                );
                if (null != method) {
                    return Expression.Call(method, left, right);
                }
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Add"))
                return Checked ? Expression.AddChecked(left, right) : Expression.Add(left, right);
            if (StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Subtract"))
                return Checked ? Expression.SubtractChecked(left, right) : Expression.Subtract(left, right);
            if (StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Multiply"))
                return Checked ? Expression.MultiplyChecked(left, right) : Expression.Multiply(left, right);
            if (StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Divide"))
                return Expression.Divide(left, right);
            return null;
        }

        private Expression GenerateCompareTo(IExpressionGenerationRequest request, Expression left, Expression right) {
            Contract.Requires(null != request);
            Contract.Requires(null != left);
            Contract.Requires(null != right);
            if (left.IsMemoryLocationOrConstant() && right.IsMemoryLocationOrConstant()) {
                var eq = request.TopLevelGenerator.GenerateOrThrow("EQUAL", left, right);
                var less = request.TopLevelGenerator.GenerateOrThrow("LESS", left, right);
                Contract.Assume(typeof(IComparable<>).GetGenericArguments() != null);
                Contract.Assume(typeof(IComparable<>).GetGenericArguments().Length == 1);
                Contract.Assume(typeof(IComparable<>).IsGenericTypeDefinition);
                var comparableType = typeof(IComparable<>).MakeGenericType(right.Type);
                Contract.Assume(comparableType != null);

                if (left.Type.ImplementsInterface(comparableType)) {
                    var method = comparableType.GetPublicInstanceInvokableMethod(
                        "CompareTo",
                        new[] {right.Type}
                    );
                    Contract.Assume(method != null);
                    return method.BuildInstanceCallExpression(left, right);
                }

                return Expression.Condition(
                    eq,
                    Expression.Constant(0),
                    Expression.Condition(
                        less,
                        Expression.Constant(-1),
                        Expression.Constant(1)
                    )
                );
            }

            return new BlockExpressionBuilder().AddUsingMemoryLocationsOrConstants(locals => new[] {
                GenerateCompareTo(request, locals[0], locals[1])
            }, left, right).GetExpression();
        }

        private bool TryGetDoubleConstantValue(string expressionName, out double constantValue) {
            constantValue = 0;
            if (String.IsNullOrEmpty(expressionName))
                return false;

            if (expressionName.Equals("ONE", StringComparison.OrdinalIgnoreCase)) {
                constantValue = 1;
                return true;
            }
            if (expressionName.Equals("TWO", StringComparison.OrdinalIgnoreCase)) {
                constantValue = 2;
                return true;
            }
            if (expressionName.Equals("ZERO", StringComparison.OrdinalIgnoreCase)) {
                constantValue = 0;
                return true;
            }
            if (expressionName.Equals("PI", StringComparison.OrdinalIgnoreCase)) {
                constantValue = Math.PI;
                return true;
            }
            if (expressionName.Equals("HALFPI", StringComparison.OrdinalIgnoreCase)) {
                constantValue = Math.PI / 2.0;
                return true;
            }
            if (expressionName.Equals("QUARTERPI", StringComparison.OrdinalIgnoreCase)) {
                constantValue = Math.PI / 4.0;
                return true;
            }
            if (expressionName.Equals("E", StringComparison.OrdinalIgnoreCase)) {
                constantValue = Math.E;
                return true;
            }
            if (
                expressionName.Equals("UNDEFINED", StringComparison.OrdinalIgnoreCase)
                || expressionName.Equals("INVALID", StringComparison.OrdinalIgnoreCase)
            ) {
                constantValue = Double.NaN;
                return true;
            }
            return Double.TryParse(expressionName, out constantValue);
        }

        /// <summary>
        /// True when the expressions generated will use checked versions of operations when possible.
        /// </summary>
        public bool Checked { get; private set; }

        private Expression GenerateConstantExpression(string expressionName, Type resultType) {
            if (String.IsNullOrEmpty(expressionName)) throw new ArgumentException("expressionName is not valid", "expressionName");
            if (null == resultType) throw new ArgumentNullException("resultType");
            Contract.EndContractBlock();

            double constantValue;
            if (!TryGetDoubleConstantValue(expressionName, out constantValue))
                return null;

            if (resultType == typeof(double)) {
                return Expression.Constant(constantValue);
            }
            if (resultType == typeof(float)) {
                if (Double.IsNaN(constantValue))
                    return Expression.Constant(Single.NaN);
                return Expression.Constant(unchecked((float)constantValue));
            }
            if (resultType == typeof(decimal)) {
                if (Double.IsNaN(constantValue)) {
                    return null;
                }
                return Expression.Constant(unchecked((decimal)constantValue));
            }
            if (resultType == typeof(int)) {
                if (Double.IsNaN(constantValue))
                    return Expression.Constant(0);
                return Expression.Constant(unchecked((int)constantValue));
            }
            if (Double.IsNaN(constantValue))
                return null;
            return GenerateConversionExpression(Expression.Constant(constantValue), resultType);
        }

        private Expression GenerateStandardExpression(IExpressionGenerationRequest expressionRequest) {
            Contract.Requires(null != expressionRequest);

            var parameters = expressionRequest.InputExpressions;
            Contract.Assume(Contract.ForAll(parameters, x => x != null));
            var expressionName = expressionRequest.ExpressionName;

            if (parameters.Count == 1) {
                Func<IExpressionGenerationRequest, Expression, Expression> unaryGenerator;
                if (_standardUnaryExpressionGeneratorLookup.TryGetValue(expressionName, out unaryGenerator)) {
                    Contract.Assume(unaryGenerator != null);
                    return unaryGenerator(expressionRequest, parameters[0]);
                }
            }
            if (parameters.Count == 2) {
                Func<IExpressionGenerationRequest, Expression, Expression, Expression> binaryGenerator;
                if (_standardBinaryExpressionGeneratorLookup.TryGetValue(expressionName, out binaryGenerator)) {
                    Contract.Assume(binaryGenerator != null);
                    return binaryGenerator(expressionRequest, parameters[0], parameters[1]);
                }
            }
            if (parameters.Count > 2) {
                var result = GenerateArithmetic(expressionRequest, parameters);
                if (null != result)
                    return result;
            }

            return null;
        }

        private Expression GenerateConversionExpression(Expression expression, Type resultType) {
            Contract.Requires(null != expression);
            Contract.Requires(null != resultType);
            Contract.Ensures(Contract.Result<Expression>() != null);

            if (expression.Type == resultType)
                return expression;
            return Checked
                ? Expression.ConvertChecked(expression, resultType)
                : Expression.Convert(expression, resultType);
        }

        /// <inheritdoc/>
        public Expression Generate(IExpressionGenerationRequest request) {
            if (null == request) throw new ArgumentNullException("request");
            if (String.IsNullOrEmpty(request.ExpressionName)) throw new ArgumentException("Invalid request expression name.", "request");
            Contract.EndContractBlock();

            var expressionName = request.ExpressionName;

            if (
                "CONVERT".Equals(expressionName, StringComparison.OrdinalIgnoreCase)
                && request.InputExpressions.Count == 1
                && null != request.DesiredResultType
            ) {
                Contract.Assume(request.InputExpressions[0] != null);
                return GenerateConversionExpression(request.InputExpressions[0], request.DesiredResultType);
            }

            if (
                request.InputExpressions.Count == 0
                && null != request.DesiredResultType
                && typeof(void) != request.DesiredResultType
            ) {
                return GenerateConstantExpression(expressionName, request.DesiredResultType);
            }

            return GenerateStandardExpression(request);
        }

    }

}
