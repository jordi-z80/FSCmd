using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.TaskScheduler;


namespace FSCmd;

internal class SetAlarmToolModule : IToolModule
{
	public ToolModuleInfo[] Info => new ToolModuleInfo[] 
	{ 
		new ("setAlarm", "Sets an alarm.", new List<ToolParameterInfo>()
		{
			new ("--text","Alarm text","Optional text to display on the alert."),
			new ("--deltaTime","[HH:]mm[:SS]","Time to wait before the alarm is triggered."),
			new ("--date","YYYY/MM/DD HH:mm:SS","Set an alarm for a specified date."),
			new ("--audioFile","Path to audio file","Optional audio file to play as alarm.")
		}) 
	};
	IConfigurationRoot Configuration => Program.Configuration;


	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		Debug.Assert (action == Info[0].Name);

		string reason = Configuration["text"];
		string deltaTime = Configuration["deltaTime"];
		string audioFile = Configuration["audioFile"];
		string date = Configuration["date"];

		deltaTime = null;
		date = "02 19:44";

		if (deltaTime == null && date == null) { Console.WriteLine ("Missing parameters."); return false; }
		if (deltaTime != null)
		{
			var ts = getDeltaTimeFromString (deltaTime);
			if (!ts.HasValue) { Console.WriteLine ("The deltaTime does not match the pattern."); return false; }

			DateTime targetTime = DateTime.Now + ts.Value;

			CreateTaskToRunFile (reason, targetTime, audioFile);
		}
		else if (date != null)
		{
			var dt = getTimestampFromString (date);
			if (!dt.HasValue) { Console.WriteLine ("The date does not match the pattern."); return false; }

			CreateTaskToRunFile (reason, dt.Value, audioFile);
		}

		return true;
	}

	//=============================================================================
	/// <summary></summary>
	private DateTime? getTimestampFromString (string date)
	{
		string pattern = @"((?<Year>\d{4})/)?((?<Month>\d{2})/)?(?<Day>\d{2})(\s(?<Hour>\d{2}):(?<Minute>\d{2})(:(?<Second>\d{2}))?)?";

		var match = Regex.Match (date, pattern);
		if (!match.Success) return null;

		int year = getGroup ("Year", DateTime.Now.Year);
		int month = getGroup ("Month", DateTime.Now.Month);
		int day = getGroup ("Day", DateTime.Now.Day);
		int hour = getGroup ("Hour", 0);
		int minute = getGroup ("Minute", 0);
		int second = getGroup ("Second", 0);

		return new DateTime (year, month, day, hour, minute, second);

		int getGroup (string grp, int def=0)
		{
			if (match.Groups[grp].Success) return int.Parse (match.Groups[grp].Value);
			return def;
		}
	}

	//=============================================================================
	/// <summary></summary>
	TimeSpan? getDeltaTimeFromString (string str)
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

			Console.WriteLine ($"Alarm set as a Task '{uniqueTaskName}' at {alarmTime.ToLongTimeString ()}");
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
