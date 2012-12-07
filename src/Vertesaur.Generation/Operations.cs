// ===============================================================================
//
// Copyright (c) 2011 Aaron Dandy 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Vertesaur.Generation.Contracts;
using Vertesaur.Generation.ExpressionBuilder;

namespace Vertesaur.Generation
{

	/// <summary>
	/// Defines and creates multiple operations that can be executed for generic types.
	/// </summary>
	/// <typeparam name="TValue">The generic type that operations must be performed on.</typeparam>
	public class Operations<TValue> : IGenericOperationProvider
	{

		/// <summary>
		/// Various delegate definitions.
		/// </summary>
		public static class Delegates {

			/// <summary>
			/// A function with the components of two 2D points.
			/// </summary>
			/// <param name="x1">X-coordinate of the first point.</param>
			/// <param name="y1">Y-coordinate of the first point.</param>
			/// <param name="x2">X-coordinate of the second point.</param>
			/// <param name="y2">Y-coordinate of the second point.</param>
			/// <returns>A result.</returns>
			public delegate TValue TwoPoint2(TValue x1, TValue y1, TValue x2, TValue y2);

			/// <summary>
			/// A function with the components of two 3D points.
			/// </summary>
			/// <param name="x1">X-coordinate of the first point.</param>
			/// <param name="y1">Y-coordinate of the first point.</param>
			/// <param name="z1">Z-coordinate of the first point.</param>
			/// <param name="x2">X-coordinate of the second point.</param>
			/// <param name="y2">Y-coordinate of the second point.</param>
			/// <param name="z2">Z-coordinate of the second point.</param>
			/// <returns>A result.</returns>
			public delegate TValue TwoPoint3(TValue x1, TValue y1, TValue z1, TValue x2, TValue y2, TValue z2);
		}

		private static readonly ParameterExpression DoubleParam = Expression.Parameter(typeof(double), "dParam");
		private static readonly ParameterExpression IntParam = Expression.Parameter(typeof(int), "iParam");
		private static readonly ParameterExpression TParam = Expression.Parameter(typeof(TValue), "tParam");

		/// <summary>
		/// Converts the given value to a <see cref="System.Double"/> value.
		/// </summary>
		[NotNull] public readonly Func<TValue, double> ConvertToDouble;
		/// <summary>
		/// Converts the given value from <see cref="System.Double"/> to the generic type.
		/// </summary>
		[NotNull] public readonly Func<double, TValue> ConvertFromDouble;
		/// <summary>
		/// Converts the given value to a <see cref="System.Int32"/> value.
		/// </summary>
		[NotNull] public readonly Func<TValue, int> ConvertToInt;
		/// <summary>
		/// Converts the given value from <see cref="System.Int32"/> to the generic type.
		/// </summary>
		[NotNull] public readonly Func<int, TValue> ConvertFromInt;

		[NotNull] private readonly IGenericOperationProvider _operationProvider;

		/// <summary>
		/// Provides operations based on the given operation providers.
		/// </summary>
		/// <param name="operationProviders">The operation providers.</param>
		public Operations([NotNull] IEnumerable<IGenericOperationProvider> operationProviders)
			: this(new CombinedGenericOperationProvider(operationProviders))
		{
			Contract.Requires(null != operationProviders);
			Contract.EndContractBlock();
		}

		/// <summary>
		/// Provides operations based on the given operation provider.
		/// </summary>
		/// <param name="operationProvider">The operation provider.</param>
		public Operations(IGenericOperationProvider operationProvider) {
			if(null == operationProvider) throw new ArgumentNullException("operationProvider");
			Contract.EndContractBlock();
			_operationProvider = operationProvider;
			ConvertFromDouble = Expression.Lambda<Func<double,TValue>>(
				_operationProvider.GetUnaryExpression(DoubleParam, GenericUnaryOperationType.ConvertFromDouble)
				?? Expression.Convert(DoubleParam, typeof(TValue))
			).Compile();
			ConvertToDouble = Expression.Lambda<Func<TValue, double>>(
				_operationProvider.GetUnaryExpression(TParam, GenericUnaryOperationType.ConvertToDouble)
				?? Expression.Convert(TParam, typeof(double))
			).Compile();
			ConvertFromInt = Expression.Lambda<Func<int, TValue>>(
				_operationProvider.GetUnaryExpression(IntParam, GenericUnaryOperationType.ConvertFromInt)
				?? Expression.Convert(IntParam, typeof(TValue))
			).Compile();
			ConvertToInt = Expression.Lambda<Func<TValue, int>>(
				_operationProvider.GetUnaryExpression(TParam, GenericUnaryOperationType.ConvertToInt)
				?? Expression.Convert(TParam, typeof(int))
			).Compile();
		}

		public Expression GetConstantExpression(GenericConstantOperationType operationType) {
			return _operationProvider.GetConstantExpression(operationType);
		}

		public Expression GetUnaryExpression(Expression input, GenericUnaryOperationType operationType) {
			if(null == input) throw new ArgumentNullException("input");
			Contract.EndContractBlock();
			return _operationProvider.GetUnaryExpression(input, operationType);
		}

		public Expression GetBinaryExpression(Expression leftHandSide, Expression rightHandSide, GenericBinaryOperationType operationType) {
			if(null == leftHandSide) throw new ArgumentNullException("leftHandSide");
			if(null == rightHandSide) throw new ArgumentNullException("rightHandSide");
			Contract.EndContractBlock();
			return _operationProvider.GetBinaryExpression(leftHandSide, rightHandSide, operationType);
		}

	}
}

