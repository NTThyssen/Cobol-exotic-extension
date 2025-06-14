using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

public class ParagraphInfo
{
    public string Name { get; set; }
    public Range Location { get; set; }

    public ParagraphInfo(string name, Range location)
    {
        Name = name;
        Location = location;
    }
}
