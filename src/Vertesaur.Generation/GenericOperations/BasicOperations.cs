using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;
using Vertesaur.Generation.ExpressionBuilder;

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

		public static BasicOperations<TValue> Default { get; private set; }

		public delegate TValue UnaryFunc(TValue value);

		public delegate TValue BinaryFunc(TValue left, TValue right);

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
		}

		private UnaryFunc BuildUnaryFunc(string expressionName){
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			Contract.EndContractBlock();

			var tParam = Expression.Parameter(typeof (TValue));
			var expression = ExpressionGenerator.GenerateExpression(new FunctionExpressionGenerationRequest(ExpressionGenerator, expressionName, tParam));
			if (null == expression)
				return null;
			return Expression.Lambda<UnaryFunc>(expression,tParam).Compile();
		}

		private BinaryFunc BuildBinaryFunc(string expressionName){
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			Contract.EndContractBlock();

			var tParam0 = Expression.Parameter(typeof(TValue));
			var tParam1 = Expression.Parameter(typeof(TValue));
			var expression = ExpressionGenerator.GenerateExpression(new FunctionExpressionGenerationRequest(ExpressionGenerator, expressionName,tParam0, tParam1));
			if (null == expression)
				return null;
			return Expression.Lambda<BinaryFunc>(expression,tParam0, tParam1).Compile();
		}

		/// <summary>
		/// The expression generator that was used to create the run-time executable generic functions.
		/// </summary>
		public IExpressionGenerator ExpressionGenerator { get; private set; }

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

		[ContractInvariantMethod]
		private void CodeContractInvariants(){
			Contract.Invariant(null != ExpressionGenerator);
		}

	}
}
