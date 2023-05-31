using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace FSCmd;



internal class Program
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

		// create configuration
		var builder = new ConfigurationBuilder ();
		builder.AddCommandLine (args);
		Configuration = builder.Build ();

		// check all tools, run the one specified
		foreach (var tool in toolModules)
		{
			var toolNames = tool.Info.Select (s => s.Name);
			if (toolNames.Contains (toolName))
			{
				if (!tool.Run (toolName))
				{
					showHelp (tool);
					return -1;
				}

				// valid execution
				return 0;
			}
		}

		// no tool found, show help
		showHelp (toolModules);
		return -1;

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
	private static void showHelp (IToolModule tool)
	{
		Console.WriteLine ("Usage: FSCmd " + tool.MultiLineHelp);
	}
}