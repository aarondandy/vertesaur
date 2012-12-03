// ===============================================================================
//
// Copyright (c) 2010 Aaron Dandy
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
using System.Linq.Expressions;
using NUnit.Framework;
using Vertesaur.Generation.ExpressionBuilder;


namespace Vertesaur.Generation.Test {

	/// <summary>
	/// Tests for the <c>DefaultGenericOperationProvider</c>.
	/// </summary>
	[TestFixture]
	public class DefaultGenericOperationProviderTests {

// ReSharper disable InconsistentNaming
#pragma warning disable 1591

		[Test]
		public void zero_constant_is_zero() {
			constant_is(GenericConstantOperationType.ValueZero, 0);
		}

		[Test]
		public void one_constant_is_one() {
			constant_is(GenericConstantOperationType.ValueOne, 1);
		}

		[Test]
		public void two_constant_is_two() {
			constant_is(GenericConstantOperationType.ValueTwo, 2);
		}

		[Test]
		public void e_constant_is_e() {
			real_number_constant_is(GenericConstantOperationType.ValueE, Math.E);
		}

		[Test]
		public void pi_constant_is_pi() {
			constant_is(GenericConstantOperationType.ValuePi, Math.PI);
		}

		[Test]
		public void from_double_and_back_again() {
			from_double_and_back_again(new[] {
				0.0,1,2,3,4,5,6,7,8,9,10,16,32,64,127
			});
			real_number_from_double_and_back_again(new[] {
 				-65535,-0.0000000001,0,Math.E,Math.PI,9001,42
			});
		}

		[Test]
		public void from_int_and_back_again() {
			from_int_and_back_again(new[] {
				0,1,2,3,4,5,6,7,8,9,10,16,32,64,127
			});
		}

		[Test]
		public void generic_add_positive_integers() {
			generic_add(new[] { 0, 1, 9, 4 });
		}

		[Test]
		public void generic_add_real() {
			generic_add(new[] { 0.0, -1, 2.5, 4,-9001 });
		}

		[Test]
		public void generic_subtract_positive_integers() {
			generic_subtract(new[] { 0 });
		}

		[Test]
		public void generic_subtract_integers() {
			generic_subtract_signed(new[] { 0, 1, 9, 4, -1, -9, -4 });
		}

		[Test]
		public void generic_subtract_real() {
			generic_subtract(new[] { 0.0, -1, 2.5, 4, -9001 });
		}

		[Test]
		public void generic_multiply_positive_integers() {
			generic_multiply(new[] { 0 });
		}

		[Test]
		public void generic_multiply_integers() {
			generic_multiply_signed(new[] { 0, 1, 9, 4, -1, -9, -4 });
		}

		[Test]
		public void generic_multiply_real() {
			generic_multiply(new[] { 0.0, -1, 2.5, 4, -9001 });
		}

		[Test]
		public void generic_divide_positive() {
			generic_divide(new[] { 1.0, 2, 3, 8 });
		}

		[Test]
		public void generic_divide_real() {
			generic_divide(new[] { 0.5, -1, 2.5, 4, -9001 });
		}

#pragma warning restore 1591

/*
 * a list of types to ease test creation. would be nice if resharper/nunit had generic test support
 * 

double
float
int
byte
char
decimal
long
sbyte
short
uint
ulong
ushort

*/

		private void generic_add(double[] values) {
			generic_add<double>(values);
			generic_add<float>(values);
			generic_add<decimal>(values);
		}

		private void generic_add(int[] values) {
			generic_add<double>(values);
			generic_add<float>(values);
			generic_add<int>(values);
			generic_add<byte>(values);
			generic_add<char>(values);
			generic_add<decimal>(values);
			generic_add<long>(values);
			generic_add<sbyte>(values);
			generic_add<short>(values);
			generic_add<uint>(values);
			generic_add<ulong>(values);
			generic_add<ushort>(values);
		}

		private void generic_add<TValue>(int[] values) {
			foreach(var a in values) {
				foreach(var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a + b);

					var leftHandSide = Expression.Parameter(typeof (TValue), "leftHandSide");
					var rightHandSide = Expression.Parameter(typeof (TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide,rightHandSide,GenericBinaryOperationType.Add);
					var func = Expression.Lambda<Func<TValue,TValue,TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a),ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_add<TValue>(double[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a + b);

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Add);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_subtract(double[] values) {
			generic_subtract<double>(values);
			generic_subtract<float>(values);
			generic_subtract<decimal>(values);
		}

		private void generic_subtract(int[] values) {
			generic_subtract<double>(values);
			generic_subtract<float>(values);
			generic_subtract<int>(values);
			generic_subtract<byte>(values);
			generic_subtract<char>(values);
			generic_subtract<decimal>(values);
			generic_subtract<long>(values);
			generic_subtract<sbyte>(values);
			generic_subtract<short>(values);
			generic_subtract<uint>(values);
			generic_subtract<ulong>(values);
			generic_subtract<ushort>(values);
		}

		private void generic_subtract_signed(int[] values) {
			generic_subtract<double>(values);
			generic_subtract<float>(values);
			generic_subtract<int>(values);
			generic_subtract<decimal>(values);
			generic_subtract<long>(values);
			generic_subtract<sbyte>(values);
			generic_subtract<short>(values);
		}

		private void generic_subtract<TValue>(int[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a - b);

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Subtract);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_subtract<TValue>(double[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a - b);

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Subtract);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_multiply(double[] values) {
			generic_multiply<double>(values);
			generic_multiply<float>(values);
			generic_multiply<decimal>(values);
		}

		private void generic_multiply(int[] values) {
			generic_multiply<double>(values);
			generic_multiply<float>(values);
			generic_multiply<int>(values);
			generic_multiply<byte>(values);
			generic_multiply<char>(values);
			generic_multiply<decimal>(values);
			generic_multiply<long>(values);
			generic_multiply<sbyte>(values);
			generic_multiply<short>(values);
			generic_multiply<uint>(values);
			generic_multiply<ulong>(values);
			generic_multiply<ushort>(values);
		}

		private void generic_multiply_signed(int[] values) {
			generic_multiply<double>(values);
			generic_multiply<float>(values);
			generic_multiply<int>(values);
			generic_multiply<decimal>(values);
			generic_multiply<long>(values);
			generic_multiply<sbyte>(values);
			generic_multiply<short>(values);
		}

		private void generic_multiply<TValue>(int[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a * b);

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Multiply);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_multiply<TValue>(double[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();

					var expected = ConvertTo<TValue>(a * b);

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Multiply);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					Assert.AreEqual(expected, result);
				}
			}
		}

		private void generic_divide(double[] values) {
			generic_divide<double>(values);
			generic_divide<float>(values);
			generic_divide<decimal>(values);
		}

		private void generic_divide<TValue>(double[] values) {
			foreach (var a in values) {
				foreach (var b in values) {
					var provider = new DefaultGenericOperationProvider<TValue>();
					var expected = a / b;

                    var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
                    var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
					var expression = provider.GetBinaryExpression(leftHandSide, rightHandSide, GenericBinaryOperationType.Divide);
					var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
					var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));

					const double delta = 0.0001;
					Assert.AreEqual(expected, ConvertToDouble(result), delta);
				}
			}
		}

		private TValue ConvertTo<TValue>(int value) {
			TValue result;
			try {
				result = (TValue)Convert.ChangeType(value, typeof(TValue));
			}
			catch {
				var p = Expression.Parameter(typeof(int), "iVal");
				var exp = Expression.Convert(p, typeof(TValue));
				var func = Expression.Lambda<Func<int, TValue>>(exp, p).Compile();
				result = func(value);
			}
			return result;
		}

		private TValue ConvertTo<TValue>(double value) {
			TValue result;
			try {
				result = (TValue)Convert.ChangeType(value, typeof(TValue));
			}
			catch {
				var p = Expression.Parameter(typeof(double), "dVal");
				var exp = Expression.Convert(p, typeof(TValue));
				var func = Expression.Lambda<Func<double,TValue>>(exp,p).Compile();
				result = func(value);
			}
			return result;
		}

		private double ConvertToDouble<TValue>(TValue value) {
			double result;
			try {
				var changedValue = Convert.ChangeType(value, typeof (double));
				Assert.IsNotNull(changedValue);
				result = (double)changedValue;
			}
			catch {
				var p = Expression.Parameter(typeof(TValue), "tVal");
				var exp = Expression.Convert(p, typeof(double));
				var func = Expression.Lambda<Func<TValue, double>>(exp, p).Compile();
				result = func(value);
			}
			return result;
		}

		private void constant_is<TValue>(GenericConstantOperationType constantType, double value) {
			var provider = new DefaultGenericOperationProvider<TValue>();

			var expected = ConvertTo<TValue>(value);

			var expression = provider.GetConstantExpression(constantType);
			var func = Expression.Lambda<Func<TValue>>(expression).Compile();
			var result = func();
			Assert.AreEqual(
				expected, result,
				String.Format("Value: {0} ; Type: {1}.", value, typeof(TValue).FullName)
			);
		}

		private void constant_is(GenericConstantOperationType constantType, double value) {
			constant_is<double>(constantType, value);
			constant_is<float>(constantType, value);
			constant_is<int>(constantType, value);
			constant_is<byte>(constantType, value);
			constant_is<char>(constantType, value);
			constant_is<decimal>(constantType, value);
			constant_is<long>(constantType, value);
			constant_is<sbyte>(constantType, value);
			constant_is<short>(constantType, value);
			constant_is<uint>(constantType, value);
			constant_is<ulong>(constantType, value);
			constant_is<ushort>(constantType, value);
		}

		private void real_number_constant_is(GenericConstantOperationType constantType, double value) {
			constant_is<double>(constantType, value);
			constant_is<float>(constantType, value);
			constant_is<decimal>(constantType, value);
		}

		private void from_double_and_back_again<TValue>(double value) {
			var provider = new DefaultGenericOperationProvider<TValue>();

			var doubleParam = Expression.Parameter(typeof (double), "dVal");
			var fromDoubleExp = provider.GetUnaryExpression(doubleParam, GenericUnaryOperationType.ConvertFromDouble);
			var fromDoubleFunc = Expression.Lambda<Func<double, TValue>>(fromDoubleExp, doubleParam).Compile();
			var genericParam = Expression.Parameter(typeof (TValue), "tVal");
			var toDoubleExp = provider.GetUnaryExpression(genericParam, GenericUnaryOperationType.ConvertToDouble);
			var toDoubleFunc = Expression.Lambda<Func<TValue, double>>(toDoubleExp, genericParam).Compile();

			var expectedGenericValue = ConvertTo<TValue>(value);

			var genericValue = fromDoubleFunc(value);

			Assert.AreEqual(expectedGenericValue, genericValue);

			var doubleResult = toDoubleFunc(genericValue);
			const double delta = 0.0000001;
			Assert.AreEqual(
				value, doubleResult, delta,
				String.Format("Converting type: {0} value: {1} from double and back.", typeof(TValue).FullName, value)
			);
		}

		private void from_double_and_back_again<TValue>(IEnumerable<double> values) {
			foreach(var value in values) {
				from_double_and_back_again<TValue>(value);
			}
		}

		private void from_double_and_back_again(double[] values) {
			from_double_and_back_again<double>(values);
			from_double_and_back_again<float>(values);
			from_double_and_back_again<int>(values);
			from_double_and_back_again<byte>(values);
			from_double_and_back_again<char>(values);
			from_double_and_back_again<decimal>(values);
			from_double_and_back_again<long>(values);
			from_double_and_back_again<sbyte>(values);
			from_double_and_back_again<short>(values);
			from_double_and_back_again<uint>(values);
			from_double_and_back_again<ulong>(values);
			from_double_and_back_again<ushort>(values);
		}

		private void real_number_from_double_and_back_again(double[] values) {
			from_double_and_back_again<double>(values);
			from_double_and_back_again<float>(values);
			from_double_and_back_again<decimal>(values);
		}

		private void from_int_and_back_again<TValue>(int value) {
			var provider = new DefaultGenericOperationProvider<TValue>();

			var intParam = Expression.Parameter(typeof(int), "iVal");
			var fromIntExp = provider.GetUnaryExpression(intParam, GenericUnaryOperationType.ConvertFromInt);
			var fromIntFunc = Expression.Lambda<Func<int, TValue>>(fromIntExp, intParam).Compile();
			var genericParam = Expression.Parameter(typeof(TValue), "tVal");
			var toIntExp = provider.GetUnaryExpression(genericParam, GenericUnaryOperationType.ConvertToInt);
			var toIntFunc = Expression.Lambda<Func<TValue, int>>(toIntExp, genericParam).Compile();

			var expectedGenericValue = ConvertTo<TValue>(value);

			var genericValue = fromIntFunc(value);

			Assert.AreEqual(expectedGenericValue, genericValue);

			var intResult = toIntFunc(genericValue);

			//var delta = 0.0000001;
			Assert.AreEqual(
				value, intResult,
				String.Format("Converting type: {0} value: {1} from int and back.", typeof(TValue).FullName, value)
			);
		}

		private void from_int_and_back_again<TValue>(IEnumerable<int> values) {
			foreach (var value in values) {
				from_int_and_back_again<TValue>(value);
			}
		}

		private void from_int_and_back_again(int[] values) {
			from_int_and_back_again<double>(values);
			from_int_and_back_again<float>(values);
			from_int_and_back_again<int>(values);
			from_int_and_back_again<byte>(values);
			from_int_and_back_again<char>(values);
			from_int_and_back_again<decimal>(values);
			from_int_and_back_again<long>(values);
			from_int_and_back_again<sbyte>(values);
			from_int_and_back_again<short>(values);
			from_int_and_back_again<uint>(values);
			from_int_and_back_again<ulong>(values);
			from_int_and_back_again<ushort>(values);
		}

// ReSharper restore InconsistentNaming

	}

}
