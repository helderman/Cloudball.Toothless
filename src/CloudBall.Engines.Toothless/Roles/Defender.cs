using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public class Defender : IRole
	{
		public Common.Player Apply(Models.TurnInfo turn, IEnumerable<Common.Player> queue)
		{
			var player = queue.OrderBy(p => p.GetDistanceTo(Common.Field.MyGoal)).FirstOrDefault();
			if (player != null)
			{
				player.ActionGo(new Common.Vector((Common.Field.MyGoal.X + turn.Ball.Position.X) / 2, (Common.Field.MyGoal.Y + turn.Ball.Position.Y) / 2));
			}
			return player;
		}
	}
}
