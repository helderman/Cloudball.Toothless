using CloudBall.Engines.Toothless.Models;
using Common;
using System.Collections.Generic;
using System.Linq;

namespace CloudBall.Engines.Toothless.Roles
{
	public class CatchUp : IRole
	{
		public Player Apply(TurnInfo turn, IEnumerable<Player> queue)
		{
			if(turn.Own.Players.Contains(turn.Ball.Owner) ||
				turn.Own.Players.Any(p => p.CanPickUpBall(turn.Ball)))
			{
				return null;
			}

			var ourCatchup = turn.CatchUps.FirstOrDefault(c => turn.Own.Players.Contains(c.Player));

			if(ourCatchup != null)
			{
				ourCatchup.Player.ActionGo(ourCatchup.Position);
				return ourCatchup.Player;
			}
			return null;
		}
	}
}
