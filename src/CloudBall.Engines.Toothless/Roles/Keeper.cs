using CloudBall.Engines.Toothless.Models;
using Common;
using System.Collections.Generic;
using System.Linq;

namespace CloudBall.Engines.Toothless.Roles
{
	public class Keeper : IRole
	{
		private static readonly Vector DefaultPositon = new Vector(150, Field.MyGoal.Center.Y);

		public Player Apply(TurnInfo turn, IEnumerable<Player> queue)
		{
			var keeper = GetClosestBy(queue);

			if (keeper != null)
			{
				var catchup = turn.CatchUps.FirstOrDefault();

				// give it just a best try.
				if (catchup == null)
				{
					keeper.ActionGo(turn.Ball);
				}
				else 
				{
					var vector = (catchup.Position - Field.MyGoal.Center);
					vector.Normalize();

					var target = Field.MyGoal.Center + vector * 150;
					keeper.ActionGo(target);
				}
			}
			return keeper;
		}

		private Player GetClosestBy(IEnumerable<Player> queue)
		{
			return queue
				.OrderBy(q => GetDistanceToGoalSquared(q.Position))
				.FirstOrDefault();
		}

		public float GetDistanceToGoalSquared(Vector position)
		{
			// above the goal
			if (position.Y <= Field.EnemyGoal.Top.Y)
			{
				return (Field.EnemyGoal.Top - position).LengthSquared;
			}
			if (position.Y >= Field.EnemyGoal.Bottom.Y)
			{
				return (Field.EnemyGoal.Bottom - position).LengthSquared;
			}
			var dY = Field.Borders.Y - position.Y;

			return dY * dY;
		}
	}
}
