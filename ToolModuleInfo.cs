namespace FSCmd;

struct ToolModuleInfo
{
	public string Name;
	public string SingleLineHelp;

	public ToolModuleInfo (string name, string singleLineHelp)
	{
		Name = name;
		SingleLineHelp = singleLineHelp;
	}
}
