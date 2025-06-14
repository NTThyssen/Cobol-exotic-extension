// filepath: /home/futte/projects/projects/cobol-vscode-extensions/server/LanguageServer/Visitors/PreProcessorVisitor.cs
using Antlr4.Runtime.Misc;


namespace LanguageServer.Visitors
{
    /// <summary>
    /// Visitor for traversing the Cobol preprocessor parse tree.
    /// </summary>
    public class PreProcessorVisitor : CobolPreprocessorBaseVisitor<object?>
    {
        private  CopybookUnit _copybookUnit;

        private WorkspaceFileHandler _workspaceFileHandler;
        public PreProcessorVisitor(ICobolUnit copybookUnit, WorkspaceFileHandler workspaceFileHandler)
        {
            _copybookUnit = (CopybookUnit)copybookUnit;
            _workspaceFileHandler = workspaceFileHandler;
        }

        public override object? VisitStartRule([NotNull] CobolPreprocessorParser.StartRuleContext context)
        {
            // Visit all children by default
            return base.VisitStartRule(context);
        }


        public override object? VisitCopyStatement([NotNull] CobolPreprocessorParser.CopyStatementContext context)
        {

            // Handle the COPY statement specifically
            if (context.copySource() != null)
            {
                var path = _workspaceFileHandler.GetIncludeFileUri(context.copySource().GetText());

                if (path == null)
                {
                    return null;
                }
                var lines = _workspaceFileHandler.GetFileAllLines(path);
                _copybookUnit.CopybookName = context.copySource().GetText();
                _copybookUnit.Uri = path;
                _copybookUnit.CopybookText = string.Join("\n", lines);
                _copybookUnit.lineOffSet = lines.Length-1;
                
            }
            return base.VisitCopyStatement(context);
        }
    }
}