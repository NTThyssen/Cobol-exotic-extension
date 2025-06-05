using Antlr4.Runtime;

public class IdentificationUnit : ICobolUnit
    {
        public IdentificationUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
        }

        public string Uri { get; }
        public ParserRuleContext? Tree { get; }

        public string programName{ get; set; } = string.Empty;

    ParserRuleContext? ICobolUnit.Tree => throw new NotImplementedException();
}