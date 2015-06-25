using CloudBall.Engines.Toothless.Models;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBall.Engines.Toothless.Roles
{
	public interface IRole
	{
		Player Apply(TurnInfo turn, IEnumerable<Player> queue);
	}
}
