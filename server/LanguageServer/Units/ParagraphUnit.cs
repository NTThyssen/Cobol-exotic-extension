using Antlr4.Runtime;
using System.Collections.Generic;

public class ParagraphUnit : ICobolUnit
{
    public ParagraphUnit(string uri, ParserRuleContext? tree)
    {
        Uri = uri;
        Tree = tree;
        Paragraphs = new List<ParagraphInfo>();
        Calls = new List<ProcedureCall>();
    }

    public string Uri { get; }
    public ParserRuleContext? Tree { get; }

    public List<ParagraphInfo> Paragraphs { get; }
    public List<ProcedureCall> Calls { get; }

    public void AddParagraph(string name, Microsoft.VisualStudio.LanguageServer.Protocol.Range location)
    {
        Paragraphs.Add(new ParagraphInfo(name, location));
    }

    public void AddCall(string name, Microsoft.VisualStudio.LanguageServer.Protocol.Range location)
    {
        Calls.Add(new ProcedureCall(name, location));
    }

    ParserRuleContext? ICobolUnit.Tree => throw new System.NotImplementedException();
}
