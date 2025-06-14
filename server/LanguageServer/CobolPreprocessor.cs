using Antlr4.Runtime;

namespace LanguageServer.Visitors
{
    public class CobolPreprocessor
    {

         private readonly CobolPreprocessorLexer lexer;
        private readonly ITokenStream tokenStream;
        public CobolPreprocessorParser parser { get;  set; }
        private readonly ParsingErrorListener errorListener;

        public CobolPreprocessor(string input)
        {
            var inputStream = new AntlrInputStream(input);
            lexer = new CobolPreprocessorLexer(inputStream);
            tokenStream = new CommonTokenStream(lexer);
            parser = new CobolPreprocessorParser(tokenStream);
            errorListener = new ParsingErrorListener();

            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);
        }

        public ParserRuleContext Parse()
        {
        
            var startContext = parser.startRule();

            return startContext;
        }

        public bool HasErrors => errorListener.HasErrors;
        public string[] Errors => errorListener.Errors.ToArray();
    }

    public class ParsingErrorListener : IAntlrErrorListener<IToken>
    {
        private readonly List<string> errors = new();
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            errors.Add($"Line {line}:{charPositionInLine} {msg}");
        }

        public bool HasErrors => errors.Count > 0;
        public IReadOnlyList<string> Errors => errors;
    }
}