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
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Vertesaur.Generation.ExpressionBuilder;


namespace Vertesaur.Generation.Test {

	/// <summary>
	/// Tests for the <c>DefaultBasicExpressionGenerator</c>.
	/// </summary>
	[TestFixture]
	public class CoreExpressionGeneratorTests {

		[Test]
		public void zero_constant_is_zero() {
			constant_is("0", 0);
		}

		[Test]
		public void one_constant_is_one() {
			constant_is("1", 1);
		}

		[Test]
		public void two_constant_is_two() {
			constant_is("2", 2);
		}

		[Test]
		public void e_constant_is_e() {
			real_number_constant_is("E", Math.E);
		}

		[Test]
		public void pi_constant_is_pi() {
			constant_is("PI", Math.PI);
		}

		[Test]
		public void generic_add_from_doubles(
			[Values(typeof(double),typeof(float),typeof(decimal))] Type valueType,
			[Values(0.0, 2.5, -9001)] double aValue,
			[Values(-1, 4)] double bValue
		) {
			var method = GetType().GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double), typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		private void generic_add<TValue>(double a, double b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a + b);
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator,"Add", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void generic_add_from_ints_signed(
			[Values(
				typeof(double),
				typeof(float),
				typeof(int),
				typeof(char),
				typeof(decimal),
				typeof(long),
				typeof(sbyte),
				typeof(short)
			)]
			Type valueType,
			[Values(0, 9)] int aValue,
			[Values(-8, 1)] int bValue
		) {
			var method = GetType().GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		[Test]
		public void generic_add_from_ints_unsigned(
			[Values(
				typeof(byte),
				typeof(uint),
				typeof(ulong),
				typeof(ushort)
			)]
			Type valueType,
			[Values(0, 9)] int aValue,
			[Values(8, 1)] int bValue
		) {
			var method = GetType().GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		private void generic_add<TValue>(int a, int b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a + b);
			var leftHandSide = Expression.Parameter(typeof (TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof (TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Add", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue,TValue,TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a),ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void generic_subtract_real(
			[Values(typeof(double), typeof(float), typeof(decimal))] Type valueType,
			[Values(0.0, 4)] double aValue,
			[Values(-1, 2.5, -9001)] double bValue
		) {
			var method = GetType().GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double), typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		private void generic_subtract<TValue>(double a, double b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a - b);
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Subtract", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void generic_subtract_signed(
			[Values(
				typeof(double),
				typeof(float),
				typeof(int),
				typeof(byte),
				typeof(char),
				typeof(decimal),
				typeof(long),
				typeof(short)
			)]
			Type valueType,
			[Values(0, 9, -1, -4)] int aValue,
			[Values(1, 4, -9)] int bValue
		) {
			var method = GetType().GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		[Test]
		public void generic_subtract_unsigned(
			[Values(
				typeof(byte),
				typeof(uint),
				typeof(ulong),
				typeof(ushort)
			)]
			Type valueType,
			[Values(9,42)] int aValue,
			[Values(0,2,3)] int bValue
		) {
			var method = GetType().GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
		}

		private void generic_subtract<TValue>(int a, int b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a - b);
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Subtract", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void generic_multiply_real(
			[Values(typeof(double), typeof(float), typeof(decimal))] Type valueType,
			[Values(0.0, 4)] double aValue,
			[Values(-1, 2.5, -9001.9)] double bValue
		) {
			var method = GetType().GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double), typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		[Test]
		public void generic_multiply_signed(
			[Values(
				typeof(double),
				typeof(float),
				typeof(int),
				typeof(byte),
				typeof(char),
				typeof(decimal),
				typeof(long),
				typeof(short)
			)] Type valueType,
			[Values(0, 9, -1, -4)] int aValue,
			[Values(1, 4, -9)] int bValue
		) {
			var method = GetType().GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		[Test]
		public void generic_multiply_unsigned(
			[Values(
				typeof(byte),
				typeof(uint),
				typeof(ulong),
				typeof(ushort)
			)]
			Type valueType,
			[Values(9, 42)] int aValue,
			[Values(0, 2, 3)] int bValue
		) {
			var method = GetType().GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		private void generic_multiply<TValue>(int a, int b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a * b);
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Multiply", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		private void generic_multiply<TValue>(double a, double b) {
			var generator = new CoreExpressionGenerator();
			var expected = ConvertTo<TValue>(a * b);
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Multiply", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void generic_divide(
			[Values(typeof(double), typeof(float), typeof(decimal))] Type valueType,
			[Values(0.0, 4)] double aValue,
			[Values(-1, 2.5, -9001.9)] double bValue
		) {
			var method = GetType().GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double), typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { aValue, bValue });
			genericMethod.Invoke(this, new object[] { bValue, aValue });
		}

		private void generic_divide<TValue>(double a, double b) {
			var generator = new CoreExpressionGenerator();
			var expected = a / b;
			var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
			var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
			var expression = generator.GenerateExpression(new FunctionExpressionGenerationRequest(generator, "Divide", leftHandSide, rightHandSide));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
			var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
			const double delta = 0.0001;
			Assert.AreEqual(expected, ConvertToDouble(result), delta);
		}


		[Test]
		public void from_double_and_back_again(
			[Values(
				typeof(double),
				typeof(float),
				typeof(int),
				typeof(char),
				typeof(decimal),
				typeof(long),
				typeof(sbyte),
				typeof(short),
				typeof(byte),
				typeof(uint),
				typeof(ulong),
				typeof(ushort)
			)]
			Type valueType,
			[Values(0.0,1,2,3,4,5,6,7,8,9,10,16,32,64,127)]
			double value
		) {
			var method = GetType().GetMethod("from_double_and_back_again", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { value });
		}

		[Test]
		public void from_double_and_back_again_real(
			[Values(
				typeof(double),
				typeof(float),
				typeof(decimal)
			)]
			Type valueType,
			[Values(-65535, -0.0000000001, 0, Math.E, Math.PI, 9001, 42)]
			double value
		) {
			var method = GetType().GetMethod("from_double_and_back_again", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(double) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { value });
		}

		[Test]
		public void from_int_and_back_again(
			[Values(
				typeof(double),
				typeof(float),
				typeof(int),
				typeof(char),
				typeof(decimal),
				typeof(long),
				typeof(sbyte),
				typeof(short),
				typeof(byte),
				typeof(uint),
				typeof(ulong),
				typeof(ushort)
			)]
			Type valueType,
			[Values(0,1,2,3,4,5,6,7,8,9,10,16,32,64,127)]
			int value
		) {
			var method = GetType().GetMethod("from_int_and_back_again", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(int) }, null);
			var genericMethod = method.MakeGenericMethod(new[] { valueType });
			genericMethod.Invoke(this, new object[] { value });
		}

		private void from_double_and_back_again<TValue>(double value) {
			var generator = new CoreExpressionGenerator();

			var doubleParam = Expression.Parameter(typeof(double), "dVal");
			var fromDoubleExp = generator.GenerateExpression(new ConversionExpressionRequest(generator, doubleParam, typeof(TValue)));
			Assert.IsNotNull(fromDoubleExp);
			var fromDoubleFunc = Expression.Lambda<Func<double, TValue>>(fromDoubleExp, doubleParam).Compile();
			var genericParam = Expression.Parameter(typeof(TValue), "tVal");
			var toDoubleExp = generator.GenerateExpression(new ConversionExpressionRequest(generator, genericParam, typeof(double)));
			Assert.IsNotNull(toDoubleExp);
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

		private void from_int_and_back_again<TValue>(int value) {
			var generator = new CoreExpressionGenerator();

			var intParam = Expression.Parameter(typeof(int), "iVal");
			var fromIntExp = generator.GenerateExpression(new ConversionExpressionRequest(generator, intParam, typeof(TValue)));
			Assert.IsNotNull(fromIntExp);
			var fromIntFunc = Expression.Lambda<Func<int, TValue>>(fromIntExp, intParam).Compile();
			var genericParam = Expression.Parameter(typeof(TValue), "tVal");
			var toIntExp = generator.GenerateExpression(new ConversionExpressionRequest(generator, genericParam, typeof(int)));
			Assert.IsNotNull(toIntExp);
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

		private void constant_is<TValue>(string constantName, double value) {
			var generator = new CoreExpressionGenerator();

			var expected = ConvertTo<TValue>(value);

			var expression = generator.GenerateExpression(
				new ConstantExpressionGenerationRequest(generator, constantName, typeof (TValue)));
			Assert.IsNotNull(expression);
			var func = Expression.Lambda<Func<TValue>>(expression).Compile();
			var result = func();
			Assert.AreEqual(
				expected, result,
				String.Format("Value: {0} ; Type: {1}.", value, typeof(TValue).FullName)
			);
		}

		private void constant_is(string constantName, double value) {
			constant_is<double>(constantName, value);
			constant_is<float>(constantName, value);
			constant_is<int>(constantName, value);
			constant_is<byte>(constantName, value);
			constant_is<char>(constantName, value);
			constant_is<decimal>(constantName, value);
			constant_is<long>(constantName, value);
			constant_is<sbyte>(constantName, value);
			constant_is<short>(constantName, value);
			constant_is<uint>(constantName, value);
			constant_is<ulong>(constantName, value);
			constant_is<ushort>(constantName, value);
		}

		private void real_number_constant_is(string constantName, double value) {
			constant_is<double>(constantName, value);
			constant_is<float>(constantName, value);
			constant_is<decimal>(constantName, value);
		}

	}

}
