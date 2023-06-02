using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace FSCmd;



internal static class Program
{
	public static IConfigurationRoot Configuration { get; private set; }


	//=============================================================================
	/// <summary></summary>
	[STAThread]
	static int Main (string[] args)
	{
		var toolModules = enumerateAllClassesImplementing<IToolModule> ();

		// no arguments, show help
		if (args.Length < 1)
		{
			showHelp (toolModules);
			return -1;
		}

		// the first argument must be the tool name
		string toolName = args[0];

		// special case: help
		if (toolName == "help")
		{
			if (args.Length > 1)
			{
				// particular help
				toolName = args[1];
				showHelp (toolModules, toolName);
			}
			else
			{
				showHelp (toolModules);
			}
			return -1;
		}

		// create configuration
		var builder = new ConfigurationBuilder ();
		builder.AddCommandLine (args);
		builder.AddEnvironmentVariables ();
		Configuration = builder.Build ();

		// find the tool
		var tool = getToolByName (toolModules, toolName);

		// tool not found
		if (tool == null)
		{
			Console.WriteLine ($"Tool '{toolName}' not found.");
			showHelp (toolModules);
			return -1;
		}

		// run the tool
		if (!tool.Run (toolName))
		{
			showHelp (toolModules, toolName);
			return -1;
		}
		return 0;
	}

	//=============================================================================
	/// <summary></summary>
	static IToolModule getToolByName (List<IToolModule> toolModules, string name)
	{
		foreach (var tool in toolModules)
		{
			var toolNames = tool.Info.Select (s => s.Name);
			if (toolNames.Contains (name)) return tool;
		}

		return null;
	}

	//=============================================================================
	/// <summary></summary>
	static List<T> enumerateAllClassesImplementing<T> ()
	{
		var types = AppDomain.CurrentDomain.GetAssemblies ()
			.SelectMany (s => s.GetTypes ())
			.Where (p => typeof (T).IsAssignableFrom (p) && !p.IsInterface)
			.Select (p => (T)Activator.CreateInstance (p))
			;
		return types.ToList ();
	}

	//=============================================================================
	/// <summary></summary>
	private static void showHelp (List<IToolModule> toolModules)
	{
		Console.WriteLine ("Usage: FSCmd <tool> [tool options]");
		Console.WriteLine ("Available tools: ");
		Console.WriteLine ("\tFSCmd help [tool]");
		foreach (var tool in toolModules)
		{
			foreach (var info in tool.Info)
			{
				string str = $"\tFSCmd {info.Name} ";
				while (str.Length < 30) str += " ";
				str += $"{info.SingleLineHelp}";

				Console.WriteLine (str);
			}
		}
	}

	//=============================================================================
	/// <summary></summary>
	private static void showHelp (List<IToolModule> toolModules, string toolName)
	{
		var tool = getToolByName (toolModules, toolName);
		if (tool == null) return;

		var info = tool.Info.FirstOrDefault (p => p.Name == toolName);

		Console.WriteLine ($"FSCmd {info.Name} : {info.SingleLineHelp}\n");

		if (info.Parameters.Count <= 0)
		{
			Console.WriteLine ($"Usage: FSCmd {toolName} [no parameters]");
			return;
		}

		Console.WriteLine ($"Usage: FSCmd {toolName} [tool options]");


		foreach (var parm in info.Parameters)
		{
			string str= $"   {parm.ParameterName}";
			str += $"=\"{parm.ParameterValueDescription}\"";

			while (str.Length < 48) str += " ";
			str += $"{parm.ParameterHelp}";
			Console.WriteLine (str);
		}
	}
}