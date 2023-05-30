using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCmd;

internal class PowerControlToolModule : IToolModule
{
	public string[] Name => new[] { "hibernate", "sleep"  };
	public string SingleLineHelp => "Allows hibernation, sleep.";
	public string MultiLineHelp => @"hibernate, suspend";

	public bool Run (string action)
	{
		if (action == "hibernate") Application.SetSuspendState (PowerState.Hibernate, true, false);
		if (action == "sleep") Application.SetSuspendState (PowerState.Suspend, true, false);
		return true;
	}
}
