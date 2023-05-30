using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.TaskScheduler;


namespace FSCmd;

internal class SetAlarmToolModule : IToolModule
{
	public string Name => "setAlarm";
	public string SingleLineHelp => "Sets an alarm.";
	public string MultiLineHelp => @"setAlarm
	set_alarm --text=""Reason""       ; Text to display on the alert
	--deltaTime=[hh:]mm[:ss]          ; Time to wait before the alarm is triggered
	[--audioFile=""Path to audio file""
";
	IConfigurationRoot Configuration => Program.Configuration;

	//=============================================================================
	/// <summary></summary>
	public bool Run ()
	{
		string reason = Configuration["text"];
		string deltaTime = Configuration["deltaTime"];
		string audioFile = Configuration["audioFile"];

		if (deltaTime == null) { Console.WriteLine ("Missing parameters."); return false; }

		var ts = getTimeFromString (deltaTime);
		if (!ts.HasValue) { Console.WriteLine ("The deltaTime does not match the pattern."); return false; }

		DateTime targetTime = DateTime.Now + ts.Value;

		CreateTaskToRunFile (reason,targetTime,audioFile);

		return true;
	}

	//=============================================================================
	/// <summary></summary>
	TimeSpan? getTimeFromString (string str)
	{

		string regEx = @"^(?:(?<hours>\d+):)?(?<minutes>\d+)(:(?<seconds>\d+))?$";
		var match = Regex.Match (str, regEx);
		if (!match.Success) return null;

		int hour = match.Groups["hours"].Success ? int.Parse (match.Groups["hours"].Value) : 0;
		int minute = match.Groups["minutes"].Success ? int.Parse (match.Groups["minutes"].Value) : 0;
		int second = match.Groups["seconds"].Success ? int.Parse (match.Groups["seconds"].Value) : 0;

		return new TimeSpan (hour, minute, second);
	}


	//=============================================================================
	/// <summary></summary>
	public void CreateTaskToRunFile (string reason, DateTime alarmTime,string audioFile=null)
	{
		// Get the service on the local machine
		using (TaskService ts = new TaskService ())
		{
			// Create a new task definition and assign properties
			TaskDefinition td = ts.NewTask ();
			td.RegistrationInfo.Description = "Task to create an alarm which will call back this tool to show a message.";

			// Create a trigger that will fire 10 minutes from now
			td.Triggers.Add (new TimeTrigger { StartBoundary = alarmTime });

			// we need an unique name
			string uniqueTaskName = "FSCmd_AlarmToolModule_" + Guid.NewGuid ().ToString ();

			// The alarm will call this very same FSCmd, using the "alert" tool
			string FSCmdPath = System.Reflection.Assembly.GetExecutingAssembly ().Location;

			// I'm having some problems on the debug version with the DLL, so I'll force the EXE
			if (Path.GetExtension (FSCmdPath) == ".dll") FSCmdPath=Path.ChangeExtension (FSCmdPath, ".exe");

			// two actions: First show alert, then delete the task
			td.Actions.Add (new ExecAction (FSCmdPath, getParamsForAlert (reason, audioFile)));
			td.Actions.Add (new ExecAction (FSCmdPath, getParamsForTaskDeletion (uniqueTaskName)));

			// Register the task in the root folder
			ts.RootFolder.RegisterTaskDefinition (uniqueTaskName, td);

			Console.Write ($"Alarm set as a Task '{uniqueTaskName}' at {alarmTime.ToLongTimeString ()}");
		}

	}

	//=============================================================================
	/// <summary></summary>
	string getParamsForTaskDeletion (string taskName)
	{
		return $"deleteTask --taskName=\"{taskName}\"";
	}

	//=============================================================================
	/// <summary></summary>
	string getParamsForAlert (string reason,string audioFile)
	{
		string parameters = "alert";
		if (reason != null)    parameters += $" --text=\"{reason}\"";
		if (audioFile != null) parameters += $" --audioFile=\"{audioFile}\"";
		return parameters;
	}


}
