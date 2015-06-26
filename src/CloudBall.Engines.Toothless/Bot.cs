using CloudBall.Engines.Toothless.Models;
using CloudBall.Engines.Toothless.Roles;
using CloudBall.Engines.Toothless.Scenarios;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless
{
	[BotName("Toothless 1.0")]
	public class Bot : ITeam
	{
		protected static readonly IRole[] Roles = new IRole[]
		{
			Role.BallOwner,
			Role.Pickup,
			Role.CatchUp,
			Role.Keeper,
			Role.Sweeper,
		};

		public void Action(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			var info = TurnInfo.Create(myTeam, enemyTeam, ball, matchInfo);

			var queue = info.Own.Players.ToList();

			foreach (var role in Roles)
			{
				queue.Remove(role.Apply(info, queue));
			}
			Scenario.DefaultFieldplay.Apply(info, queue);
		}

		private void DumbAttack(TurnInfo turn)
		{
			var path = BallPath.Create(turn);
			var sortedPlayers = turn.Own.Players.OrderByDescending(p => p.Position.X);

			foreach (Player player in turn.Own.Players)
			{
				if (turn.Ball.Owner == player)
				{
					if (turn.Ball.GetDistanceTo(Field.EnemyGoal.Center) < 50)
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
					//player.ActionGo(turn.Ball);
					player.ActionGo(path[10]);
					if (player.CanPickUpBall(turn.Ball))
					{
						player.ActionPickUpBall();
					}
				}
				//player.ActionGo(new Vector(50, 50));
			}
		}

		private void SmartDefense(TurnInfo turn)
		{
			var unassigned = new List<Player>(turn.Own.Players);
			unassigned.Remove(_ballOwner.Apply(turn, unassigned));
			unassigned.Remove(_pickup.Apply(turn, unassigned));
			unassigned.Remove(_catchUp.Apply(turn, unassigned));
			unassigned.Remove(_keeper.Apply(turn, unassigned));
			unassigned.Remove(_sweeper.Apply(turn, unassigned));
			unassigned.Remove(_defender.Apply(turn, unassigned));
			unassigned.Remove(_defender.Apply(turn, unassigned));
			unassigned.Remove(_defender.Apply(turn, unassigned));
		}

		private Roles.IRole _ballOwner = new Roles.BallOwner();
		private Roles.IRole _pickup = new Roles.Pickup();
		private Roles.IRole _catchUp = new Roles.CatchUp();
		private Roles.IRole _keeper = new Roles.Keeper();
		private Roles.IRole _sweeper = new Roles.Sweeper();
		private Roles.IRole _defender = new Roles.Defender();

		private bool InitiativeIsOurs(TurnInfo turn)
		{
			var closestPlayer = turn.Ball.Owner ?? turn.CatchUps.First().Player;
			return turn.Own.Players.Contains(closestPlayer);
		}
	}
}
