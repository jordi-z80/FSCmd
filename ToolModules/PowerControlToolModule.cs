using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCmd;

internal class PowerControlToolModule : IToolModule
{
	public string MultiLineHelp => @"hibernate, suspend";

	public ToolModuleInfo[] Info => new ToolModuleInfo[]
	{
		new ("hibernate","Puts the computer into hibernation.", new List <ToolParameterInfo>() { }),
		new ("sleep", "Puts the computer into sleep.", new List <ToolParameterInfo>()),
	};

	public bool Run (string action)
	{
		if (action == "hibernate") Application.SetSuspendState (PowerState.Hibernate, true, false);
		if (action == "sleep") Application.SetSuspendState (PowerState.Suspend, true, false);
		return true;
	}
}
