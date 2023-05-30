﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSCmd.Forms;
using Microsoft.Win32.TaskScheduler;

namespace FSCmd;

internal class AlertToolModule : IToolModule
{
	public string[] Name => new[] { "alert" };
	public string SingleLineHelp => "Displays an alert on the screen.";

	public string MultiLineHelp => @"alert 
	--text=""Text""                   ; Text to display
	--audioFile=""audioFile""         ; Path to audio file to play";



	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		Debug.Assert (action == Name[0]);

		ApplicationConfiguration.Initialize ();

		var form = new AlertForm ();
		form.setParameters (Program.Configuration["text"]);
		Application.Run (form);

		return true;
		
	}
}
