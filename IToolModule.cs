using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCmd;

internal interface IToolModule
{
	string Name { get; }
	string SingleLineHelp { get; }
	string MultiLineHelp { get; }

	bool Run ();						// show return false if the help should be shown, true otherwise
}
