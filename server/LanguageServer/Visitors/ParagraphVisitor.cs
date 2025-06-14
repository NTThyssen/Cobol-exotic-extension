using Antlr4.Runtime.Misc;
using LanguageServer;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

public class ParagraphVisitor : CobolParserBaseVisitor<object>
{
    private readonly ParagraphUnit _unit;

    public ParagraphVisitor(ParagraphUnit unit)
    {
        _unit = unit;
    }

    public override object VisitSectionOrParagraph([NotNull] CobolParser.SectionOrParagraphContext context)
    {
        // Record only paragraphs (not sections)
        if (context.SECTION() == null)
        {
            var nameToken = context.Start;
            var range = new Range
            {
                Start = new Position(nameToken.Line - 1, nameToken.Column),
                End = new Position(context.Stop.Line - 1, context.Stop.Column + context.Stop.Text.Length)
            };
            _unit.AddParagraph(nameToken.Text, range);
        }
        return base.VisitSectionOrParagraph(context);
    }

    public override object VisitPerformProcedureStatement([NotNull] CobolParser.PerformProcedureStatementContext context)
    {
        var procName = context.procedureName();
        var startToken = procName.Start;
        var range = new Range
        {
            Start = new Position(startToken.Line - 1, startToken.Column),
            End = new Position(procName.Stop.Line - 1, procName.Stop.Column + procName.Stop.Text.Length)
        };
        _unit.AddCall(procName.GetText(), range);
        return base.VisitPerformProcedureStatement(context);
    }

    public override object VisitGoToStatement([NotNull] CobolParser.GoToStatementContext context)
    {
        foreach (var proc in context.procedureName())
        {
            var startToken = proc.Start;
            var range = new Range
            {
                Start = new Position(startToken.Line - 1, startToken.Column),
                End = new Position(proc.Stop.Line - 1, proc.Stop.Column + proc.Stop.Text.Length)
            };
            _unit.AddCall(proc.GetText(), range);
        }
        return base.VisitGoToStatement(context);
    }
}
