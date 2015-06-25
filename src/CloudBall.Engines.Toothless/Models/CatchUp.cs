using Common;

namespace CloudBall.Engines.Toothless.Models
{
	public class CatchUp
	{
		public Player Player { get; set; }
		public int Turn { get; set; }
		public Vector Position { get; set; }

		public override string ToString()
		{
			return string.Format("Turn: {0}, Pos: ({1:0}, {2:0}), Player: {3}",
				Turn, Position.X, Position.Y, (int)Player.PlayerType);
		}
	}
}
