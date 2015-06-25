using CloudBall.Engines.Toothless.Models;
using Common;
using System.Collections.Generic;

namespace CloudBall.Engines.Toothless.Scenarios
{
	public interface IScenario
	{
		bool Apply(TurnInfo info, IEnumerable<Player> players);
	}
}
