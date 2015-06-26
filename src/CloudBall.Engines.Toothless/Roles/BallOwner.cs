using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudBall.Engines.Toothless.Roles
{
	public class BallOwner : IRole
	{
		public const float PassingPower = 7.5f;
		public const float PassingPowerNoCandidate = 8.5f;
		public const float PassingZ = .8f;

		public const float MaximumShootDistanceSquared = 650 * 650;
		
		public const float MinimumPassDistanceSquared = 200 * 200;
		public const float MaximumPassDistanceSquared = 500 * 500;

		public static readonly float[] PassingZs = new float[]{.95f, .9f, .85f, .8f, .75f };
		public static readonly float[] PassingPowers = new float[] { 8.5f, 8f, 7.5f, 7f, 6.5f };


		public Player Apply(TurnInfo info, IEnumerable<Player> queue)
		{
			var owner = queue.FirstOrDefault(p => p == info.Ball.Owner);
			
			// We are not the owner.
			if (owner == null) { return null; }

			var ownerDistanceToGoal2 = (Field.EnemyGoal.Center - owner.Position).LengthSquared;

			if(!info.Other.Players.Any(p => (p.Position - Field.EnemyGoal.Center).LengthSquared  < ownerDistanceToGoal2))
			{
				if (ownerDistanceToGoal2 < 150 * 150)
				{
					owner.ActionShootGoal();
				}
				else
				{
					owner.ActionGo(Field.EnemyGoal.Center);
				}
				return owner;
			}

			var shotOnGoalTop = Field.EnemyGoal.Top - owner.Position;
			var shotOnGoalCen = Field.EnemyGoal.Center - owner.Position;
			var shotOnGoalBot = Field.EnemyGoal.Bottom - owner.Position;
			var accuracy = Statistics.GetAccuracy(10, 0.75f);
			var shotAngle = Theta.Create(shotOnGoalTop, shotOnGoalBot);

			if (shotAngle > 2f * accuracy)
			{
				// vector for ball being shot at goal
				var shot = Field.EnemyGoal.Center - owner.Position;
				shot.Normalize();
				shot *= 12f;

				// if we cannot miss, shoot!
				var toGoal = BallPath.Create(info, shot);
				if (toGoal.End == BallPath.Ending.OtherGoal && toGoal.GetCatchUp(info.Other.Players, info.Ball).Count() <= 2)
				{
					owner.ActionShootGoal();
					return owner;
				}
			}

			var passCandidates = info.Own.Players
				.Where(p => p != owner && IsCandidate(owner, p, ownerDistanceToGoal2))
				.OrderBy(p => (Field.EnemyGoal.Center - p.Position).LengthSquared)
				.ToList();

			if (!passCandidates.Any())
			{
				owner.ActionGo(Field.EnemyGoal.Center);
				return owner;
			}

			var oppos = info.Other.Players.Where(p => p.Position.X > owner.Position.X).ToList();

			foreach (var z in PassingZs)
			{
				foreach (var power in PassingPowers)
				{
					var safe = new List<Player>();
					foreach (var candidate in passCandidates)
					{
						if (!info.Other.Players.Any(oppo => MightCatch(oppo.Position - owner.Position, candidate.Position - owner.Position, power, z)))
						{
							safe.Add(candidate);
						}
					}

					if (safe.Any())
					{
						var target = safe.OrderBy(s => (s.Position - Field.EnemyGoal.Center).LengthSquared).FirstOrDefault();
						owner.ActionShoot(target, PassingPower);
						return owner;
					}
				}
			}
			// else run.
			owner.ActionGo(Field.EnemyGoal.Center);
				
			return owner;
		}
		
		private static bool MightCatch(Vector oppoVector, Vector targetVector, float power, float z)
		{
			var accuracy = Statistics.GetAccuracy(power, z);
			return Math.Abs(Theta.Create(oppoVector, targetVector)) < accuracy;
		}

		private static bool IsCandidate(Player ballOwener, Player candidate, float ownerDistanceGoal2)
		{
			var candidateDistanceGoal2 = (Field.EnemyGoal.Center - candidate.Position).LengthSquared;
			if (candidateDistanceGoal2 > ownerDistanceGoal2) { return false; }

			var ownDistance = (ballOwener.Position - candidate.Position).LengthSquared;

			if (ownDistance < MinimumPassDistanceSquared || ownDistance > MaximumPassDistanceSquared) { return false; }

			return true;
		}
	}
}
