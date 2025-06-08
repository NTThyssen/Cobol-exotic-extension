// filepath: /home/futte/projects/projects/cobol-vscode-extensions/server/LanguageServer/Visitors/PreProcessorVisitor.cs
using Antlr4.Runtime.Misc;
using LanguageServer.grammar;

namespace LanguageServer.Visitors
{
    /// <summary>
    /// Visitor for traversing the Cobol preprocessor parse tree.
    /// </summary>
    public class PreProcessorVisitor : CobolPreprocessorBaseVisitor<object?>
    {
        public override object? VisitStartRule([NotNull] CobolPreprocessorParser.StartRuleContext context)
        {
            // Visit all children by default
            return base.VisitStartRule(context);
        }


        public override object? VisitCopyStatement([NotNull] CobolPreprocessorParser.CopyStatementContext context)
        {

            return base.VisitCopyStatement(context);
        }
    }
}