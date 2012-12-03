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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// Provides default operations for the standard .NET numerical value types.
	/// </summary>
	/// <typeparam name="TValue">The type to provide operations for.</typeparam>
	public class DefaultGenericOperationProvider<TValue> : IGenericOperationProvider {

		/// <summary>
		/// These methods are used via reflection.
		/// </summary>
		private static class DefaultGenericOperationProviderUtility {

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
// ReSharper restore UnusedMember.Local

		}

		private readonly Type _type;
		private readonly bool _checked;
		private readonly Func<double, TValue> _convertFromDouble;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// Operations that can be, are checked operations by default.
		/// </remarks>
		public DefaultGenericOperationProvider() : this(true) { }

		/// <summary>
		/// Constructs a new operation provider.
		/// </summary>
		/// <param name="isChecked">Flag to determine if operations that can be, are checked operations.</param>
		public DefaultGenericOperationProvider(bool isChecked) {
			_type = typeof (TValue);
			var valueParameter = Expression.Parameter(typeof (double), "dVal");
			var conversionExpression = GetUnaryExpression(valueParameter, GenericUnaryOperationType.ConvertFromDouble);
			_convertFromDouble = Expression.Lambda<Func<double, TValue>>(conversionExpression, valueParameter).Compile();
			_checked = isChecked;
		}

		/// <inheritdoc/>
		public Expression GetConstantExpression(GenericConstantOperationType operationType) {
			switch(operationType) {
			case GenericConstantOperationType.ValueZero:
				return Expression.Constant(_convertFromDouble(0));
			case GenericConstantOperationType.ValueOne:
				return Expression.Constant(_convertFromDouble(1));
			case GenericConstantOperationType.ValueTwo:
				return Expression.Constant(_convertFromDouble(2));
			case GenericConstantOperationType.ValuePi:
				return Expression.Constant(_convertFromDouble(Math.PI));
			case GenericConstantOperationType.ValueE:
				return Expression.Constant(_convertFromDouble(Math.E));
			default:
				return null;
			}
		}

		/// <inheritdoc/>
		public Expression GetUnaryExpression(Expression input, GenericUnaryOperationType operationType) {
			switch (operationType) {
			case GenericUnaryOperationType.ConvertToDouble:
				return typeof(TValue) == typeof(double) ? input : Expression.Convert(input, typeof(double));
			case GenericUnaryOperationType.ConvertFromDouble:
				return typeof(TValue) == typeof(double) ? input : Expression.Convert(input, typeof(TValue));
			case GenericUnaryOperationType.ConvertToInt:
				return typeof(TValue) == typeof(int) ? input : Expression.Convert(input, typeof(int));
			case GenericUnaryOperationType.ConvertFromInt:
				return typeof(TValue) == typeof(int) ? input : Expression.Convert(input, typeof(TValue));
			case GenericUnaryOperationType.Negate:
				return (_type == typeof(ulong) || _type == typeof(uint) || _type == typeof(ushort) || _type == typeof(byte) || _type == typeof(sbyte))
					? GetBinaryExpression(GetConstantExpression(GenericConstantOperationType.ValueZero), input, GenericBinaryOperationType.Subtract)
					: (_checked ? Expression.NegateChecked(input) : Expression.Negate(input))
				;
			default:
				return null;
			}
		}

		/// <inheritdoc/>
		public Expression GetBinaryExpression(Expression leftHandSide, Expression rightHandSide, GenericBinaryOperationType operationType) {

			// special case for operations that need to be defined manually
			if (_type == typeof(byte) || _type == typeof(char) || _type == typeof(sbyte)) {
				string methodName;
				switch (operationType) {
					case GenericBinaryOperationType.Add:
						methodName = "Add";
						break;
					case GenericBinaryOperationType.Subtract:
						methodName = "Subtract";
						break;
					case GenericBinaryOperationType.Multiply:
						methodName = "Multiply";
						break;
					default:
						methodName = null;
						break;
				}
				
				if(!String.IsNullOrEmpty(methodName)) {
					var typeList = new[] { _type, _type };
					methodName += _checked ? "Checked" : "Unchecked";
					return Expression.Call(
						typeof(DefaultGenericOperationProviderUtility).GetMethod(
							methodName,
							BindingFlags.Static | BindingFlags.Public,
							null, typeList, null
						),
						leftHandSide, rightHandSide
					);
				}
			}

			switch(operationType) {

			// arithmetic
			case GenericBinaryOperationType.Add:
				return _checked
					? Expression.AddChecked(leftHandSide, rightHandSide)
					: Expression.Add(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Subtract:
				return _checked
					? Expression.SubtractChecked(leftHandSide, rightHandSide)
					: Expression.Subtract(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Multiply:
				return _checked
					? Expression.MultiplyChecked(leftHandSide, rightHandSide)
					: Expression.Multiply(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Divide:
				return Expression.Divide(leftHandSide, rightHandSide);

			// comparison
			case GenericBinaryOperationType.Equal:
				return Expression.Equal(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.NotEqual:
				return Expression.NotEqual(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Less:
				return Expression.LessThan(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.LessEqual:
				return Expression.LessThanOrEqual(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Greater:
				return Expression.GreaterThan(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.GreaterEqual:
				return Expression.GreaterThanOrEqual(leftHandSide, rightHandSide);
			case GenericBinaryOperationType.Min:
				return ((leftHandSide is ConstantExpression || leftHandSide is ParameterExpression) && (rightHandSide is ConstantExpression || rightHandSide is ParameterExpression))
					? Expression.Condition(GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.LessEqual), leftHandSide, rightHandSide)
					: null;
			case GenericBinaryOperationType.Max:
				return ((leftHandSide is ConstantExpression || leftHandSide is ParameterExpression) && (rightHandSide is ConstantExpression || rightHandSide is ParameterExpression))
					? Expression.Condition(GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.GreaterEqual), leftHandSide, rightHandSide)
					: null;
			case GenericBinaryOperationType.CompareTo:
				return _type.GetInterfaces().Contains(typeof(IComparable<TValue>))
					? Expression.Call(
						leftHandSide,
						typeof(TValue).GetMethod(
							"CompareTo",
							BindingFlags.Public | BindingFlags.Instance,
							null, new [] { _type }, null
						),
						rightHandSide
					) as Expression
					: Expression.Condition(
						GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Equal),
						Expression.Constant(0),
						Expression.Condition(
							GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Less),
							Expression.Constant(-1),
							Expression.Constant(1)
						)
					);
			default:
				return null;
			}
		}
	}
}
