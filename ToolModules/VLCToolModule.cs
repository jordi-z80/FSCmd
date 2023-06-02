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
		}),
		new ("vlcPlay", "Plays VLC video.", new List < ToolParameterInfo >()
		{
		}),
		new ("vlcNext", "Skips to the next VLC video.", new List < ToolParameterInfo >()
		{
		}),
		new ("vlcPrevious", "Goes back to the previous VLC video.", new List < ToolParameterInfo >()
		{
		}),

	};

	// for the moment those values are hardcoded. (I have to implement common parameters for all the ToolModuleInfo members.
	const string Host = "localhost";
	const int Port = 8080;
	const string Password = "1234";

	

	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
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
