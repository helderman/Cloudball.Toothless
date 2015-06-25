using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public class BallOwner : IRole
	{
		public Common.Player Apply(Models.TurnInfo turn, IEnumerable<Common.Player> queue)
		{
			var player = queue.FirstOrDefault(p => p == turn.Ball.Owner);
			if (player != null)
			{
				var closestOpponent = turn.Other.Players.Where(p => p.GetDistanceTo(Common.Field.EnemyGoal) < player.GetDistanceTo(Common.Field.EnemyGoal)).OrderBy(p => p.GetDistanceTo(player)).First();
				if (!turn.Other.Players.Where(p => p.CanTackle(player) || p.GetDistanceTo(Common.Field.EnemyGoal) < player.GetDistanceTo(Common.Field.EnemyGoal)).Any())
				{
					// Just keep running, they will never catch up with you! Shoot softly at the very last moment.
					if (player.GetDistanceTo(Common.Field.EnemyGoal) < 3)
					{
						player.ActionShootGoal(1);
					}
					else
					{
						player.ActionGo(Common.Field.EnemyGoal);
					}
				}
				else if (player.GetDistanceTo(Common.Field.EnemyGoal) < 700)
				{
					// Take a shot at the goal; if we miss, we might succeed on the rebound.
					player.ActionShootGoal();
				}
				else
				{
					var closestTeamMate = turn.Own.Players.OrderBy(p => p.GetDistanceTo(player)).First();
					var distance = closestTeamMate.GetDistanceTo(player);
					if (distance > 200 && distance < 500)
					{
						player.ActionShoot(closestTeamMate);
					}
					else
					{
						player.ActionGo(Common.Field.EnemyGoal);
					}
				}
			}
			return player;
		}
	}
}
