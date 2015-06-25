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

				if (position.X < Field.Borders.Left.X)
				{
					velocity.X = -velocity.X;
					position.X = 2 * Field.Borders.Left.X - position.X;
				}
				if (position.X > Field.Borders.Right.X)
				{
					velocity.X = -velocity.X;
					position.X = 2 * Field.Borders.Right.X - position.X;
				}
				if (position.Y < Field.Borders.Top.Y)
				{
					velocity.Y = -velocity.Y;
					position.Y = 2 * Field.Borders.Top.Y - position.Y;
				}
				if (position.Y > Field.Borders.Bottom.Y)
				{
					velocity.Y = -velocity.Y;
					position.Y = 2 * Field.Borders.Bottom.Y - position.Y;
				}

				path.Add(position);

			}
			path.End = Ending.EndOfGame;
			return path;
		}
	}
}
