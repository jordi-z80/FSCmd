using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSCmd.Forms;

namespace FSCmd;

internal class AlertToolModule : IToolModule
{
	public string Name => "alert";
	public string SingleLineHelp => "Displays an alert on the screen.";

	public string MultiLineHelp => @"alert 
	--text=""Text""                   ; Text to display
	--audioFile=""audioFile""         ; Path to audio file to play";

	

	//=============================================================================
	/// <summary></summary>
	public bool Run ()
	{
		ApplicationConfiguration.Initialize ();

		var form = new AlertForm ();
		form.setParameters (Program.Configuration["text"]);
		Application.Run (form);

		return true;
		
	}
}
