using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public class Pickup : IRole
	{
		public Common.Player Apply(Models.TurnInfo turn, IEnumerable<Common.Player> queue)
		{
			var player = queue.FirstOrDefault(p => p.CanPickUpBall(turn.Ball));
			if (player != null)
			{
				player.ActionPickUpBall();
			}
			return player;
		}
	}
}
