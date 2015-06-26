using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public class BallOwner : IRole
	{
		public const float PassingPower = 7.5f;
		public const float PassingZ = .8f;
		public Player Apply(TurnInfo info, IEnumerable<Player> queue)
		{
			var owner = queue.FirstOrDefault(p => p == info.Ball.Owner);
			
			// We are not the owner.
			if (owner == null) { return null; }


			var ownerDistanceToGoal2 = (Field.EnemyGoal.Center - owner.Position).LengthSquared;

			var passCandidates = info.Own.Players
				.Where(p => p != owner && IsCandidate(owner, p, ownerDistanceToGoal2))
				.OrderBy(p => (Field.EnemyGoal.Center - p.Position).LengthSquared)
				.ToList();

			if (passCandidates.Any())
			{
				var oppos = info.Other.Players.Where(p => p.Position.X > owner.Position.X).ToList();

				var safe = new List<Player>();

				foreach (var candidate in passCandidates)
				{
					var veloMin = candidate.Position - owner.Position;
					var veloMax = candidate.Position - owner.Position;
					var accuracy = Statistics.GetAccuracy(PassingPower, PassingZ);
					veloMin.Rotate(accuracy);
					veloMax.Rotate(-accuracy);

					var candidateDistance = (candidate.Position - owner.Position).Length -  Constants.PlayerMaxTackleDistance;
					candidateDistance*=candidateDistance;

					foreach (var oppo in oppos.Where(o => (o.Position - owner.Position).LengthSquared > candidateDistance))
					{
						var vector = oppo.Position - owner.Position;
						var thetaMin = Mathematics.GetAngle(vector, veloMin);
						var thetaMax = Mathematics.GetAngle(vector, veloMax);

						var thetaSafe = Passing.GetSafeTheta(owner, oppo, PassingPower);

						// those 
						if (thetaSafe > thetaMin && thetaSafe < thetaMax)
						{
							continue;
						}
					}
					safe.Add(candidate);
				}

				if (safe.Any())
				{
					var target = safe.OrderBy(s => (s.Position - Field.EnemyGoal.Center).LengthSquared).FirstOrDefault();
					owner.ActionShoot(target, PassingPower);
					return owner;
				}
			}
			// run or shoot

			if (owner.Position.X > 1800)
			{
				owner.ActionShootGoal();
			}
			else
			{
				owner.ActionGo(Field.EnemyGoal.Center);
			}
					
			return owner;
		}

		private static bool IsCandidate(Player ballOwener, Player candidate, float ownerDistanceGoal2)
		{
			var candidateDistanceGoal2 = (Field.EnemyGoal.Center - candidate.Position).LengthSquared;
			if (candidateDistanceGoal2 > ownerDistanceGoal2) { return false; }

			var ownDistance = (ballOwener.Position - candidate.Position).LengthSquared;

			if (ownDistance < 200 * 200 || ownDistance > 500 * 500) { return false; }

			return true;
		}
	}
}
