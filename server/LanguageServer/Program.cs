using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LanguageServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Stream stdin = Console.OpenStandardInput();
                Stream stdout = Console.OpenStandardOutput();

                var messageHandler = new HeaderDelimitedMessageHandler(stdout, stdin);
                var jsonRpc = new JsonRpc(messageHandler);
                
                var server = new CobolLanguageServer(jsonRpc);
                
                // Start listening
                jsonRpc.StartListening();
                
                Console.Error.WriteLine("COBOL Language Server started");
                
                // Keep the application alive
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"COBOL Language Server error: {ex}");
                throw;
            }
        }
    }
}
