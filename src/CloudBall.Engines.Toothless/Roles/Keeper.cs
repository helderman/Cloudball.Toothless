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
					keeper.ActionWait();
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
			var sorted = queue.OrderBy(q => GetDistanceToGoalSquared(q.Position)).ToList();
			return queue
				.OrderBy(q => GetDistanceToGoalSquared(q.Position))
				.FirstOrDefault();
		}

		public float GetDistanceToGoalSquared(Vector position)
		{
			// above the goal
			if (position.Y <= Field.MyGoal.Top.Y)
			{
				return (Field.MyGoal.Top - position).LengthSquared;
			}
			if (position.Y >= Field.MyGoal.Bottom.Y)
			{
				return (Field.MyGoal.Bottom - position).LengthSquared;
			}
			var dX = Field.Borders.X - position.X;

			return dX * dX;
		}
	}
}
