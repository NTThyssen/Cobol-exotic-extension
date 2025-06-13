using Microsoft.VisualStudio.LanguageServer.Protocol;

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
