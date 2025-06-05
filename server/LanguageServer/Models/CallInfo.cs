using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

public class CallInfo
{
    public string ProgramName { get; set; }
    public Range Location { get; set; }
    public string SourceText { get; set; }

    public CallInfo(string programName, Range location, string sourceText)
    {
        ProgramName = programName;
        Location = location;
        SourceText = sourceText;
    }
}