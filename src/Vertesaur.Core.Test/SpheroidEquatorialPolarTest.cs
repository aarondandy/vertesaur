using System;
using NUnit.Framework;

namespace Vertesaur.Core.Test
{
	[TestFixture]
	public class SpheroidEquatorialPolarTest
	{

		[Test]
		public void ConstructorTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(3.0, obj.A);
			Assert.AreEqual(9.0 / 4.0, obj.B);

			obj = new SpheroidEquatorialPolar(4.0, 5.0);
			Assert.AreEqual(4.0, obj.A);
			Assert.AreEqual(5.0, obj.B);
		}

		[Test]
		public void ATest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(3.0, obj.A);
		}

		[Test]
		public void BTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(9.0 / 4.0, obj.B);
		}

		[Test]
		public void FTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(1.0 / 4.0, obj.F);
		}

		[Test]
		public void InvFTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(4.0, obj.InvF);
		}

		[Test]
		public void ETest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(Math.Sqrt(7.0 / 16.0), obj.E);
		}

		[Test]
		public void ESquareTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(obj.E * obj.E, obj.ESquared, 0.0000001);
		}

		[Test]
		public void E2Test() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(Math.Sqrt((0.4375 / (1 - 0.4375))), obj.ESecond);
		}

		[Test]
		public void E2SquaredTest() {
			var obj = new SpheroidEquatorialPolar(3.0, 9.0 / 4.0);
			Assert.AreEqual(obj.ESecond * obj.ESecond, obj.ESecondSquared, 0.0000000001);
		}

	}
}
