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
		public int Turn { get { return Match.CurrentTimeStep; } }
		public Team Own { get; set; }
		public Team Other { get; set; }
		public Ball Ball { get; set; }
		public MatchInfo Match { get; set; }
		public BallPath Path { get; protected set; }
		public List<CatchUp> CatchUps { get; protected set; }
		public IEnumerable<Player> Players
		{
			get
			{
				foreach (var p in Own.Players) { yield return p; }
				foreach (var p in Other.Players) { yield return p; }
			}
		}

		public bool HasPossession { get; protected set; }

		public static TurnInfo Create(Team myTeam, Team enemyTeam, Ball ball, MatchInfo matchInfo)
		{
			var info = new TurnInfo()
			{
				Own = myTeam,
				Other = enemyTeam,
				Ball = ball,
				Match = matchInfo,
			};
			info.Path = BallPath.Create(info);
			info.CatchUps = info.Path.GetCatchUp(info.Players, info.Ball).OrderBy(c => c.Turn).ToList();
			info.HasPossession =
				myTeam.Players.Contains(ball.Owner) ||
				myTeam.Players.Any(p => p.CanPickUpBall(ball)) ||
				(info.CatchUps.Any() && myTeam.Players.Contains(info.CatchUps[0].Player));

			return info;
		}
	}
}
