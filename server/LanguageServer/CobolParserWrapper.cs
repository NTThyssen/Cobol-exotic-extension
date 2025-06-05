using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using LanguageServer.grammar;
using System;
using System.Collections.Generic;

namespace LanguageServer
{
    public class CobolParserWrapper
    {
        private readonly CobolLexer lexer;
        private readonly ITokenStream tokenStream;
        public CobolParser parser { get;  set; }
        private readonly ParsingErrorListener errorListener;

        public CobolParserWrapper(string input)
        {
            var inputStream = new AntlrInputStream(input);
            lexer = new CobolLexer(inputStream);
            tokenStream = new CommonTokenStream(lexer);
            parser = new CobolParser(tokenStream);
            errorListener = new ParsingErrorListener();

            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);
        }

        public ParserRuleContext Parse()
        {
            // Get the start rule name from your CobolParser.g4
            var startContext = parser.startRule();

            return startContext;
        }

        public bool HasErrors => errorListener.HasErrors;
        public string[] Errors => errorListener.Errors.ToArray();
    }

    public class ParsingErrorListener : IAntlrErrorListener<IToken>
    {
        private readonly List<string> errors = new();

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            errors.Add($"Line {line}:{charPositionInLine} {msg}");
        }

        public bool HasErrors => errors.Count > 0;
        public IReadOnlyList<string> Errors => errors;
    }
}
