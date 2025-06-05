using Antlr4.Runtime;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Antlr4.Runtime.Tree;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

namespace LanguageServer
{
    public class CobolLanguageServer
    {
        private readonly JsonRpc rpc;
        private readonly Dictionary<string, CompilationUnit> documents;

        public CobolLanguageServer(JsonRpc rpc)
        {
            this.rpc = rpc;
            this.documents = new Dictionary<string, CompilationUnit>();
            this.rpc.AddLocalRpcTarget(this);
        }

        [JsonRpcMethod("initialize")]
        public Task<InitializeResult> Initialize(InitializeParams @params)
        {
            return Task.FromResult(new InitializeResult
            {
                Capabilities = new ServerCapabilities
                {
                    TextDocumentSync = new TextDocumentSyncOptions
                    {
                        OpenClose = true,
                        Change = TextDocumentSyncKind.Full
                    },
                    CompletionProvider = new CompletionOptions
                    {
                        TriggerCharacters = new[] { "." }
                    },
                    HoverProvider = true,
                    DefinitionProvider = true
                }
            });
        }

        [JsonRpcMethod("textDocument/didOpen")]
        public async Task TextDocumentDidOpen(DidOpenTextDocumentParams @params)
        {
            var document = @params.TextDocument;
            await ParseAndAnalyzeDocument(document.Uri.ToString(), document.Text);
        }

        [JsonRpcMethod("textDocument/didChange")]
        public async Task TextDocumentDidChange(DidChangeTextDocumentParams @params)
        {
            var document = @params.TextDocument;
            var text = @params.ContentChanges[0].Text;
            await ParseAndAnalyzeDocument(document.Uri.ToString(), text);
        }

        private Task ParseAndAnalyzeDocument(string documentUri, string text)
        {
            try
            {
                var inputStream = new AntlrInputStream(text);
                // TODO: CobolLexer and CobolParser will be generated from grammar files
                // var lexer = new CobolLexer(inputStream);
                // var tokenStream = new CommonTokenStream(lexer);
                // var parser = new CobolParser(tokenStream);

                var compilationUnit = new CompilationUnit(documentUri, null); // Temporarily using null for Tree
                documents[documentUri] = compilationUnit;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error parsing document {documentUri}: {ex}");
            }

            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/didClose")]
        public Task TextDocumentDidClose(DidCloseTextDocumentParams @params)
        {
            documents.Remove(@params.TextDocument.Uri.ToString());
            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/completion")]
        public Task<CompletionList> TextDocumentCompletion(CompletionParams @params)
        {
            return Task.FromResult(new CompletionList());
        }

        [JsonRpcMethod("textDocument/hover")]
        public Task<Hover> TextDocumentHover(TextDocumentPositionParams @params)
        {
            return Task.FromResult(new Hover());
        }

        [JsonRpcMethod("textDocument/definition")]
        public Task<Location> TextDocumentDefinition(TextDocumentPositionParams @params)
        {
            var location = new Location 
            { 
                Uri = @params.TextDocument.Uri,
                Range = new Range
                {
                    Start = new Position(0, 0),
                    End = new Position(0, 0)
                }
            };
            return Task.FromResult(location);
        }
    }

    internal class CompilationUnit
    {
        public CompilationUnit(string uri, ParserRuleContext? tree)
        {
            Uri = uri;
            Tree = tree;
        }

        public string Uri { get; }
        public ParserRuleContext? Tree { get; }
    }
}
