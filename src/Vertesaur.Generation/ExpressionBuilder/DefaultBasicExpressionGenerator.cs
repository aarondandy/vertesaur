// ===============================================================================
//
// Copyright (c) 2011,2012 Aaron Dandy 
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Vertesaur.Generation.Contracts;

namespace Vertesaur.Generation.ExpressionBuilder
{

	/// <summary>
	/// Provides default operations for the standard .NET numerical value types.
	/// </summary>
	public class DefaultBasicExpressionGenerator : IBasicExpressionGenerator {

		/// <summary>
		/// These methods are used via reflection.
		/// </summary>
		private static class SpecializedOperations {

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

		/// <summary>
		/// The default unchecked expression generator.
		/// </summary>
		public static DefaultBasicExpressionGenerator Default { get; private set; }
		/// <summary>
		/// The default checked expression generator.
		/// </summary>
		public static DefaultBasicExpressionGenerator Checked { get; private set; }

		static DefaultBasicExpressionGenerator() {
			Default = new DefaultBasicExpressionGenerator();
			Checked = new DefaultBasicExpressionGenerator(true);
		}

		private readonly bool _checked;
		[NotNull] private readonly Func<Expression, Type, UnaryExpression> _conversionGenerator; 

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks>
		/// Operations that can be, are non-checked operations by default.
		/// </remarks>
		public DefaultBasicExpressionGenerator() : this(false) { }

		/// <summary>
		/// Constructs a new operation provider.
		/// </summary>
		/// <param name="isChecked">Flag to determine if operations that can be, are checked operations.</param>
		public DefaultBasicExpressionGenerator(bool isChecked) {
			_checked = isChecked;
			_conversionGenerator = _checked ? (Func<Expression, Type, UnaryExpression>)Expression.ConvertChecked : Expression.Convert;
		}

		/// <summary>
		/// True when the expressions generated will use checked versions of operations when possible.
		/// </summary>
		public bool IsChecked { get { return _checked; } }

		private Expression CreateConstantFromDouble(double value, Type constantType) {
			Contract.Requires(null != constantType);
			Contract.EndContractBlock();

			if (typeof(double) == constantType)
				return Expression.Constant(value);
			if (typeof(int) == constantType)
				return Expression.Constant((int)value);
			return GetUnaryExpression(BasicUnaryOperationType.Convert, constantType, Expression.Constant(value));
		}

		/// <inheritdoc/>
		public Expression GetConstantExpression(BasicConstantOperationType operationType, Type constantType) {
			if(null == constantType) throw new ArgumentNullException("constantType");
			Contract.EndContractBlock();

			switch(operationType) {
			case BasicConstantOperationType.ValueZero:
				return CreateConstantFromDouble(0, constantType);
			case BasicConstantOperationType.ValueOne:
				return CreateConstantFromDouble(1, constantType);
			case BasicConstantOperationType.ValueTwo:
				return CreateConstantFromDouble(2, constantType);
			case BasicConstantOperationType.ValuePi:
				return CreateConstantFromDouble(Math.PI, constantType);
			case BasicConstantOperationType.ValueE:
				return CreateConstantFromDouble(Math.E, constantType);
			default:
				return null;
			}
		}

		/// <inheritdoc/>
		public Expression GetUnaryExpression(BasicUnaryOperationType operationType, Type resultType, Expression input) {
			if(null == input) throw new ArgumentNullException("input");
			if(null == resultType) throw new ArgumentNullException("resultType");
			Contract.EndContractBlock();
			switch (operationType) {
			case BasicUnaryOperationType.Convert:
				return resultType == input.Type
					? input
					: _conversionGenerator(input, resultType);
			case BasicUnaryOperationType.Negate:
				if (resultType == typeof(ulong) || resultType == typeof(uint) || resultType == typeof(ushort) || resultType == typeof(byte) || resultType == typeof(sbyte)) {
					var zeroConstant = GetConstantExpression(BasicConstantOperationType.ValueZero, resultType);
					Contract.Assume(null != zeroConstant);
					return GetBinaryExpression(BasicBinaryOperationType.Subtract, resultType, zeroConstant, input);
				}
				return _checked ? Expression.NegateChecked(input) : Expression.Negate(input);
			case BasicUnaryOperationType.SquareRoot:
				return new SquareRootExpression(input);
			case BasicUnaryOperationType.Square:
				return new SquareExpression(input, this);
			default:
				return null;
			}
		}

		/// <inheritdoc/>
		public Expression GetBinaryExpression(BasicBinaryOperationType operationType, Type resultType, Expression leftHandSide, Expression rightHandSide) {
			if(null == leftHandSide) throw new ArgumentNullException("leftHandSide");
			if(null == rightHandSide) throw new ArgumentNullException("rightHandSide");
			Contract.EndContractBlock();
			// special case for operations that need to be defined manually
			if (
				(resultType == typeof(byte) || resultType == typeof(char) || resultType == typeof(sbyte))
				&& resultType == leftHandSide.Type && resultType == rightHandSide.Type
			) {
				string methodName;
				switch (operationType) {
					case BasicBinaryOperationType.Add:
						methodName = "Add";
						break;
					case BasicBinaryOperationType.Subtract:
						methodName = "Subtract";
						break;
					case BasicBinaryOperationType.Multiply:
						methodName = "Multiply";
						break;
					default:
						methodName = null;
						break;
				}
				
				if(!String.IsNullOrEmpty(methodName)) {
					methodName += _checked ? "Checked" : "Unchecked";
					return Expression.Call(
						typeof(SpecializedOperations).GetMethod(
							methodName,
							BindingFlags.Static | BindingFlags.Public,
							null, new[] { leftHandSide.Type, rightHandSide.Type }, null
						),
						leftHandSide, rightHandSide
					);
				}
			}

			switch(operationType) {

			// arithmetic
			case BasicBinaryOperationType.Add:
				return _checked
					? Expression.AddChecked(leftHandSide, rightHandSide)
					: Expression.Add(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Subtract:
				return _checked
					? Expression.SubtractChecked(leftHandSide, rightHandSide)
					: Expression.Subtract(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Multiply:
				return _checked
					? Expression.MultiplyChecked(leftHandSide, rightHandSide)
					: Expression.Multiply(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Divide:
				return Expression.Divide(leftHandSide, rightHandSide);

			// comparison
			case BasicBinaryOperationType.Equal:
				return Expression.Equal(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.NotEqual:
				return Expression.NotEqual(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Less:
				return Expression.LessThan(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.LessEqual:
				return Expression.LessThanOrEqual(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Greater:
				return Expression.GreaterThan(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.GreaterEqual:
				return Expression.GreaterThanOrEqual(leftHandSide, rightHandSide);
			case BasicBinaryOperationType.Min:
				// TODO: would like a better way to determine if an expression can be easily evaluated multiple times
				if ((leftHandSide is ConstantExpression || leftHandSide is ParameterExpression) && (rightHandSide is ConstantExpression || rightHandSide is ParameterExpression)) {
					var leq = GetBinaryExpression(BasicBinaryOperationType.LessEqual, typeof(bool), leftHandSide, rightHandSide);
					Contract.Assume(null != leq);
					return Expression.Condition(leq, leftHandSide, rightHandSide);
				}
				return null; // TODO: need to create local variables, then compare
			case BasicBinaryOperationType.Max:
				// TODO: would like a better way to determine if an expression can be easily evaluated multiple times
				if ((leftHandSide is ConstantExpression || leftHandSide is ParameterExpression) && (rightHandSide is ConstantExpression || rightHandSide is ParameterExpression)) {
					var geq = GetBinaryExpression(BasicBinaryOperationType.GreaterEqual, typeof(bool), leftHandSide, rightHandSide);
					Contract.Assume(null != geq);
					return Expression.Condition(geq, leftHandSide, rightHandSide);
				}
				return null; // TODO: need to create local variables, then compare
			case BasicBinaryOperationType.CompareTo:
				// TODO: should make sure this handles nulls for both generation paths
				// ReSharper disable PossiblyMistakenUseOfParamsMethod
				var eq = GetBinaryExpression(BasicBinaryOperationType.Equal, typeof(bool), leftHandSide, rightHandSide);
				Contract.Assume(null != eq);
				var less = GetBinaryExpression(BasicBinaryOperationType.Less, typeof(bool), leftHandSide, rightHandSide);
				Contract.Assume(null != less);
				var comparableType = typeof(IComparable<>).MakeGenericType(new[] {rightHandSide.Type});
				return leftHandSide.Type.GetInterfaces().Contains(comparableType)
					? Expression.Call(
						leftHandSide,
						comparableType.GetMethod(
							"CompareTo",
							BindingFlags.Public | BindingFlags.Instance,
							null, new [] { rightHandSide.Type }, null
						),
						rightHandSide
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
				// ReSharper restore PossiblyMistakenUseOfParamsMethod
			default:
				return null;
			}
		}
	}
}
