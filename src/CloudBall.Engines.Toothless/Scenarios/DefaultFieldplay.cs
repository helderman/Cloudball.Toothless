using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Scenarios
{

	/// <summary>Puts the remaining players in a proper position.</summary>
	/// <remarks>
	/// 
	///  o-------------------o-------------------o
	///  |                   |                   |
	///  |   left back       |     left forward  |
	///  |                  ~|~                  |
	///  |                /  |  \                |
	///  o-------------------o-------------------o
	///  |                \  |  /                |
	///  |                  _|_                  |
	///  |   right back      |    right forward  |
	///  |                   |                   |
	///  o-------------------o-------------------o
	/// </remarks>
	public class DefaultFieldplay : IScenario
	{
		[Flags]
		public enum Quadrant
		{
			Left = 0,
			Right = 1,
			Forward = 2,
			LB = Left,
			RB = Right,
			LF = Left | Forward,
			RF = Right | Forward,
		}

		public static float DistanceFromBallSquared = 220 * 220;
		public static float MinimumMiddleLine = 400;

		public bool Apply(TurnInfo info, IEnumerable<Player> players)
		{
			if (!players.Any()) { return true; }

			var queue = players.ToList();

			var x = GetMiddleLine(info.Ball);
			var y = Field.Borders.Center.Y;

			var lb = FieldSquare.Create(Quadrant.LB, x, y, info);
			var rb = FieldSquare.Create(Quadrant.RB, x, y, info);
			var lf = FieldSquare.Create(Quadrant.LF, x, y, info);
			var rf = FieldSquare.Create(Quadrant.RF, x, y, info);

			var squares = new List<FieldSquare>() { lb, rb, lf, rf };

			// First handle the players that are in the queue that are on their own square.
			foreach(var square in squares.Where(s => s.OwnPlayers.Count == 1 && queue.Contains(s.OwnPlayers[0])))
			{
				var player = square.OwnPlayers.FirstOrDefault();
				player.ActionGo(square.Target);
				queue.Remove(player);
			}
					
			foreach (var square in squares.Where(s => s.OwnPlayers.Count == 0))
			{
				var player = queue.OrderBy(p => (square.Target - p.Position).LengthSquared).FirstOrDefault();
				if (player != null)
				{
					player.ActionGo(square.Target);
					queue.Remove(player);
				}
			}

			foreach (var player in queue)
			{
				player.ActionWait();
			}
			return true;
		}

		public static float GetMiddleLine(IPosition ball)
		{
			if (ball.Position.X < Field.Borders.Center.X) 
			{
				return Math.Max(MinimumMiddleLine, ball.Position.X); 
			}
			return (Field.Borders.Center.X + ball.Position.X) / 2f;
		}

		public class FieldSquare : IComparable<FieldSquare>, IComparable
		{
			Quadrant Quadrant { get; set; }
			public List<Player> Players{get;set;}
			public List<Player> OwnPlayers { get; set; }
			public List<Player> OtherPlayers { get; set; }
			public List<Player> Assigned { get; set; }

			public Vector Target { get; set; }

			public static FieldSquare Create(Quadrant q, float x, float y, TurnInfo info)
			{
				var square = new FieldSquare()
				{
					Quadrant = q,
					Players = new List<Player>(),
					Assigned = new List<Player>(),
				};

				foreach(var p in info.Players)
				{
					if ((q.HasFlag(Quadrant.Forward) ? p.Position.X > x : p.Position.X <= x) &&
						(q.HasFlag(Quadrant.Right) ? p.Position.Y > y : p.Position.Y <= y))
					{
						square.Players.Add(p);
					}
				}

				var tX = q.HasFlag(Quadrant.Forward) ?  (x + Field.Borders.Right.X) / 2f:  x / 2f;
				var tY = Field.Borders.Bottom.Y * (q.HasFlag(Quadrant.Right) ? 0.75f : 0.25f);

				square.Target = new Vector(tX, tY);
				square.OwnPlayers = square.Players.Where(p => info.Own.Players.Contains(p)).ToList();
				square.OtherPlayers = square.Players.Where(p => info.Other.Players.Contains(p)).ToList();
				return square;
			}

			public int CompareTo(FieldSquare other)
			{
				var c = OwnPlayers.Count.CompareTo(other.OwnPlayers.Count);
				if (c != 0) { return c; }

				return other.OtherPlayers.Count.CompareTo(OtherPlayers.Count);
			}

			public int CompareTo(object obj)
			{
				return CompareTo(obj as FieldSquare);
			}

			public override string ToString()
			{
				return String.Format("{0}: Own: {1}, Other: {2}", Quadrant, OwnPlayers.Count, OtherPlayers.Count);
			}
		}
	}
}

