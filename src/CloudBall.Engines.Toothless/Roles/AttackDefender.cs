using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public class AttackDefender : IRole
	{
		public Common.Player Apply(Models.TurnInfo turn, IEnumerable<Common.Player> queue)
		{
			Player actor = null;

			// if the ball is rolling for the enemy goal
			if (turn.Path.End == BallPath.Ending.OtherGoal)
			{
				// get a list of all opponents between ball and goal; ordered by distance to ball (i.e. front man first; keeper last)
				var defenders = turn.CatchUps.Where(c => turn.Other.Players.Contains(c.Player)).OrderBy(c => c.Turn).Select(c => c.Player);
				if (defenders.Any())
				{
					// if some defender is in ball range AND in tackling range of someone in the queue, tackle now
					var picker = defenders.FirstOrDefault(o => o.CanPickUpBall(turn.Ball) && queue.Any(p => p.CanTackle(o)));
					if (picker != null)
					{
						actor = queue.FirstOrDefault(p => p.CanTackle(picker));
					}

					if (actor != null)
					{
						actor.ActionTackle(picker);
					}
					else
					{
						// Pick attacker and defender closest to each other
						actor = queue.OrderBy(p => defenders.Min(o => o.GetDistanceTo(p))).FirstOrDefault();
						if (actor != null)
						{
							picker = defenders.OrderBy(o => o.GetDistanceTo(actor)).FirstOrDefault();
							if (picker != null)
							{
								actor.ActionGo(picker);
							}
						}
					}
				}
			}
			return actor;
		}
	}
}
