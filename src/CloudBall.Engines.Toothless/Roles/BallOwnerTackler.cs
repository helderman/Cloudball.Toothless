using CloudBall.Engines.Toothless.Models;
using Common;
using System.Collections.Generic;
using System.Linq;

namespace CloudBall.Engines.Toothless.Roles
{
	public class BallOwnerTackler : IRole
	{
		public Player Apply(TurnInfo turn, IEnumerable<Player> queue)
		{
			if (turn.Ball.Owner == null) { return null; }

			var tackler = queue.FirstOrDefault(player => player.CanTackle(turn.Ball.Owner));
			if (tackler != null)
			{
				tackler.ActionTackle(turn.Ball.Owner);
			}
			return tackler;
		}
	}
}
