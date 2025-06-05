using Antlr4.Runtime;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Antlr4.Runtime.Tree;
using LanguageServer.Visitors;
using LanguageServer.Extensions;

namespace LanguageServer
{
    public class CobolLanguageServer
    {
        private readonly JsonRpc rpc;
        private readonly Dictionary<string, List<ICobolUnit>> documents;

        public CobolLanguageServer(JsonRpc rpc)
        {
            this.rpc = rpc;
            this.documents = new Dictionary<string, List<ICobolUnit>>();
            this.rpc.AddLocalRpcTarget(this);
        }

        private T? GetUnit<T>(string uri) where T : class, ICobolUnit
        {
            if (documents.TryGetValue(uri, out var units))
            {
                return units.OfType<T>().FirstOrDefault();
            }
            return null;
        }

        [JsonRpcMethod("initialize", UseSingleObjectParameterDeserialization =  true)]
        public async Task<InitializeResult> Initialize(InitializeParams @params)
        {
            // Scan workspace if root path is available
            if (@params.RootUri != null)
            {
                await ScanWorkspaceFolder(@params.RootUri, "includes");
            }
            else if (@params.RootUri != null)
            {
                var rootUri = new Uri(Path.GetFullPath(@params.RootUri.ToString()));
                await ScanWorkspaceFolder(rootUri, "includes");
            }

            return new InitializeResult
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
            };
        }

        [JsonRpcMethod("textDocument/didOpen", UseSingleObjectParameterDeserialization = true)]
        public async Task TextDocumentDidOpen(DidOpenTextDocumentParams @params)
        {
    
            var document = @params.TextDocument;
            await ParseAndAnalyzeDocument(document.Uri.ToString(), document.Text);
            
            
        }

        [JsonRpcMethod("textDocument/didChange", UseSingleObjectParameterDeserialization = true)]
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
                var parser = new CobolParserWrapper(text);
                var tree = parser.Parse();

                // Create units first
                var callUnit = new CallStatementUnit(documentUri, tree);
                var includeUnit = new IncludeUnit(documentUri, tree);
                // Create different visitors and units
                var callVisitor = new CallStatementVisitor(callUnit);
                var includeVisitor = new CallStatementVisitor(callUnit);

                callVisitor.Visit(tree);
                includeVisitor.Visit(tree);

                var units = new List<ICobolUnit>
                {
                    callUnit
                };

                documents[documentUri] = units;

                if (parser.HasErrors)
                {

                    // Report errors to the client
                    var diagnostics = parser.Errors.Select(error => new Diagnostic
                    {

                        Message = error,
                        Severity = DiagnosticSeverity.Error,
                        Source = "cobol",
                        Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                        {
                            Start = new Position(0, 0), // Placeholder, adjust as needed
                            End = new Position(0, 0) // Placeholder, adjust as needed
                        }
                    }).ToArray();

                    rpc.NotifyWithParameterObjectAsync("textDocument/publishDiagnostics", new Microsoft.VisualStudio.LanguageServer.Protocol.PublishDiagnosticParams
                    {
                        Uri = new Uri(documentUri),
                        Diagnostics = diagnostics,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error parsing document {documentUri}: {ex}");
            }

            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/didClose", UseSingleObjectParameterDeserialization = true)]
        public Task TextDocumentDidClose(DidCloseTextDocumentParams @params)
        {
            documents.Remove(@params.TextDocument.Uri.ToString());
            return Task.CompletedTask;
        }

        [JsonRpcMethod("textDocument/completion",UseSingleObjectParameterDeserialization = true)]
        public Task<CompletionList> TextDocumentCompletion(CompletionParams @params)
        {
            return Task.FromResult(new CompletionList());
        }

        [JsonRpcMethod("textDocument/hover", UseSingleObjectParameterDeserialization = true)]
        public Task<Hover> TextDocumentHover(TextDocumentPositionParams @params)
        {
            var callUnit = GetUnit<CallStatementUnit>(@params.TextDocument.Uri.ToString());
    if (callUnit == null) return Task.FromResult(new Hover());

    // Find if we're hovering over a CALL statement
    var callInfo = callUnit.CallStatements.FirstOrDefault(c => 
        c.Location.Contains(@params.Position));

    if (callInfo != null)
    {
        return Task.FromResult(new Hover
        {
            Contents = new MarkupContent
            {
                Kind = MarkupKind.Markdown,
                Value = $"```cobol\nCALL {callInfo.ProgramName}\n```"
            },
            Range = callInfo.Location
        });
    }

            return Task.FromResult(new Hover());
        }

        [JsonRpcMethod("textDocument/definition",UseSingleObjectParameterDeserialization = true)]
        public Task<Location> TextDocumentDefinition(TextDocumentPositionParams @params)
        {
            var location = new Location
            {
                Uri = @params.TextDocument.Uri,
                Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                {
                    Start = new Position(0, 0),
                    End = new Position(0, 0)
                }
            };
            return Task.FromResult(location);
        }

        private async Task ScanWorkspaceFolder(Uri workspaceFolderUri, string includesPath)
        {
            try
            {
                var folderPath = Path.Combine(new Uri(workspaceFolderUri.ToString()).LocalPath, includesPath);
                if (!Directory.Exists(folderPath))
                {
                    return;
                }

                var cobolFiles = Directory.GetFiles(folderPath, "*.cbl", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(folderPath, "*.cpy", SearchOption.AllDirectories));

                foreach (var file in cobolFiles)
                {
                    var fileUri = new Uri(file).ToString();
                    var content = await File.ReadAllTextAsync(file);
                    await ParseAndAnalyzeDocument(fileUri, content);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error scanning workspace folder: {ex}");
            }
        }
    }


}
