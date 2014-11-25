using System;
using FluentAssertions;
using Vertesaur.Generation.GenericOperations;
using Xunit;
using Xunit.Extensions;

namespace Vertesaur.Generation.Test
{
    public static class MathFunctionsFacts
    {

        [Fact]
        public static void SinExpression() {
            Assert.Equal(Math.Sin(1.0), BasicOperations<double>.Default.Sin(1.0));
        }

        [Fact]
        public static void CosExpression() {
            Assert.Equal(Math.Cos(1.0), BasicOperations<double>.Default.Cos(1.0));
        }

        [Fact]
        public static void TanExpression() {
            Assert.Equal(Math.Tan(0.9), BasicOperations<double>.Default.Tan(0.9));
        }

        [Fact]
        public static void AcosExpression() {
            Assert.Equal(Math.Acos(0.9), BasicOperations<double>.Default.Acos(0.9));
        }

        [Fact]
        public static void AsinExpression() {
            Assert.Equal(Math.Asin(0.9), BasicOperations<double>.Default.Asin(0.9));
        }

        [Fact]
        public static void AtanExpression() {
            Assert.Equal(Math.Atan(0.9), BasicOperations<double>.Default.Atan(0.9));
        }

        [Fact]
        public static void CoshExpression() {
            Assert.Equal(Math.Cosh(0.9), BasicOperations<double>.Default.Cosh(0.9));
        }

        [Fact]
        public static void SinhExpression() {
            Assert.Equal(Math.Sinh(0.9), BasicOperations<double>.Default.Sinh(0.9));
        }

        [Fact]
        public static void TanhExpression() {
            Assert.Equal(Math.Tanh(0.9), BasicOperations<double>.Default.Tanh(0.9));
        }

        [Fact]
        public static void AsinhExpression() {
            Assert.Equal(0.95034693, BasicOperations<double>.Default.Asinh(1.1), 5);
        }

        [Fact]
        public static void AcoshExpression() {
            Assert.Equal(0.443568254, BasicOperations<double>.Default.Acosh(1.1), 5);
        }

        [Fact]
        public static void AtanhExpression() {
            Assert.Equal(0.549306144, BasicOperations<double>.Default.Atanh(0.5), 5);
        }

        [Fact]
        public static void LogExpression() {
            Assert.Equal(Math.Log(0.4), BasicOperations<double>.Default.Log(0.4));
        }

        [Fact]
        public static void Log10Expression() {
            Assert.Equal(Math.Log10(0.4), BasicOperations<double>.Default.Log10(0.4));
        }

        [Fact]
        public static void ExpExpression() {
            Assert.Equal(Math.Exp(1.3), BasicOperations<double>.Default.Exp(1.3));
        }

        [Fact]
        public static void PowExpression() {
            Assert.Equal(Math.Pow(1.2, 1.1), BasicOperations<double>.Default.Pow(1.2, 1.1));
        }

        [Fact]
        public static void AbsExpression() {
            Assert.Equal(2, BasicOperations<double>.Default.Abs(2));
            Assert.Equal(2, BasicOperations<double>.Default.Abs(-2));
        }

        [Fact]
        public static void MinExpression() {
            Assert.Equal(3, BasicOperations<double>.Default.Min(3, 7));
        }

        [Fact]
        public static void MaxExpression() {
            Assert.Equal(7, BasicOperations<double>.Default.Max(3, 7));
        }

        [Fact]
        public static void Atan2Expression() {
            Assert.Equal(Math.Atan2(0.1, 0.9), BasicOperations<double>.Default.Atan2(0.1, 0.9));
            Assert.Equal(Math.Atan2(0.1, -0.9), BasicOperations<double>.Default.Atan2(0.1, -0.9));
            Assert.Equal(Math.Atan2(-0.1, -0.9), BasicOperations<double>.Default.Atan2(-0.1, -0.9));
            Assert.Equal(Math.Atan2(-0.1, 0.9), BasicOperations<double>.Default.Atan2(-0.1, 0.9));
        }

        [Fact]
        public static void Atan2DecimalExpression() {
            Assert.Equal(Math.Atan2(0.1, 0.9), (double)BasicOperations<decimal>.Default.Atan2(0.1m, 0.9m), 10);
            Assert.Equal(Math.Atan2(0.1, -0.9), (double)BasicOperations<decimal>.Default.Atan2(0.1m, -0.9m), 10);
            Assert.Equal(Math.Atan2(-0.1, -0.9), (double)BasicOperations<decimal>.Default.Atan2(-0.1m, -0.9m), 10);
            Assert.Equal(Math.Atan2(-0.1, 0.9), (double)BasicOperations<decimal>.Default.Atan2(-0.1m, 0.9m), 10);
        }

        [Fact]
        public static void Atan2FloatExpression() {
            Assert.Equal(Math.Atan2(0.1, 0.9), BasicOperations<float>.Default.Atan2(0.1f, 0.9f), 5);
            Assert.Equal(Math.Atan2(0.1, -0.9), BasicOperations<float>.Default.Atan2(0.1f, -0.9f), 5);
            Assert.Equal(Math.Atan2(-0.1, -0.9), BasicOperations<float>.Default.Atan2(-0.1f, -0.9f), 5);
            Assert.Equal(Math.Atan2(-0.1, 0.9), BasicOperations<float>.Default.Atan2(-0.1f, 0.9f), 5);
        }

        [Fact]
        public static void CeilingDouble() {
            Assert.Equal(1, BasicOperations<double>.Default.Ceiling(1.0));
            Assert.Equal(2, BasicOperations<double>.Default.Ceiling(1.1));
            Assert.Equal(-1, BasicOperations<double>.Default.Ceiling(-1.1));
        }

        [Fact]
        public static void CeilingDecimal() {
            Assert.Equal(1m, BasicOperations<decimal>.Default.Ceiling(1.0m));
            Assert.Equal(2m, BasicOperations<decimal>.Default.Ceiling(1.1m));
            Assert.Equal(-1m, BasicOperations<decimal>.Default.Ceiling(-1.1m));
        }

        [Fact]
        public static void CeilingFloat() {
            Assert.Equal(1f, BasicOperations<float>.Default.Ceiling(1.0f));
            Assert.Equal(2f, BasicOperations<float>.Default.Ceiling(1.1f));
            Assert.Equal(-1f, BasicOperations<float>.Default.Ceiling(-1.1f));
        }

        [Fact]
        public static void FloorDouble() {
            Assert.Equal(1, BasicOperations<double>.Default.Floor(1.0));
            Assert.Equal(2, BasicOperations<double>.Default.Floor(2.9));
            Assert.Equal(-3, BasicOperations<double>.Default.Floor(-2.9));
        }

        [Fact]
        public static void FloorDecimal() {
            Assert.Equal(1m, BasicOperations<decimal>.Default.Floor(1.0m));
            Assert.Equal(2m, BasicOperations<decimal>.Default.Floor(2.9m));
            Assert.Equal(-3m, BasicOperations<decimal>.Default.Floor(-2.9m));
        }

        [Fact]
        public static void FloorFloat() {
            Assert.Equal(1f, BasicOperations<float>.Default.Floor(1.0f));
            Assert.Equal(2f, BasicOperations<float>.Default.Floor(2.9f));
            Assert.Equal(-3f, BasicOperations<float>.Default.Floor(-2.9f));
        }

        [Fact]
        public static void TruncateDouble() {
            Assert.Equal(1, BasicOperations<double>.Default.Truncate(1.0));
            Assert.Equal(2, BasicOperations<double>.Default.Truncate(2.9));
            Assert.Equal(-2, BasicOperations<double>.Default.Truncate(-2.9));
        }

        [Fact]
        public static void TruncateDecimal() {
            Assert.Equal(1m, BasicOperations<decimal>.Default.Truncate(1.0m));
            Assert.Equal(2m, BasicOperations<decimal>.Default.Truncate(2.9m));
            Assert.Equal(-2m, BasicOperations<decimal>.Default.Truncate(-2.9m));
        }

        [Fact]
        public static void TruncateFloat() {
            Assert.Equal(1f, BasicOperations<float>.Default.Truncate(1.0f));
            Assert.Equal(2f, BasicOperations<float>.Default.Truncate(2.9f));
            Assert.Equal(-2f, BasicOperations<float>.Default.Truncate(-2.9f));
        }


    }
}
