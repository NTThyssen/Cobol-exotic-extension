using System.Text;
using Antlr4.Runtime.Misc;

using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace LanguageServer.Visitors
{
    public  class IdentificationVisitor : CobolParserBaseVisitor<object>
    {

        private readonly IdentificationUnit _unit;
        public IdentificationVisitor(IdentificationUnit unit)
        {
            _unit = unit;
        }
        public override object VisitProgramIdParagraph(CobolParser.ProgramIdParagraphContext context)
        {

            string programName = context.programName().GetText();

            _unit.programName = programName;

            return base.VisitProgramIdParagraph(context);
        }
    }
}