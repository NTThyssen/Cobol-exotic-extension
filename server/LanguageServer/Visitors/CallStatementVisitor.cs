using System.Text;
using Antlr4.Runtime.Misc;
using LanguageServer.grammar;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;
using CobolDevExtension;

namespace LanguageServer.Visitors
{
    public class CallstatementVisitor : CobolParserBaseVisitor<object>
    {
        private SymbolTable _symbolTable = SymbolTable.Instance;
        private readonly CallStatementUnit _unit;
        private List<string> possibleVariableValues = new List<string>();
        public CallstatementVisitor(CallStatementUnit unit)
        {
            _unit = unit;
        }


        public override object VisitSetStatement([NotNull] CobolParser.SetStatementContext context)
        {
    
            var varNameParent = context.setToStatement().receivingField()[0].generalIdentifier().qualifiedDataName().variableUsageName().GetText();
            var nameChildValue = context.setToStatement().sendingField().generalIdentifier().qualifiedDataName().variableUsageName().GetText();
            CobolDataVariable variable = _symbolTable.GetDataNode(varNameParent);

            foreach (var element in variable.Children)
            {
                if (element.Name != nameChildValue) continue;
                
                possibleVariableValues.Add(element.Value.Trim('\''));
            }

            return base.VisitSetStatement(context);
        }


        public override object VisitSectionOrParagraph([NotNull] CobolParser.SectionOrParagraphContext context)
        {
            possibleVariableValues.Clear();
            return base.VisitSectionOrParagraph(context);
        }

        public override object VisitCallStatement(CobolParser.CallStatementContext context)
        {

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
            if(context.generalIdentifier() != null)
            {
                _unit.AddCallStatement(context.generalIdentifier().GetText(), range, possibleVariableValues);
            }

            if(context.constantName() != null)
            {
                var programName = context.constantName().GetText().Replace("'", "");
                _unit.AddCallStatement(programName, range);
            }          

            return base.VisitCallStatement(context);
        }
    }
}