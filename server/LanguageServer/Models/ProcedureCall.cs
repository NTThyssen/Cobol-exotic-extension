using Microsoft.VisualStudio.LanguageServer.Protocol;

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
