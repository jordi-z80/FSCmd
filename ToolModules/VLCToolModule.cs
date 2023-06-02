using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FSCmd;

internal class VLCToolModule : IToolModule
{
	public ToolModuleInfo[] Info => new ToolModuleInfo[]
	{
		new ("vlcPause", "Pauses VLC video.", new List < ToolParameterInfo >()
		{
			new ToolParameterInfo("--VLC_PORT","VLC HTTP port","Optional, defaults to 8080."),
			new ToolParameterInfo("--VLC_PASSWORD","Password","Optional, defaults to 'vlcremote'.")
		}),
		new ("vlcPlay", "Plays VLC video.", new List < ToolParameterInfo >()
		{
			new ToolParameterInfo("--VLC_PORT","VLC HTTP port","Optional, defaults to 8080."),
			new ToolParameterInfo("--VLC_PASSWORD","Password","Optional, defaults to 'vlcremote'.")
		}),
		new ("vlcNext", "Skips to the next VLC video.", new List < ToolParameterInfo >()
		{
			new ToolParameterInfo("--VLC_PORT","VLC HTTP port","Optional, defaults to 8080."),
			new ToolParameterInfo("--VLC_PASSWORD","Password","Optional, defaults to 'vlcremote'.")
		}),
		new ("vlcPrevious", "Goes back to the previous VLC video.", new List < ToolParameterInfo >()
		{
			new ToolParameterInfo("--VLC_PORT","VLC HTTP port","Optional, defaults to 8080."),
			new ToolParameterInfo("--VLC_PASSWORD","Password","Optional, defaults to 'vlcremote'.")
		}),

	};

	const string Host = "localhost";
	int Port = 8080;
	string Password = "vlcremote";

	

	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		setupParams ();

		switch (action)
		{
			case "vlcNext": sendMessage ("command=pl_next").Wait (); break;
			case "vlcPrevious": sendMessage ("command=pl_previous").Wait (); break;
			case "vlcPause": sendMessage ("command=pl_pause").Wait (); break;
			case "vlcPlay": sendMessage ("command=pl_play").Wait (); break;
			default: return false;
		}
		return true;
	}

	//=============================================================================
	/// <summary></summary>
	private void setupParams ()
	{
		string sPort = Program.Configuration["VLC_PORT"];
		if (sPort != null)
		{
			if (Int32.TryParse (sPort, out var port))
			{
				Port = port;
			}
		}

		string sPassword = Program.Configuration["VLC_PASSWORD"];
		if (sPassword != null) Password = sPassword;
	}


	//=============================================================================
	/// <summary></summary>
	async Task sendMessage(string msg)
	{
		string b64Password = Convert.ToBase64String (Encoding.ASCII.GetBytes ($":{Password}"));

		var client = new HttpClient ();
		client.BaseAddress = new Uri ($"http://{Host}:{Port}/");
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Basic", b64Password);
		client.DefaultRequestHeaders.Accept.Clear ();
		client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("text/xml"));

		try
		{
			// we send the message, we don't care about the response
			var response = await client.GetAsync ($"requests/status.xml?{msg}");
			if (response.IsSuccessStatusCode)
			{
				// we don't care about the response
				var responseString = await response.Content.ReadAsStringAsync ();
			}
			else
			{
				Console.WriteLine ($"Error sending message to VLC: {response.StatusCode}");
			}
		}
		catch (Exception e)
		{
			Console.WriteLine (e.Message);
		}

	}
}
