using Common;
using System;
using System.Collections.Generic;

namespace CloudBall
{
#if WINDOWS || XBOX
	static class RunCloudBall
	{
		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		static void Main(string[] args)
		{
			var teams = new List<ITeam>()
			{
				//TeamFactory.Load(@"*\Engines\*.dll"),
				new CloudBall.Engines.Toothless.Bot(),
				new CloudBall.Engines.SimpleStart(),
			};

			using (Client.Client client = new Client.Client(teams[0], teams[1]))
			{
				client.Run();
			}
		}
	}
#endif
}


