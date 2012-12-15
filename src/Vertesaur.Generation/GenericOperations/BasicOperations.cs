using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;
using Vertesaur.Generation.Expressions;

namespace Vertesaur.Generation.GenericOperations
{
	/// <summary>
	/// Basic generic operations that are generated at run-time.
	/// </summary>
	/// <typeparam name="TValue">The value type the operations are primarily for.</typeparam>
	public class BasicOperations<TValue>
	{

		static BasicOperations(){
			Default = new BasicOperations<TValue>(new MefCombinedExpressionGenerator());
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

		public readonly UnaryFunc Sin;
		public readonly UnaryFunc Cos;
		public readonly UnaryFunc Tan;
		public readonly UnaryFunc Asin;
		public readonly UnaryFunc Acos;
		public readonly UnaryFunc Atan;
		public readonly UnaryFunc Sinh;
		public readonly UnaryFunc Cosh;
		public readonly UnaryFunc Tanh;
		public readonly UnaryFunc Ceiling;
		public readonly UnaryFunc Floor;
		public readonly UnaryFunc Truncate;
		public readonly UnaryFunc Log;
		public readonly UnaryFunc Log10;
		public readonly UnaryFunc Exp;
		public readonly UnaryFunc Abs;

		public readonly BinaryFunc Atan2;
		public readonly BinaryFunc Pow;
		public readonly BinaryFunc Min;
		public readonly BinaryFunc Max;

		/// <summary>
		/// Converts a generic typed value to the double type.
		/// </summary>
		public readonly Func<TValue, double> ToDouble;
		/// <summary>
		/// Convertes a double typed value to the generic type.
		/// </summary>
		public readonly Func<double, TValue> FromDouble;

		/// <summary>
		/// Creates a new basic generic operations implementation using the given expression generator.
		/// </summary>
		/// <param name="expressionGenerator">The expression generator that will be used to build the generic methods at run-time.</param>
		public BasicOperations(IExpressionGenerator expressionGenerator){
			Contract.Requires(null != expressionGenerator);
			Contract.Ensures(null != ExpressionGenerator);
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
			Log = BuildUnaryFunc("Log");
			Log10 = BuildUnaryFunc("Log10");
			Exp = BuildUnaryFunc("Exp");
			Abs = BuildUnaryFunc("Abs");
			Atan2 = BuildBinaryFunc("Atan2");
			Pow = BuildBinaryFunc("Pow");
			Ceiling = BuildUnaryFunc("Ceiling");
			Floor = BuildUnaryFunc("Floor");
			Truncate = BuildUnaryFunc("Truncate");
			Min = BuildBinaryFunc("Min");
			Max = BuildBinaryFunc("Max");

			ToDouble = BuildConversion<TValue, double>();
			FromDouble = BuildConversion<double,TValue>();
			_zeroValueGenerator = BuildConstant("0");
		}

		private Func<TFrom, TTo> BuildConversion<TFrom,TTo>() {
			var inputParam = Expression.Parameter(typeof(TFrom));
			var expression = ExpressionGenerator.GenerateConversionExpression(typeof(TTo), inputParam);
			if(null == expression)
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

		private UnaryFunc BuildUnaryFunc(string expressionName){
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			var tParam = Expression.Parameter(typeof (TValue));
			var expression = ExpressionGenerator.Generate(expressionName, tParam);
			if (null == expression)
				return null;
			return Expression.Lambda<UnaryFunc>(expression,tParam).Compile();
		}

		private BinaryFunc BuildBinaryFunc(string expressionName){
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			var tParam0 = Expression.Parameter(typeof(TValue));
			var tParam1 = Expression.Parameter(typeof(TValue));
			var expression = ExpressionGenerator.Generate(expressionName,tParam0, tParam1);
			if (null == expression)
				return null;
			return Expression.Lambda<BinaryFunc>(expression, tParam0, tParam1).Compile();
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

		[ContractInvariantMethod]
		private void CodeContractInvariants(){
			Contract.Invariant(null != ExpressionGenerator);
		}

	}
}
