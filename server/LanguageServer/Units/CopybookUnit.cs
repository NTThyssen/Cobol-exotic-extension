using Antlr4.Runtime;

public class CopybookUnit : ICobolUnit
    {
        public CopybookUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
        }

        public string Uri { get; }
        public ParserRuleContext? Tree { get; }

    ParserRuleContext? ICobolUnit.Tree => throw new NotImplementedException();
}