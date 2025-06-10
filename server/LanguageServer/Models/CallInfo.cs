using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

public class CallInfo
{
    public string ProgramName { get; set; }
    public Range Location { get; set; }
    public List<string> PossibleVariableValues { get; set; } = new List<string>();

    public CallInfo(string programName, Range location, List<string>? possibleVariableValues = null)
    {
        ProgramName = programName;
        Location = location;
        PossibleVariableValues = possibleVariableValues ?? new List<string>();
    }
}