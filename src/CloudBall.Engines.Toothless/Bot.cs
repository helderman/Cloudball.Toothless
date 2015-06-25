using CloudBall.Engines.Toothless.Models;
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
			var turn = TurnInfo.Create(myTeam, enemyTeam, ball, matchInfo);
			

			var path = BallPath.Create(turn);

			foreach (Player player in myTeam.Players)
			{
				if (ball.Owner == player)
				{
					player.ActionShootGoal();
				}
				else
				{
					//player.ActionGo(ball);
					player.ActionGo(path[100]);
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
