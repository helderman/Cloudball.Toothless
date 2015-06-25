using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless
{
	[BotName("Toothless 0.1")]
	public class Bot : ITeam
	{
		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			foreach (Player player in myTeam.Players)
			{
				if (ball.Owner == player)
				{
					player.ActionShootGoal();
				}
				else
				{
					player.ActionGo(ball);
					if (player.CanPickUpBall(ball))
					{
						player.ActionPickUpBall();
					}
				}
				//player.ActionGo(new Vector(50, 50));
			}
		}
	}
}
