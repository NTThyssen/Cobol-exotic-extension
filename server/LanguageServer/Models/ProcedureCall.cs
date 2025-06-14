using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

public class ProcedureCall
{
    public string Name { get; set; }
    public Range Location { get; set; }

    public ProcedureCall(string name, Range location)
    {
        Name = name;
        Location = location;
    }
}
