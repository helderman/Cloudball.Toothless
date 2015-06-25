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
	}
}
