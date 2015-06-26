using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudBall.Engines.Toothless.Test
{
	[TestClass]
	public class PassingTest
	{
		[TestMethod]
		public void GetTheta_()
		{
			var act = Passing.GetSafeTheta(150f * 150f, 8f, 0);
			var exp = 0.9442f;
			Assert.AreEqual(exp, act, 0.001f);
		}
	}
}
