using System;
using NUnit.Framework;
using Vertesaur.Generation.GenericOperations;

namespace Vertesaur.Generation.Test
{
	[TestFixture]
	public class MathFunctionsTest
	{

		[Test]
		public void SinExpression() {
			Assert.AreEqual(
				Math.Sin(1.0),
				BasicOperations<double>.Default.Sin(1.0)
			);
		}

		[Test]
		public void CosExpression() {
			Assert.AreEqual(
				Math.Cos(1.0),
				BasicOperations<double>.Default.Cos(1.0)
			);
		}

		[Test]
		public void TanExpression() {
			Assert.AreEqual(
				Math.Tan(0.9),
				BasicOperations<double>.Default.Tan(0.9)
			);
		}

		[Test]
		public void AcosExpression() {
			Assert.AreEqual(
				Math.Acos(0.9),
				BasicOperations<double>.Default.Acos(0.9));
		}

		[Test]
		public void AsinExpression() {
			Assert.AreEqual(
				Math.Asin(0.9),
				BasicOperations<double>.Default.Asin(0.9));
		}

		[Test]
		public void AtanExpression() {
			Assert.AreEqual(
				Math.Atan(0.9),
				BasicOperations<double>.Default.Atan(0.9));
		}

		[Test]
		public void CoshExpression() {
			Assert.AreEqual(Math.Cosh(0.9), BasicOperations<double>.Default.Cosh(0.9));
		}

		[Test]
		public void SinhExpression() {
			Assert.AreEqual(Math.Sinh(0.9), BasicOperations<double>.Default.Sinh(0.9));
		}

		[Test]
		public void TanhExpression() {
			Assert.AreEqual(Math.Tanh(0.9), BasicOperations<double>.Default.Tanh(0.9));
		}

		[Test]
		public void AsinhExpression(){
			Assert.AreEqual(0.95034693, BasicOperations<double>.Default.Asinh(1.1), 0.000001);
		}

		[Test]
		public void AcoshExpression(){
			Assert.AreEqual(0.443568254, BasicOperations<double>.Default.Acosh(1.1), 0.000001);
		}

		[Test]
		public void AtanhExpression(){
			Assert.AreEqual(0.549306144, BasicOperations<double>.Default.Atanh(0.5), 0.000001);
		}

		[Test]
		public void LogExpression() {
			Assert.AreEqual(Math.Log(0.4), BasicOperations<double>.Default.Log(0.4));
		}

		[Test]
		public void Log10Expression() {
			Assert.AreEqual(Math.Log10(0.4), BasicOperations<double>.Default.Log10(0.4));
		}

		[Test]
		public void ExpExpression() {
			Assert.AreEqual(Math.Exp(1.3), BasicOperations<double>.Default.Exp(1.3));
		}

		[Test]
		public void PowExpression() {
			Assert.AreEqual(Math.Pow(1.2,1.1), BasicOperations<double>.Default.Pow(1.2,1.1));
		}

		[Test]
		public void AbsExpression() {
			Assert.AreEqual(2,BasicOperations<double>.Default.Abs(2));
			Assert.AreEqual(2, BasicOperations<double>.Default.Abs(-2));
		}

		[Test]
		public void MinExpression() {
			Assert.AreEqual(3, BasicOperations<double>.Default.Min(3,7));
		}

		[Test]
		public void MaxExpression() {
			Assert.AreEqual(7, BasicOperations<double>.Default.Max(3, 7));
		}

		[Test]
		public void Atan2Expression() {
			Assert.AreEqual(Math.Atan2(0.1, 0.9), BasicOperations<double>.Default.Atan2(0.1, 0.9));
			Assert.AreEqual(Math.Atan2(0.1, -0.9), BasicOperations<double>.Default.Atan2(0.1, -0.9));
			Assert.AreEqual(Math.Atan2(-0.1, -0.9), BasicOperations<double>.Default.Atan2(-0.1, -0.9));
			Assert.AreEqual(Math.Atan2(-0.1, 0.9), BasicOperations<double>.Default.Atan2(-0.1, 0.9));
		}

		[Test]
		public void Atan2DecimalExpression() {
			Assert.AreEqual(Math.Atan2(0.1, 0.9), (double)BasicOperations<decimal>.Default.Atan2(0.1m, 0.9m), 0.0000001);
			Assert.AreEqual(Math.Atan2(0.1, -0.9), (double)BasicOperations<decimal>.Default.Atan2(0.1m, -0.9m), 0.0000001);
			Assert.AreEqual(Math.Atan2(-0.1, -0.9), (double)BasicOperations<decimal>.Default.Atan2(-0.1m, -0.9m), 0.0000001);
			Assert.AreEqual(Math.Atan2(-0.1, 0.9), (double)BasicOperations<decimal>.Default.Atan2(-0.1m, 0.9m), 0.0000001);
		}

		[Test]
		public void Atan2FloatExpression() {
			Assert.AreEqual(Math.Atan2(0.1, 0.9), BasicOperations<float>.Default.Atan2(0.1f, 0.9f), 0.0000001);
			Assert.AreEqual(Math.Atan2(0.1, -0.9), BasicOperations<float>.Default.Atan2(0.1f, -0.9f), 0.0000001);
			Assert.AreEqual(Math.Atan2(-0.1, -0.9), BasicOperations<float>.Default.Atan2(-0.1f, -0.9f), 0.0000001);
			Assert.AreEqual(Math.Atan2(-0.1, 0.9), BasicOperations<float>.Default.Atan2(-0.1f, 0.9f), 0.0000001);
		}

		[Test]
		public void CeilingDouble() {
			Assert.AreEqual(1, BasicOperations<double>.Default.Ceiling(1.0));
			Assert.AreEqual(2, BasicOperations<double>.Default.Ceiling(1.1));
			Assert.AreEqual(-1, BasicOperations<double>.Default.Ceiling(-1.1));
		}

		[Test]
		public void CeilingDecimal() {
			Assert.AreEqual(1m, BasicOperations<decimal>.Default.Ceiling(1.0m));
			Assert.AreEqual(2m, BasicOperations<decimal>.Default.Ceiling(1.1m));
			Assert.AreEqual(-1m, BasicOperations<decimal>.Default.Ceiling(-1.1m));
		}

		[Test]
		public void CeilingFloat() {
			Assert.AreEqual(1f, BasicOperations<float>.Default.Ceiling(1.0f));
			Assert.AreEqual(2f, BasicOperations<float>.Default.Ceiling(1.1f));
			Assert.AreEqual(-1f, BasicOperations<float>.Default.Ceiling(-1.1f));
		}

		[Test]
		public void FloorDouble() {
			Assert.AreEqual(1, BasicOperations<double>.Default.Floor(1.0));
			Assert.AreEqual(2, BasicOperations<double>.Default.Floor(2.9));
			Assert.AreEqual(-3, BasicOperations<double>.Default.Floor(-2.9));
		}

		[Test]
		public void FloorDecimal() {
			Assert.AreEqual(1m, BasicOperations<decimal>.Default.Floor(1.0m));
			Assert.AreEqual(2m, BasicOperations<decimal>.Default.Floor(2.9m));
			Assert.AreEqual(-3m, BasicOperations<decimal>.Default.Floor(-2.9m));
		}

		[Test]
		public void FloorFloat() {
			Assert.AreEqual(1f, BasicOperations<float>.Default.Floor(1.0f));
			Assert.AreEqual(2f, BasicOperations<float>.Default.Floor(2.9f));
			Assert.AreEqual(-3f, BasicOperations<float>.Default.Floor(-2.9f));
		}

		[Test]
		public void TruncateDouble() {
			Assert.AreEqual(1, BasicOperations<double>.Default.Truncate(1.0));
			Assert.AreEqual(2, BasicOperations<double>.Default.Truncate(2.9));
			Assert.AreEqual(-2, BasicOperations<double>.Default.Truncate(-2.9));
		}

		[Test]
		public void TruncateDecimal() {
			Assert.AreEqual(1m, BasicOperations<decimal>.Default.Truncate(1.0m));
			Assert.AreEqual(2m, BasicOperations<decimal>.Default.Truncate(2.9m));
			Assert.AreEqual(-2m, BasicOperations<decimal>.Default.Truncate(-2.9m));
		}

		[Test]
		public void TruncateFloat() {
			Assert.AreEqual(1f, BasicOperations<float>.Default.Truncate(1.0f));
			Assert.AreEqual(2f, BasicOperations<float>.Default.Truncate(2.9f));
			Assert.AreEqual(-2f, BasicOperations<float>.Default.Truncate(-2.9f));
		}


	}
}
