using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudBall.Engines.Toothless.Test
{
	[TestClass]
	public class StatisticsTest
	{
		[TestMethod]
		public void GetSAccuracy_Power5z75_0f068()
		{
			var act = Statistics.GetAccuracy(5f, .75f);
			var exp = 0.068f;
			Assert.AreEqual(exp, act);
		}

		[TestMethod]
		public void GetSAccuracy_Power10z95_0f312()
		{
			var act = Statistics.GetAccuracy(10f, .95f);
			var exp = 0.312f;
			Assert.AreEqual(exp, act);
		}
	}
}
