
using System.Diagnostics;
using StreamJsonRpc;


namespace LanguageServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Add process ID to help with debugging
                       
                Console.InputEncoding = System.Text.Encoding.UTF8;
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                var processId = Process.GetCurrentProcess().Id;
                Console.Error.WriteLine($"[LSP Server] Starting COBOL Language Server (PID: {processId})");

                if (args.Contains("--debug") || Environment.GetEnvironmentVariable("VSLS_SERVER_DEBUG") == "true")
                {
                    Console.Error.WriteLine("[LSP Server] Waiting for debugger to attach...");
                    while (!Debugger.IsAttached)
                    {
                        await Task.Delay(1000);
                    }
                    Console.Error.WriteLine("[LSP Server] Debugger successfully attached!");
                }
                Console.Error.WriteLine("COBOL Language Server started");
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
