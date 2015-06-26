using Common;
using System;

namespace CloudBall.Engines.Toothless
{
	public static partial class Statistics
	{
		private static readonly float[,] BallDistances = new float[51, 512];
		static Statistics()
		{
			for (var p = 0; p < 51; p++)
			{
				var pow = 5.0f + (p / 10f);
				var dis = 0f;
				for (var t = 0; t < 512; t++)
				{
					BallDistances[p, t] = dis;
					dis += pow;
					pow *= Constants.BallSlowDownFactor;
				}
			}
		}

		public static float GetBallDistance(float power, int turn)
		{
			int x = (int)((power - 4.9f) * 10f);
			return BallDistances[x, Math.Min(511, turn)];
		}
		public static float GetAccuracy(float power, float z)
		{
			int x = (int)((power - 4.9f) * 10f);
			int y = (int)((z - .745f) * 20f);

			return Accuracy[x, y];
		}
	}

}
