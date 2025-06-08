using Antlr4.Runtime;

public class CopybookUnit : ICobolUnit
    {
        public CopybookUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
        }

        public string Uri { get; set; } = string.Empty;
        public string CopybookName { get; set; } = string.Empty;
        public string CopybookText { get; set; } = string.Empty;

        public int lineOffSet { get; set; } = 0;
        public ParserRuleContext? Tree { get; }

    ParserRuleContext? ICobolUnit.Tree => throw new NotImplementedException();
}