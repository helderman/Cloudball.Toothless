using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Models
{
	public class BallPath : List<Vector>
	{
		public enum Ending
		{
			OwnGoal,
			OtherGoal,
			EndOfGame,
		}

		public Ending End { get; protected set; }

		public IEnumerable<CatchUp> GetCatchUp(IEnumerable<Player> players, Ball ball)
		{

			foreach (var player in players)
			{
				var effectiveTurn = (ball.PickUpTimer > player.FallenTimer) ? ball.PickUpTimer : player.FallenTimer;

				for (var turn = effectiveTurn; turn < Count; turn++)
				{
					var disPlayer = Constants.PlayerMaxVelocity * turn + Constants.BallMaxPickUpDistance;
					var disBall = (this[turn] - player.Position).LengthSquared;

					if (disPlayer * disPlayer > disBall)
					{
						yield return new CatchUp()
						{
							Turn = turn,
							Player = player,
							Position = this[turn],
						};
						break;
					}
				}
			}
		}

		public static BallPath Create(TurnInfo info)
		{
			var path = new BallPath();
			var position = info.Ball.Position;
			var velocity = info.Ball.Velocity;

			path.Add(position);

			for (var i = 0; i < Constants.GameEngineMatchLength - info.Turn; i++)
			{
				velocity *= Constants.BallSlowDownFactor;
				position += velocity;

				if (position.X < Field.Borders.Left.X)
				{
					velocity.X = -velocity.X;
					position.X = 2 * Field.Borders.Left.X - position.X;
					if (position.Y > Field.MyGoal.Top.Y && position.Y < Field.MyGoal.Bottom.Y)
					{
						path.End = Ending.OwnGoal;
						return path;
					}
				}
				if (position.X > Field.Borders.Right.X)
				{
					velocity.X = -velocity.X;
					position.X = 2 * Field.Borders.Right.X - position.X;
					if (position.Y > Field.EnemyGoal.Top.Y && position.Y < Field.EnemyGoal.Bottom.Y)
					{
						path.End = Ending.OtherGoal;
						return path;
					}
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
