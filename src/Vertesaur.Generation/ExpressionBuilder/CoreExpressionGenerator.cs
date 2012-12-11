using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// The core expression generator for basic arithmetic and comparisons.
	/// </summary>
	[Export(typeof(IExpressionGenerator))]
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
				{"NEGATE", GenerateNegation}
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
				{"MIN", GenerateMin},
				{"MAX", GenerateMax},
				{"COMPARETO", GenerateCompareTo}
			};
		}

		private Expression GenerateSquare(IExpressionGenerationRequest request, Expression parameter) {
			Contract.Requires(null != parameter);
			Contract.EndContractBlock();
			return new SquareExpression(parameter, request.TopLevelGenerator);
		}

		private Expression GenerateSquareRoot(IExpressionGenerationRequest request, Expression parameter) {
			Contract.Requires(null != parameter);
			Contract.EndContractBlock();
			return new SquareRootExpression(parameter, request.TopLevelGenerator);
		}

		private Expression GenerateNegation(IExpressionGenerationRequest request, Expression parameter) {
			var resultType = parameter.Type;
			if (resultType == typeof(ulong) || resultType == typeof(uint) || resultType == typeof(ushort) || resultType == typeof(byte) || resultType == typeof(sbyte)) {
				var zeroConstant = GenerateConstantExpression("0", resultType);
				Contract.Assume(null != zeroConstant);
				return request.TopLevelGenerator.GenerateExpression("Subtract", zeroConstant, parameter);
			}
			return Checked ? Expression.NegateChecked(parameter) : Expression.Negate(parameter);
		}

		private static string ToTitleCase(string text){
			if (String.IsNullOrEmpty(text))
				return text;
			return String.Concat(
				Char.ToUpperInvariant(text[0]),
				text.Substring(1).ToLowerInvariant()
			);
		}

		private Expression GenerateArithmetic(IExpressionGenerationRequest request, Expression left, Expression right) {
			if ((left.Type == right.Type) && (left.Type == typeof(byte) || left.Type == typeof(char) || left.Type == typeof(sbyte))) {
				return Expression.Call(
					typeof(SpecializedOperations).GetMethod(
						ToTitleCase(request.ExpressionName) + (Checked ? "Checked" : "Unchecked"),
						BindingFlags.Static | BindingFlags.Public,
						null, new[] { left.Type, left.Type }, null
					),
					left, right
				);
			}

			if(StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Add"))
				return Checked ? Expression.AddChecked(left, right) : Expression.Add(left, right);
			if(StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Subtract"))
				return Checked ? Expression.SubtractChecked(left, right) : Expression.Subtract(left, right);
			if(StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Multiply"))
				return Checked ? Expression.MultiplyChecked(left, right) : Expression.Multiply(left, right);
			if (StringComparer.OrdinalIgnoreCase.Equals(request.ExpressionName, "Divide"))
				return Expression.Divide(left, right);
			return null;
		}

		private Expression GenerateMin(IExpressionGenerationRequest request, Expression left, Expression right) {
			if ((left is ConstantExpression || left is ParameterExpression) && (right is ConstantExpression || right is ParameterExpression)) {
				var leq = request.TopLevelGenerator.GenerateExpression("LESSEQUAL", left, right);
				Contract.Assume(null != leq);
				return Expression.Condition(leq, left, right);
			}

			var tempLeft = Expression.Parameter(left.Type);
			var tempRight = Expression.Parameter(right.Type);
			return Expression.Block(
				new[]{tempLeft, tempRight},
				Expression.Assign(tempLeft, left),
				Expression.Assign(tempRight, right),
				GenerateMin(request, tempLeft, tempRight)
			);
		}

		private Expression GenerateMax(IExpressionGenerationRequest request, Expression left, Expression right) {
			if ((left is ConstantExpression || left is ParameterExpression) && (right is ConstantExpression || right is ParameterExpression)) {
				var geq = request.TopLevelGenerator.GenerateExpression("GREATEREQUAL", left, right);
				Contract.Assume(null != geq);
				return Expression.Condition(geq, left, right);
			}

			var tempLeft = Expression.Parameter(left.Type);
			var tempRight = Expression.Parameter(right.Type);
			return Expression.Block(
				new[] { tempLeft, tempRight },
				Expression.Assign(tempLeft, left),
				Expression.Assign(tempRight, right),
				GenerateMax(request, tempLeft, tempRight)
			);
		}

		private Expression GenerateCompareTo(IExpressionGenerationRequest request, Expression left, Expression right) {
			if ((left is ConstantExpression || left is ParameterExpression) && (right is ConstantExpression || right is ParameterExpression)) {
				var eq = request.TopLevelGenerator.GenerateExpression("EQUAL", left, right);
				Contract.Assume(null != eq);
				var less = request.TopLevelGenerator.GenerateExpression("LESS", left, right);
				Contract.Assume(null != less);
				var comparableType = typeof(IComparable<>).MakeGenericType(new[] { right.Type });
				return left.Type.GetInterfaces().Contains(comparableType)
					? Expression.Call(
						left,
						comparableType.GetMethod(
							"CompareTo",
							BindingFlags.Public | BindingFlags.Instance,
							null, new[] { right.Type }, null
						),
						right
					) as Expression
					: Expression.Condition(
						eq,
						Expression.Constant(0),
						Expression.Condition(
							less,
							Expression.Constant(-1),
							Expression.Constant(1)
						)
					);
			}

			var tempLeft = Expression.Parameter(left.Type);
			var tempRight = Expression.Parameter(right.Type);
			return Expression.Block(
				new[] { tempLeft, tempRight },
				Expression.Assign(tempLeft, left),
				Expression.Assign(tempRight, right),
				GenerateCompareTo(request, tempLeft, tempRight)
			);
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
			if (expressionName.Equals("E", StringComparison.OrdinalIgnoreCase)) {
				constantValue = Math.E;
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

			if (resultType == typeof (double))
				return Expression.Constant(constantValue);
			if (resultType == typeof(float))
				return Expression.Constant(unchecked((float)constantValue));
			if (resultType == typeof(decimal))
				return Expression.Constant(unchecked((decimal)constantValue));
			if (resultType == typeof (int))
				return Expression.Constant(unchecked((int) constantValue));
			return GenerateConversionExpression(Expression.Constant(constantValue), resultType);
		}

		private Expression GenerateStandardExpression(IExpressionGenerationRequest expressionRequest) {
			Contract.Requires(null != expressionRequest);
			Contract.EndContractBlock();
			var parameters = expressionRequest.InputExpressions;
			var expressionName = expressionRequest.ExpressionName;

			if (parameters.Count == 1){
				Func<IExpressionGenerationRequest, Expression, Expression> unaryGenerator;
				if (_standardUnaryExpressionGeneratorLookup.TryGetValue(expressionName, out unaryGenerator))
					return unaryGenerator(expressionRequest, parameters[0]);
			}
			if (parameters.Count == 2) {
				Func<IExpressionGenerationRequest, Expression, Expression, Expression> binaryGenerator;
				if (_standardBinaryExpressionGeneratorLookup.TryGetValue(expressionName, out binaryGenerator))
					return binaryGenerator(expressionRequest, parameters[0], parameters[1]);
			}
			return null;
		}

		private Expression GenerateConversionExpression(Expression expression, Type resultType){
			Contract.Requires(null != expression);
			Contract.Requires(null != resultType);
			Contract.Requires(typeof(void) != resultType);
			Contract.EndContractBlock();

			if (expression.Type == resultType)
				return expression;
			return Checked
				? Expression.ConvertChecked(expression, resultType)
				: Expression.Convert(expression, resultType);
		}

		/// <inheritdoc/>
		public Expression GenerateExpression(IExpressionGenerationRequest request) {
			if(null == request) throw new ArgumentNullException("request");
			if(String.IsNullOrEmpty(request.ExpressionName)) throw new ArgumentException("Invalid request expression name.", "request");
			Contract.EndContractBlock();

			var expressionName = request.ExpressionName;

			if ("CONVERT".Equals(expressionName, StringComparison.OrdinalIgnoreCase)
				&& request.InputExpressions != null
				&& request.InputExpressions.Count == 1
				&& null != request.DesiredResultType
			){
				return GenerateConversionExpression(request.InputExpressions[0], request.DesiredResultType);
			}

			if (
				(request.InputExpressions == null || request.InputExpressions.Count == 0)
				&& null != request.DesiredResultType
				&& typeof (void) != request.DesiredResultType
			){
				return GenerateConstantExpression(expressionName, request.DesiredResultType);
			}

			return GenerateStandardExpression(request);
		}
	}

}
