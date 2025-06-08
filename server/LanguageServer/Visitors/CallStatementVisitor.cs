using System.Text;
using Antlr4.Runtime.Misc;
using LanguageServer.grammar;
using Antlr4.Runtime.Tree;
using CobolTranspiler; // Add this if CobolParserBaseVisitor is in this namespace
using Antlr4.Runtime;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace LanguageServer.Visitors
{
    public class CallstatementVisitor : CobolParserBaseVisitor<object>
    {
        private readonly CallStatementUnit _unit;

        public CallstatementVisitor(CallStatementUnit unit)
        {
            _unit = unit;
        }

        public override object VisitCallStatement(CobolParser.CallStatementContext context)
        {
            // Extract program name - assuming the COBOL grammar has a proper 
            // identifier or literal node for the program name
            var programNameContext = context.children[1].GetText();
            if (programNameContext == null) return null;

            string programName = programNameContext.Trim('\'', '"');

            // Create range for the CALL statement location
            
            var range = new Range
            {
                Start = new Position
                {
                    Line = context.Start.Line - 1,
                    Character = context.Start.Column
                },
                End = new Position
                {
                    Line = context.Stop.Line - 1,
                    Character = context.Stop.Column + context.Stop.Text.Length
                }
            };

            // Get the original source text
            string sourceText = context.generalIdentifier() != null
                ? context.generalIdentifier().GetText()
                : context.constantName().GetText().Replace("'", "");

            // Add to our unit
            _unit.AddCallStatement(programName, range, sourceText);

            return base.VisitCallStatement(context);
        }
    }
}