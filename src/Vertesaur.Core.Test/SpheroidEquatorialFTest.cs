using NUnit.Framework;

namespace Vertesaur.Core.Test
{
	[TestFixture]
	public class SpheroidEquatorialFTest
	{

		[Test]
		public void ConstructorTest() {
			SpheroidEquatorialF obj;

			obj = new SpheroidEquatorialF(3.0, 4.0);
			Assert.AreEqual(3.0, obj.A);
			Assert.AreEqual(4.0, obj.F);

			obj = new SpheroidEquatorialF(4.0, 5.0);
			Assert.AreEqual(4.0, obj.A);
			Assert.AreEqual(5.0, obj.F);
		}

		[Test]
		public void ATest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 4.0);
			Assert.AreEqual(3.0, obj.A);
		}

		[Test]
		public void BTest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 1.0 / 4.0);
			Assert.AreEqual(9.0 / 4.0, obj.B);
		}

		[Test]
		public void FTest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 4.0);
			Assert.AreEqual(4.0, obj.F);
		}

		[Test]
		public void InvFTest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 4.0);
			Assert.AreEqual(1.0 / 4.0, obj.InvF);
		}

		[Test]
		public void ETest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 1.0 / 4.0);
			Assert.AreEqual(System.Math.Sqrt(7.0 / 16.0), obj.E);
		}

		[Test]
		public void ESquareTest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 1.0 / 4.0);
			Assert.AreEqual(obj.E * obj.E, obj.ESquared, 0.0001);
		}

		[Test]
		public void E2Test() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 1.0 / 4.0);
			Assert.AreEqual(System.Math.Sqrt((0.4375 / (1 - 0.4375))), obj.ESecond);
		}

		[Test]
		public void E2SquaredTest() {
			SpheroidEquatorialF obj = new SpheroidEquatorialF(3.0, 1.0 / 4.0);
			Assert.AreEqual(obj.ESecond * obj.ESecond, obj.ESecondSquared);
		}

	}
}
