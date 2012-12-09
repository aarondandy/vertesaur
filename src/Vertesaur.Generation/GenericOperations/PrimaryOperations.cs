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

namespace Vertesaur.Generation.GenericOperations
{

	/// <summary>
	/// Defines and creates multiple operations that can be executed for generic types.
	/// </summary>
	/// <typeparam name="TValue">The generic type that operations must be performed on.</typeparam>
	public class PrimaryOperations<TValue> :
		IEqualityComparer<TValue>
	{

		/// <summary>
		/// The default generic operation provider.
		/// </summary>
		[NotNull] public static PrimaryOperations<TValue> Default { get; private set; }

		static PrimaryOperations() {
			var defaultExpressionGenerator = DefaultBasicExpressionGenerator.Default;
			Contract.Assume(null != defaultExpressionGenerator);
			Default = new PrimaryOperations<TValue>(defaultExpressionGenerator);
		}

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
		/// <summary>
		/// Determines if two values are equal.
		/// </summary>
		[NotNull] public readonly Func<TValue, TValue, bool> EqualsTest;

		[NotNull] private readonly IBasicExpressionGenerator _operationProvider;
		[CanBeNull] private readonly Func<TValue,int> _hashCode;

		/// <summary>
		/// Provides operations based on the given operation providers.
		/// </summary>
		/// <param name="operationProviders">The operation providers.</param>
		public PrimaryOperations([NotNull] IEnumerable<IBasicExpressionGenerator> operationProviders)
			: this(new CombinedBasicExpressionGenerator(operationProviders))
		{
			Contract.Requires(null != operationProviders);
			Contract.EndContractBlock();
		}

		/// <summary>
		/// Provides operations based on the given operation provider.
		/// </summary>
		/// <param name="operationProvider">The operation provider.</param>
		public PrimaryOperations(IBasicExpressionGenerator operationProvider) {
			if(null == operationProvider) throw new ArgumentNullException("operationProvider");
			Contract.EndContractBlock();

			var doubleParam = Expression.Parameter(typeof(double), "dParam");
			var intParam = Expression.Parameter(typeof(int), "iParam");
			var tParam0 = Expression.Parameter(typeof(TValue), "tParam0");
			var tParam1 = Expression.Parameter(typeof(TValue), "tParam1");

			_operationProvider = operationProvider;
			ConvertFromDouble = Expression.Lambda<Func<double,TValue>>(
				_operationProvider.GetUnaryExpression(BasicUnaryOperationType.Convert, typeof(TValue), doubleParam)
				?? Expression.Convert(doubleParam, typeof(TValue)),
				doubleParam
			).Compile();
			ConvertToDouble = Expression.Lambda<Func<TValue, double>>(
				_operationProvider.GetUnaryExpression(BasicUnaryOperationType.Convert, typeof(double), tParam0)
				?? Expression.Convert(tParam0, typeof(double)),
				tParam0
			).Compile();
			ConvertFromInt = Expression.Lambda<Func<int, TValue>>(
				_operationProvider.GetUnaryExpression(BasicUnaryOperationType.Convert, typeof(TValue), intParam)
				?? Expression.Convert(intParam, typeof(TValue)),
				intParam
			).Compile();
			ConvertToInt = Expression.Lambda<Func<TValue, int>>(
				_operationProvider.GetUnaryExpression(BasicUnaryOperationType.Convert, typeof(int), tParam0)
				?? Expression.Convert(tParam0, typeof(int)),
				tParam0
			).Compile();
			EqualsTest = Expression.Lambda<Func<TValue, TValue, bool>>(
				_operationProvider.GetBinaryExpression(BasicBinaryOperationType.Equal, typeof(bool), tParam0, tParam1)
				?? Expression.Equal(tParam0, tParam1),
				tParam0, tParam1
			).Compile();
			var hashCodeExpression = _operationProvider.GetUnaryExpression(BasicUnaryOperationType.HashCode, typeof(int), tParam0);
			_hashCode = null == hashCodeExpression ? null : Expression.Lambda<Func<TValue, int>>(hashCodeExpression, tParam0).Compile();
		}

		/// <inheritdoc/>
		public bool Equals(TValue x, TValue y) {
			return EqualsTest(x, y);
		}

		/// <inheritdoc/>
		public int GetHashCode(TValue obj) {
			// ReSharper disable CompareNonConstrainedGenericWithNull
			return null == _hashCode
				? (null == obj ? 0 : obj.GetHashCode())
				: _hashCode(obj);
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}
	}
}

