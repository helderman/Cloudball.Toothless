using Common;
using System;

namespace CloudBall.Engines.Toothless.Test
{
	public class ShootSimulator
	{
		public ShootSimulator(int seed = 17)
		{
			Rnd = new Random(seed);
		}

		public Random Rnd { get; protected set; }

		public Vector Shoot(Vector source, Vector target, float str)
		{
			var vector = target - source;
			if (vector.Length != 0)
			{
				vector.Normalize();
			}
			str = Math.Max(0, Math.Min(Constants.PlayerMaxShootStr, str));
			var d = Rnd.NextDouble();
			var num2 = Rnd.NextDouble();
			var num3 = Math.Sqrt(-2.0 * Math.Log(d)) * Math.Sin(Math.PI * 2.0 * num2);
			var num4 = ((str / Constants.PlayerMaxShootStr) * Constants.BallMaxStrStd) * num3;
			vector.X = (float)((vector.X * Math.Cos(num4)) - (vector.Y * Math.Sin(num4)));
			vector.Y = (float)((vector.X * Math.Sin(num4)) - (vector.Y * Math.Cos(num4)));

			var result = ((vector * str) * Constants.BallMaxVelocity) / Constants.PlayerMaxShootStr;
			return result;
		}
	}
}
