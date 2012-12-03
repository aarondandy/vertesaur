using NUnit.Framework;

namespace Vertesaur.Core.Test
{
	[TestFixture]
	public class SpheroidEquatorialInvFTest
	{

		[Test]
		public void ConstructorTest() {
			SpheroidEquatorialInvF obj;

			obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(3.0, obj.A);
			Assert.AreEqual(4.0, obj.InvF);

			obj = new SpheroidEquatorialInvF(4.0, 5.0);
			Assert.AreEqual(4.0, obj.A);
			Assert.AreEqual(5.0, obj.InvF);
		}

		[Test]
		public void ATest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(3.0, obj.A);
		}

		[Test]
		public void BTest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(9.0 / 4.0, obj.B);
		}

		[Test]
		public void FTest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(1.0 / 4.0, obj.F);
		}

		[Test]
		public void InvFTest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(4.0, obj.InvF);
		}

		[Test]
		public void ETest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(System.Math.Sqrt(7.0 / 16.0), obj.E);
		}

		[Test]
		public void ESquareTest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(obj.E * obj.E, obj.ESquared, 0.0001);
		}

		[Test]
		public void E2Test() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(System.Math.Sqrt((0.4375 / (1 - 0.4375))), obj.ESecond);
		}

		[Test]
		public void E2SquaredTest() {
			SpheroidEquatorialInvF obj = new SpheroidEquatorialInvF(3.0, 4.0);
			Assert.AreEqual(obj.ESecond * obj.ESecond, obj.ESecondSquared);
		}

	}
}
