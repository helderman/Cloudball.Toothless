using CloudBall.Engines.Toothless.Roles;
using Common;
using System;

namespace CloudBall.Engines.Toothless
{
	public static class Passing
	{
		/// <summary>Gets the safe angle/theta to shoot.</summary>
		public static Theta GetSafeTheta(IPosition ball, Player opponent, float power)
		{
			var dis2 = (ball.Position - opponent.Position).LengthSquared;
			return GetSafeTheta(dis2, power, opponent.FallenTimer);
		}
		/// <summary>Gets the safe angle/theta to shoot.</summary>
		public static Theta GetSafeTheta(float distanceSquared, float power, int fallenimer)
		{
			var first_Check = Math.Max(fallenimer, Constants.BallShootTimer) - 1;
			// dis^2 < travel_opp^2 + travel_ball^2
			for (var turn = first_Check; turn < 512; turn++)
			{
				var travel_ball = Statistics.GetBallDistance(power, turn);
				var travel_oppo = (turn - fallenimer)* Constants.PlayerMaxVelocity + Constants.BallMaxPickUpDistance;

				if (travel_ball * travel_ball + travel_oppo * travel_oppo >= distanceSquared)
				{
					// the ball can not be picked up, so it is safe to shoot anyway!
					if (turn == first_Check)
					{
						return 0f;
					}
					return (float)Math.Atan(travel_ball / travel_oppo);
				}
			}
			return 0f;
		}
	}
}
