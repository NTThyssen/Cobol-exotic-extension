using Antlr4.Runtime;
    public class CallStatementUnit : ICobolUnit
    {
        public CallStatementUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
            CallStatements = new List<CallInfo>();
        }

        public string Uri { get; }
        public ParserRuleContext? Tree { get; }
        public List<CallInfo> CallStatements { get; }

        // Helper method to add found CALL statements
        public void AddCallStatement(string programName, Microsoft.VisualStudio.LanguageServer.Protocol.Range location, string sourceText)
        {
            CallStatements.Add(new CallInfo(programName, location, sourceText));
        }
    }


