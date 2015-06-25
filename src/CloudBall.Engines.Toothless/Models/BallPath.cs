using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Models
{
	public class BallPath: List<Vector>
	{
		public enum Ending
		{
			OwnGoal,
			OtherGoal,
			EndOfGame,
		}

		public Ending End { get; protected set; }

		public IEnumerable<CatchUp> GetCatchUp(IEnumerable<Player> players)
		{
			return null;
		}

		
		public static BallPath Create(TurnInfo turn)
		{
			var path = new BallPath();

			var position = turn.Ball.Position;
			var velocity = turn.Ball.Velocity;

			path.Add(position);

			for (var i = 0; i < 1000; i++)
			{
				velocity *= Constants.BallSlowDownFactor;
				position += velocity;

				//if (!Field.Borders.Contains(position))
				//{
				//	// bounce; must adjust position and velocity
				//	Field.Borders.
				//}

				path.Add(position);

			}
			path.End = Ending.EndOfGame;
			return path;
		}
	}
}
