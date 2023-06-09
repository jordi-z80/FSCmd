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
	public ToolModuleInfo[] Info => new ToolModuleInfo[] 
	{ 
		new ("alert", "Displays an alert on the screen.",new List<ToolParameterInfo>()
		{
			new ("--text","Text to display", "Optional text to display in the alert."),
			new ("--audioFile","Path to audio file", "Optional file to play when the alert is displayed.")

		}) 
	};


	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		Debug.Assert (action == Info[0].Name);

		ApplicationConfiguration.Initialize ();

		var form = new AlertForm ();
		form.setParameters (Program.Configuration["text"]);
		Application.Run (form);

		return true;
		
	}
}
