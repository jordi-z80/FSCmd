namespace FSCmd;

struct ToolParameterInfo
{
	public string ParameterName;
	public string ParameterValueDescription;
	public string ParameterHelp;
	public ToolParameterInfo (string parameterName, string parameterValueDescription, string parameterHelp)
	{
		ParameterName = parameterName;
		ParameterValueDescription = parameterValueDescription;
		ParameterHelp = parameterHelp;
	}
}

struct ToolModuleInfo
{
	public string Name;
	public string SingleLineHelp;
	public List<ToolParameterInfo> Parameters;

	public ToolModuleInfo (string name, string singleLineHelp,List<ToolParameterInfo> parameterList)
	{
		Name = name;
		SingleLineHelp = singleLineHelp;
		Parameters = parameterList;
		if (Parameters == null) Parameters = new List<ToolParameterInfo> ();
	}
}
