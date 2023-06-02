using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace FSCmd;

internal class VolumeToolVolume : IToolModule
{
	public ToolModuleInfo[] Info => new ToolModuleInfo[]
{
		new ("globalVolume", "Changes the global volume of the current device.",new List<ToolParameterInfo>()
		{
			new ("--set","[0..100]", "Volume value, in percentage."),
			new ("--relativeSet","[percentage]", "Volume change, relative. e.g. 50% = half volume, 200% = double.")
		})
};

	//=============================================================================
	/// <summary></summary>
	public bool Run (string action)
	{
		try
		{
			string set = Program.Configuration["set"];
			if (set != null) return setVolume (set);

			string relativeSet = Program.Configuration["relativeSet"];
			if (relativeSet != null) return setRelativeVolume (relativeSet);

			return true;
		}
		catch (Exception e)
		{
			Console.WriteLine (e.Message);
			return false;
		}
	}


	//=============================================================================
	/// <summary></summary>
	private MMDevice getMainDevice()
	{
		var enumerator = new MMDeviceEnumerator ();
		var device = enumerator.GetDefaultAudioEndpoint (DataFlow.Render, Role.Multimedia);
		return device;
	}

	//=============================================================================
	/// <summary></summary>
	private bool setVolume (string set)
	{
		if (!int.TryParse (set, out int value))
		{
			Console.WriteLine ($"Failed parsing value '{set}'");
			return false;
		}

		float volume = value / 100.0f;
		if (volume < 0f) volume = 0f;
		if (volume > 1f) volume = 1f;

		var device = getMainDevice ();
		device.AudioEndpointVolume.MasterVolumeLevelScalar = volume;

		return true;
		
	}

	//=============================================================================
	/// <summary></summary>
	private bool setRelativeVolume (string relativeSet)
	{
		if (!int.TryParse (relativeSet, out int value))
		{
			Console.WriteLine ($"Failed parsing value '{relativeSet}'");
			return false;
		}

		var device = getMainDevice ();

		float currentVolume = device.AudioEndpointVolume.MasterVolumeLevelScalar;

		float targetVolume = currentVolume * value / 100.0f;
		if (targetVolume < 0f) targetVolume = 0f;
		if (targetVolume > 1f) targetVolume = 1f;

		device.AudioEndpointVolume.MasterVolumeLevelScalar = targetVolume;

		return true;
	}


}
