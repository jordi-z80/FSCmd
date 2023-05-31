using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCmd;

internal interface IToolModule
{
	ToolModuleInfo[] Info { get; }

	bool Run (string action);						// show return false if the help should be shown, true otherwise
}
