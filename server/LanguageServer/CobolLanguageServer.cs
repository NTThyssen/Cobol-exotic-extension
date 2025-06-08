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
using Newtonsoft.Json.Linq;

namespace LanguageServer
{
    public class CobolLanguageServer
    {
        private readonly JsonRpc rpc;
        private readonly Dictionary<string, List<ICobolUnit>> documents;

        private readonly Dictionary<string, string> programNamesToUri = new Dictionary<string, string>();

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
                    DefinitionProvider = true,
                    
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

                var preprocessor = new CobolPreprocessor(text);
                var preTree = preprocessor.Parse();
                
                var preVisitor = new PreProcessorVisitor();
                preVisitor.Visit(preTree);

                
                var parser = new CobolParserWrapper(text);
                var tree = parser.Parse();

                // Create units first, these will be manipulated by the visitors
                var identificationUnit = new IdentificationUnit(documentUri, tree);
                var callUnit = new CallStatementUnit(documentUri, tree);
                var includeUnit = new IncludeUnit(documentUri, tree);

                // Create different visitors and units
                var callVisitor = new CallStatementVisitor(callUnit);
                var includeVisitor = new CallStatementVisitor(callUnit);
                var identificationVisitor = new IdentificationVisitor(identificationUnit);

                callVisitor.Visit(tree);
                includeVisitor.Visit(tree);
                identificationVisitor.Visit(tree);

                var units = new List<ICobolUnit>
                {
                    callUnit,
                    identificationUnit
                };

                documents[documentUri] = units;
                if(identificationUnit.programName != null && !string.IsNullOrEmpty(identificationUnit.programName))
                {
                    // Add the program name to the dictionary
                    if (!programNamesToUri.ContainsKey(identificationUnit.programName))
                    {
                        programNamesToUri[identificationUnit.programName] = documentUri;
                    }
                }

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

        [JsonRpcMethod("textDocument/definition", UseSingleObjectParameterDeserialization = true)]
        public Task<Location[]> TextDocumentDefinition(TextDocumentPositionParams @params)
        {
            var callUnit = GetUnit<CallStatementUnit>(@params.TextDocument.Uri.ToString());
            if (callUnit == null) return Task.FromResult(Array.Empty<Location>());

            // Find if we're clicking on a CALL statement
            var callInfo = callUnit.CallStatements.FirstOrDefault(c =>
                c.Location.Contains(@params.Position));

            if (callInfo != null)
            {
                // Here we would analyze the program name and determine possible targets
                // For now, let's simulate two possible targets as an example
                var possibleLocations = new List<Location>();



          
                    var files = callInfo.ProgramName.Split(',')
                        .Select(name => programNamesToUri.ContainsKey(name.Trim()) ? programNamesToUri[name.Trim()] : null)
                        .Where(uri => uri != null)
                        .Distinct()
                        .ToList();

                    foreach (var file in files)
                    {
                        possibleLocations.Add(new Location
                        {
                            Uri = new Uri(file),
                            Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                            {
                                Start = new Position(0, 0),
                                End = new Position(0, 0)
                            }
                        });
                    }
                

                // If we found any locations, return them
                if (possibleLocations.Count > 0)
                {
                    return Task.FromResult(possibleLocations.ToArray());
                }
                var location = new Location
                {
                    Uri = new Uri(@params.TextDocument.Uri.ToString()),
                    Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(0, 0),
                        End = new Position(0, 0)
                    }
                };
                var location2 = new Location
                {
                    Uri = new Uri(@params.TextDocument.Uri.ToString()),
                    Range = new Microsoft.VisualStudio.LanguageServer.Protocol.Range
                    {
                        Start = new Position(1, 0),
                        End = new Position(1, 0)
                    }
                };
                // If no locations found, return empty array
                var y = new List<Location> { location, location2 };
                return Task.FromResult(y.ToArray());
            }
            return Task.FromResult(Array.Empty<Location>());
        }

        private async Task ScanWorkspaceFolder(Uri workspaceFolderUri, string includesPath)
        {
            try
            {
                var workspacePath = new Uri(workspaceFolderUri.ToString()).LocalPath;

                // Get workspace settings
                var configurationParams = new ConfigurationParams
                {
                    Items = new[]
                    {
                        new ConfigurationItem
                        {
                            Section = "cobol"
                        }
                    }
                };


                var foldersToScan = new List<string>(){workspacePath};
               /* 
                    var response = await rpc.InvokeAsync<JToken[]>("workspace/configuration", configurationParams);
                    var programPath = response?[0]?["programPath"]?.ToString();

                    
                    
                    // If no response or invalid configuration, use default workspace path
                    if (response == null || programPath == null)
                    {
                        foldersToScan.Add(workspacePath);
                        Console.WriteLine("No custom program path configured, using workspace root");
                    }
                    else 
                    {
                        var customPath = programPath.StartsWith("/") 
                            ? programPath // Absolute path
                            : Path.Combine(workspacePath, programPath); // Relative path
                        
                        if (Directory.Exists(customPath))
                        {
                            foldersToScan.Add(customPath);
                        }
                        else
                        {
                            Console.WriteLine($"Configured program path '{customPath}' does not exist, using workspace root");
                            foldersToScan.Add(workspacePath);
                        }
                    }*/


                // Always add includes path if it exists
                var includesFolderPath = Path.Combine(workspacePath, includesPath);
                if (Directory.Exists(includesFolderPath))
                {
                    foldersToScan.Add(includesFolderPath);
                }

                foreach (var folder in foldersToScan)
                {
                    var cobolFiles = Directory.GetFiles(folder, "*.cbl", SearchOption.AllDirectories);
                        

                    foreach (var file in cobolFiles)
                    {
                        var fileUri = new Uri(file).ToString();
                        var content = await File.ReadAllTextAsync(file);
                        await ParseAndAnalyzeDocument(fileUri, content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error scanning workspace folder: {ex}");
            }
        }
    }


}
