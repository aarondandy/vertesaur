using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Vertesaur.Generation.Contracts;
using Vertesaur.Generation.ExpressionBuilder;

namespace Vertesaur.Generation.GenericOperations
{
	/// <summary>
	/// Vector related generic operations that are generated at run-time.
	/// </summary>
	/// <typeparam name="TValue">The value type the operations are primarily for.</typeparam>
	public class VectorOperations<TValue>
	{

		static VectorOperations(){
			Default = new VectorOperations<TValue>(new MefCombinedExpressionGenerator());
		}

		public static VectorOperations<TValue> Default { get; private set; }

		public delegate TValue CoordinateToValue2D(TValue x, TValue y);

		public delegate TValue TwoCoordinateToValue2D(TValue x0, TValue y0, TValue x1, TValue y1);

		public VectorOperations(IExpressionGenerator expressionGenerator) {
			if(null == expressionGenerator) throw new ArgumentNullException("expressionGenerator");
			Contract.Requires(null != expressionGenerator);
			Contract.Ensures(null != ExpressionGenerator);
			Contract.EndContractBlock();
			ExpressionGenerator = expressionGenerator;

			Magnitude2D = CreateCoordinateToValue2D("Magnitude");
			SquaredMagnitude2D = CreateCoordinateToValue2D("SquaredMagnitude");
			DotProduct2D = CreateTwoCoordinateToValue2D("DotProduct");
		}

		private CoordinateToValue2D CreateCoordinateToValue2D(string expressionName){
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			Contract.EndContractBlock();

			var tParam0 = Expression.Parameter(typeof(TValue));
			var tParam1 = Expression.Parameter(typeof(TValue));
			return Expression.Lambda<CoordinateToValue2D>(
				ExpressionGenerator.GenerateExpression(new FunctionExpressionGenerationRequest(ExpressionGenerator,expressionName,tParam0,tParam1)),
				tParam0, tParam1
			).Compile();
		}

		private TwoCoordinateToValue2D CreateTwoCoordinateToValue2D(string expressionName) {
			Contract.Requires(!string.IsNullOrEmpty(expressionName));
			Contract.EndContractBlock();

			var tParam0 = Expression.Parameter(typeof(TValue));
			var tParam1 = Expression.Parameter(typeof(TValue));
			var tParam2 = Expression.Parameter(typeof(TValue));
			var tParam3 = Expression.Parameter(typeof(TValue));
			return Expression.Lambda<TwoCoordinateToValue2D>(
				ExpressionGenerator.GenerateExpression(new FunctionExpressionGenerationRequest(
					ExpressionGenerator, expressionName, tParam0, tParam1, tParam2, tParam3)),
				tParam0, tParam1, tParam2, tParam3
			).Compile();
		}

		/// <summary>
		/// The expression generator that was used to create the run-time executable generic functions.
		/// </summary>
		public IExpressionGenerator ExpressionGenerator { get; private set; }

		public readonly CoordinateToValue2D Magnitude2D;
		public readonly CoordinateToValue2D SquaredMagnitude2D;
		public readonly TwoCoordinateToValue2D DotProduct2D;

		[ContractInvariantMethod]
		private void CodeContractInvariants(){
			Contract.Invariant(null != ExpressionGenerator);
		}

	}
}
