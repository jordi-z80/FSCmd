﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.TaskScheduler;

namespace FSCmd;

internal class DeleteTaskToolModule : IToolModule
{
	public ToolModuleInfo[] Info => new ToolModuleInfo[] 
	{ 
		new ("deleteTask", "Deletes task from the task scheduler.", new List < ToolParameterInfo >()
		{
			new ("--taskName","taskName", "Name of the task to be deleted from the task scheduler")

		}) 
	};


	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		Debug.Assert (action == Info[0].Name);

		string taskName = Program.Configuration["taskName"];
		if (taskName == null) { Console.WriteLine ("Invalid task name."); return false; }

		try
		{
			using (TaskService ts = new TaskService ())
			{
				// check if task exists
				var task = ts.FindTask (taskName);
				if (task == null) { Console.WriteLine ($"Task '{taskName}' does not exist."); return false; }

				// delete it
				ts.RootFolder.DeleteTask (taskName);
			}

			Console.WriteLine ($"Task '{taskName}' deleted.");
			return true;
		}
		catch (Exception ex)
		{
			// fail
			Console.WriteLine ($"Error deleting task '{taskName}'. Not enough privileges?");
			Console.WriteLine (ex.Message);
			return false;

		}
	}
}
