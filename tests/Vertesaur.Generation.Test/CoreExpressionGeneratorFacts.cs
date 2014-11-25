using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Vertesaur.Generation.Expressions;
using Xunit;
using Xunit.Extensions;


namespace Vertesaur.Generation.Test
{
    public static class CoreExpressionGeneratorFacts
    {

        private static readonly Type TestType = typeof(CoreExpressionGeneratorFacts);

        [Fact]
        public static void zero_constant_is_zero() {
            constant_is("0", 0);
        }

        [Fact]
        public static void one_constant_is_one() {
            constant_is("1", 1);
        }

        [Fact]
        public static void two_constant_is_two() {
            constant_is("2", 2);
        }

        [Fact]
        public static void e_constant_is_e() {
            real_number_constant_is("E", Math.E);
        }

        [Fact]
        public static void pi_constant_is_pi() {
            constant_is("PI", Math.PI);
        }

        public static IEnumerable<object[]> generic_add_from_doubles_data {
            get {
                foreach (var valueType in new[] { typeof(double), typeof(float), typeof(decimal) }) {
                    foreach (var a in new[] { 0.0, 2.5, -9001.0 }) {
                        foreach (var b in new[] { -1.0, 4.0 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_add_from_doubles_data")]
        public static void generic_add_from_doubles(Type valueType, double aValue, double bValue) {
            var method = TestType.GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double), typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        private static void generic_add<TValue>(double a, double b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a + b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Add", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> generic_add_from_ints_signed_data {
            get {
                foreach (var valueType in new[] { typeof(double),
                    typeof(float),
                    typeof(int),
                    typeof(char),
                    typeof(decimal),
                    typeof(long),
                    typeof(sbyte),
                    typeof(short)})
                {
                    foreach (var a in new int[] { 0, 9 }) {
                        foreach (var b in new int[] { -8, 1 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_add_from_ints_signed_data")]
        public static void generic_add_from_ints_signed(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        public static IEnumerable<object[]> generic_add_from_ints_unsigned_data {
            get {
                foreach (var valueType in new[] { typeof(byte), typeof(uint), typeof(ulong), typeof(ushort) }) {
                    foreach (var a in new int[] { 0, 9 }) {
                        foreach (var b in new int[] { 8, 1 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_add_from_ints_unsigned_data")]
        public static void generic_add_from_ints_unsigned(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_add", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        private static void generic_add<TValue>(int a, int b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a + b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Add", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> generic_subtract_real_data {
            get {
                foreach (var valueType in new[] { typeof(double), typeof(float), typeof(decimal) }) {
                    foreach (var a in new [] { 0.0, 4.0 }) {
                        foreach (var b in new [] { -1.0, 2.5, -9001.0 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_subtract_real_data")]
        public static void generic_subtract_real(Type valueType, double aValue, double bValue) {
            var method = TestType.GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double), typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        private static void generic_subtract<TValue>(double a, double b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a - b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Subtract", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> generic_subtract_signed_data {
            get {
                foreach (var valueType in new[] {
                    typeof(double),
                    typeof(float),
                    typeof(int),
                    typeof(byte),
                    typeof(char),
                    typeof(decimal),
                    typeof(long),
                    typeof(short) })
                {
                    foreach (var a in new int[] { 0, 9, -1, -4 }) {
                        foreach (var b in new int[] { 1, 4, -9 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_subtract_signed_data")]
        public static void generic_subtract_signed(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        public static IEnumerable<object[]> generic_subtract_unsigned_data {
            get {
                foreach (var valueType in new[] { typeof(byte), typeof(uint), typeof(ulong), typeof(ushort) }) {
                    foreach (var a in new int[] { 9, 42 }) {
                        foreach (var b in new int[] { 0, 2, 3 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_subtract_unsigned_data")]
        public static void generic_subtract_unsigned(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_subtract", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
        }

        private static void generic_subtract<TValue>(int a, int b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a - b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Subtract", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> generic_multiply_real_data {
            get {
                foreach (var valueType in new[] { typeof(double), typeof(float), typeof(decimal) }) {
                    foreach (var a in new [] { 0.0, 4.0 }) {
                        foreach (var b in new [] { -1.0, 2.5, -9001.9 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_multiply_real_data")]
        public static void generic_multiply_real(Type valueType, double aValue, double bValue) {
            var method = TestType.GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double), typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        public static IEnumerable<object[]> generic_multiply_signed_data {
            get {
                foreach (var valueType in new[] {
                    typeof(double),
                    typeof(float),
                    typeof(int),
                    typeof(byte),
                    typeof(char),
                    typeof(decimal),
                    typeof(long),
                    typeof(short) })
                {
                    foreach (var a in new int[] { 0, 9, -1, -4 }) {
                        foreach (var b in new int[] { 1, 4, -9 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_multiply_signed_data")]
        public static void generic_multiply_signed(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        public static IEnumerable<object[]> generic_multiply_unsigned_data {
            get {
                foreach (var valueType in new[] { typeof(byte), typeof(uint), typeof(ulong), typeof(ushort) }) {
                    foreach (var a in new int[] { 9, 42 }) {
                        foreach (var b in new int[] { 0, 2, 3 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_multiply_unsigned_data")]
        public static void generic_multiply_unsigned(Type valueType, int aValue, int bValue) {
            var method = TestType.GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int), typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        private static void generic_multiply<TValue>(int a, int b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a * b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Multiply", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        private static void generic_multiply<TValue>(double a, double b) {
            var generator = new CoreExpressionGenerator();
            var expected = ConvertTo<TValue>(a * b);
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Multiply", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> generic_divide_data {
            get {
                foreach (var valueType in new[] { typeof(double), typeof(float), typeof(decimal) }) {
                    foreach (var a in new [] { 0.0, 4.0 }) {
                        foreach (var b in new [] { -1.0, 2.5, -9001.9 }) {
                            yield return new object[] { valueType, a, b };
                        }
                    }
                }
            }
        }

        [Theory, PropertyData("generic_divide_data")]
        public static void generic_divide(Type valueType, double aValue, double bValue) {
            var method = TestType.GetMethod("generic_multiply", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double), typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { aValue, bValue });
            genericMethod.Invoke(null, new object[] { bValue, aValue });
        }

        private static void generic_divide<TValue>(double a, double b) {
            var generator = new CoreExpressionGenerator();
            var expected = a / b;
            var leftHandSide = Expression.Parameter(typeof(TValue), "leftHandSide");
            var rightHandSide = Expression.Parameter(typeof(TValue), "rightHandSide");
            var expression = generator.Generate(new FunctionExpressionGenerationRequest(generator, "Divide", leftHandSide, rightHandSide));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue, TValue, TValue>>(expression, leftHandSide, rightHandSide).Compile();
            var result = func(ConvertTo<TValue>(a), ConvertTo<TValue>(b));
            Assert.Equal(expected, ConvertToDouble(result), 10);
        }

        public static IEnumerable<object[]> from_double_and_back_again_data {
            get {
                foreach (var valueType in new[] {
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
                    typeof(ushort) })
                {
                    foreach (var value in new double [] { 0.0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 16, 32, 64, 127 }) {
                        yield return new object[] { valueType, value };
                    }
                }
            }
        }


        [Theory, PropertyData("from_double_and_back_again_data")]
        public static void from_double_and_back_again(Type valueType, double value) {
            var method = TestType.GetMethod("from_double_and_back_again", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { value });
        }

        public static IEnumerable<object[]> from_double_and_back_again_real_data {
            get {
                foreach (var valueType in new[] {
                    typeof(double),
                    typeof(float),
                    typeof(decimal) })
                {
                    foreach (var value in new double[] { -65535, -0.0000000001, 0, Math.E, Math.PI, 9001, 42 }) {
                        yield return new object[] { valueType, value };
                    }
                }
            }
        }

        [Theory, PropertyData("from_double_and_back_again_real_data")]
        public static void from_double_and_back_again_real(Type valueType, double value) {
            var method = TestType.GetMethod("from_double_and_back_again", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(double) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { value });
        }

        public static IEnumerable<object[]> from_int_and_back_again_data {
            get {
                foreach (var valueType in new[] {
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
                    typeof(ushort) }) {
                    foreach (var value in new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 16, 32, 64, 127 }) {
                        yield return new object[] { valueType, value };
                    }
                }
            }
        }

        [Theory, PropertyData("from_int_and_back_again_data")]
        public static void from_int_and_back_again(Type valueType, int value) {
            var method = TestType.GetMethod("from_int_and_back_again", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(int) }, null);
            var genericMethod = method.MakeGenericMethod(new[] { valueType });
            genericMethod.Invoke(null, new object[] { value });
        }

        private static void from_double_and_back_again<TValue>(double value) {
            var generator = new CoreExpressionGenerator();

            var doubleParam = Expression.Parameter(typeof(double), "dVal");
            var fromDoubleExp = generator.Generate(new ConversionExpressionRequest(generator, doubleParam, typeof(TValue)));
            Assert.NotNull(fromDoubleExp);
            var fromDoubleFunc = Expression.Lambda<Func<double, TValue>>(fromDoubleExp, doubleParam).Compile();
            var genericParam = Expression.Parameter(typeof(TValue), "tVal");
            var toDoubleExp = generator.Generate(new ConversionExpressionRequest(generator, genericParam, typeof(double)));
            Assert.NotNull(toDoubleExp);
            var toDoubleFunc = Expression.Lambda<Func<TValue, double>>(toDoubleExp, genericParam).Compile();

            var expectedGenericValue = ConvertTo<TValue>(value);

            var genericValue = fromDoubleFunc(value);

            Assert.Equal(expectedGenericValue, genericValue);

            var doubleResult = toDoubleFunc(genericValue);

            Assert.Equal(value, doubleResult, 5);
        }

        private static void from_int_and_back_again<TValue>(int value) {
            var generator = new CoreExpressionGenerator();

            var intParam = Expression.Parameter(typeof(int), "iVal");
            var fromIntExp = generator.Generate(new ConversionExpressionRequest(generator, intParam, typeof(TValue)));
            Assert.NotNull(fromIntExp);
            var fromIntFunc = Expression.Lambda<Func<int, TValue>>(fromIntExp, intParam).Compile();
            var genericParam = Expression.Parameter(typeof(TValue), "tVal");
            var toIntExp = generator.Generate(new ConversionExpressionRequest(generator, genericParam, typeof(int)));
            Assert.NotNull(toIntExp);
            var toIntFunc = Expression.Lambda<Func<TValue, int>>(toIntExp, genericParam).Compile();

            var expectedGenericValue = ConvertTo<TValue>(value);

            var genericValue = fromIntFunc(value);

            Assert.Equal(expectedGenericValue, genericValue);

            var intResult = toIntFunc(genericValue);

            Assert.Equal(value, intResult);
        }

        private static TValue ConvertTo<TValue>(int value) {
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

        private static TValue ConvertTo<TValue>(double value) {
            TValue result;
            try {
                result = (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            catch {
                var p = Expression.Parameter(typeof(double), "dVal");
                var exp = Expression.Convert(p, typeof(TValue));
                var func = Expression.Lambda<Func<double, TValue>>(exp, p).Compile();
                result = func(value);
            }
            return result;
        }

        private static double ConvertToDouble<TValue>(TValue value) {
            double result;
            try {
                var changedValue = Convert.ChangeType(value, typeof(double));
                Assert.NotNull(changedValue);
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

        private static void constant_is<TValue>(string constantName, double value) {
            var generator = new CoreExpressionGenerator();

            var expected = ConvertTo<TValue>(value);

            var expression = generator.Generate(
                new ConstantExpressionGenerationRequest(generator, constantName, typeof(TValue)));
            Assert.NotNull(expression);
            var func = Expression.Lambda<Func<TValue>>(expression).Compile();
            var result = func();
            Assert.Equal(expected, result);
        }

        private static void constant_is(string constantName, double value) {
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

        private static void real_number_constant_is(string constantName, double value) {
            constant_is<double>(constantName, value);
            constant_is<float>(constantName, value);
            constant_is<decimal>(constantName, value);
        }

    }

}
