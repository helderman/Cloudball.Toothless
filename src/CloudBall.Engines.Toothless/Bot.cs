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

			var sortedPlayers = myTeam.Players.OrderByDescending(p => p.Position.X);

			foreach (Player player in myTeam.Players)
			{
				if (ball.Owner == player)
				{
					if (ball.GetDistanceTo(Field.EnemyGoal.Center) < 50)
					{
						player.ActionShootGoal();
					}
					else if (player == sortedPlayers.First())
					{
						player.ActionGo(Field.EnemyGoal.Center);
					}
					else
					{
						player.ActionShoot(sortedPlayers.First());
					}
				}
				else
				{
					//player.ActionGo(ball);
					player.ActionGo(path[10]);
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
