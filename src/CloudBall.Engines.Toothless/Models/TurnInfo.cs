using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Models
{
	public class TurnInfo
	{
		public Team Own { get; set; }
		public Team Other { get; set; }
		public Ball Ball { get; set; }
		public MatchInfo Match { get; set; }
		public BallPath Path { get; set; }

		public IEnumerable<Player> Players
		{
			get
			{
				foreach (var p in Own.Players) { yield return p; }
				foreach (var p in Other.Players) { yield return p; }
			}
		}

		public static TurnInfo Create(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			var turn = new TurnInfo()
			{
				Own = myTeam,
				Other = enemyTeam,
				Ball = ball,
				Match = matchInfo,
			};
			turn.Path = BallPath.Create(turn);


			return turn;
		}
	}
}
