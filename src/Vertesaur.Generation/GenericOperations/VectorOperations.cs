using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Vertesaur.Generation.ExpressionBuilder;

namespace Vertesaur.Generation.GenericOperations
{
	public class VectorOperations<TValue>
	{

		public static VectorOperations<TValue> Default { get; private set; }

		static VectorOperations() {
			Default = new VectorOperations<TValue>(PrimaryOperations<TValue>.Default);
		}

		public readonly Func<TValue, TValue, TValue> GetMagnitude;
		public readonly Func<TValue, TValue, TValue> GetSquaredMagnitude;


		public PrimaryOperations<TValue> PrimaryOperations { get; private set; } 

		public VectorOperations(PrimaryOperations<TValue> primaryOperations) {
			PrimaryOperations = primaryOperations;
			var xParam = Expression.Parameter(typeof(TValue));
			var yParam = Expression.Parameter(typeof(TValue));
			var zParam = Expression.Parameter(typeof(TValue));
			GetSquaredMagnitude = Expression.Lambda<Func<TValue, TValue, TValue>>(
				new SquaredMagnitudeExpression(
					new[] { xParam, yParam },
					PrimaryOperations.OperationProvider
				),
				xParam, yParam
			).Compile();
			GetMagnitude = Expression.Lambda<Func<TValue, TValue, TValue>>(
				new MagnitudeExpression(
					new[]{xParam,yParam},
					PrimaryOperations.OperationProvider
				),
				xParam, yParam
			).Compile();
		}

	}
}
