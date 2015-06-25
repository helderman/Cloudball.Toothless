using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudBall.Engines.Toothless.Roles
{
	public class Sweeper : IRole
	{
		private const int CloseToTheGoal = 700;
		private const int TackleDistance = 100;

		public Player Apply(TurnInfo turn, IEnumerable<Player> queue)
		{
			var enemyInfo = EnemyDistanceToGoal(turn.Other.Players).OrderBy(c => c.Distance).FirstOrDefault();

			if (enemyInfo.Distance < CloseToTheGoal)
			{
				var playerInfo = PlayerDistanceToEnemy(queue, enemyInfo.Player).OrderBy(c => c.Distance).FirstOrDefault();
				if (playerInfo.Distance < TackleDistance)
				{
					playerInfo.Player.ActionTackle(enemyInfo.Player);
				}
				else
				{
					playerInfo.Player.ActionGo(enemyInfo.Player.Position);
				}
				return playerInfo.Player;
			}

			return null;
		}

		private IEnumerable<PlayerDistance> EnemyDistanceToGoal(IEnumerable<Player> EnemyPlayers)
		{
			foreach (var enemyPlayer in EnemyPlayers)
			{
				yield return new PlayerDistance()
				{
					Player = enemyPlayer,
					Distance = Field.MyGoal.Position.GetDistanceTo(enemyPlayer),
				};
			}
		}

		private IEnumerable<PlayerDistance> PlayerDistanceToEnemy(IEnumerable<Player> Players, Player Enemy)
		{
			foreach (var player in Players)
			{
				yield return new PlayerDistance()
				{
					Player = player,
					Distance = player.GetDistanceTo(Enemy.Position),
				};
			}
		}
	}
}
