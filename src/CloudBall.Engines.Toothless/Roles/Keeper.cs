using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBall.Engines.Toothless.Roles
{
	public class Keeper : IRole
	{
		public Common.Player Apply(Models.TurnInfo turn, IEnumerable<Common.Player> queue)
		{
			return null;
		}
	}
}
