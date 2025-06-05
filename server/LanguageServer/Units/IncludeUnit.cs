using Antlr4.Runtime;

public class IncludeUnit : ICobolUnit
    {
        public IncludeUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
        }

        public string Uri { get; }
        public ParserRuleContext? Tree { get; }

    ParserRuleContext? ICobolUnit.Tree => throw new NotImplementedException();
}