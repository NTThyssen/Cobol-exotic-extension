// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as path from 'path';
import {
	ConfigurationParams,
	LanguageClient,
	LanguageClientOptions,
	ServerOptions,
	TransportKind
} from 'vscode-languageclient/node';

let client: LanguageClient;

// This method is called when your extension is activated
// Your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

	// Use the console to output diagnostic information (console.log) and errors (console.error)
	// This line of code will only be executed once when your extension is activated
	console.log('Congratulations, your extension "cobol-language-support" is now active!');

	// The command has been defined in the package.json file
	// Now provide the implementation of the command with registerCommand
	// The commandId parameter must match the command field in package.json
	const disposable = vscode.commands.registerCommand('cobol-language-support.helloWorld', () => {
		// The code you place here will be executed every time your command is executed
		// Display a message box to the user
		vscode.window.showInformationMessage('Hello World from COBOL Language Support!');
	});

	context.subscriptions.push(disposable);

	// Server is implemented in C# and built with dotnet
	const serverPath = context.asAbsolutePath(path.join('server', 'LanguageServer', 'bin', 'Debug', 'net9.0', 'LanguageServer.dll'));

	// Server options - using C# executable
	const serverOptions: ServerOptions = {
		run: {
			command: 'dotnet',
			args: [serverPath],
			transport: TransportKind.stdio
		},
		debug: {
			command: 'dotnet',
			args: [serverPath, '--debug'],
			transport: TransportKind.stdio,
        options: {
            env: {

                ...process.env,
                VSLS_SERVER_DEBUG: "true"
            }
        }
		}
	};

	// Options to control the language client
	const clientOptions: LanguageClientOptions = {
		documentSelector: [{ scheme: 'file', language: 'cobol' }],
		synchronize: {
			fileEvents: vscode.workspace.createFileSystemWatcher('**/*.{cbl,cob,cpy}'),
			configurationSection: "cobol"
		}
	};

	// Create the language client and start it
	client = new LanguageClient(
		'cobolLanguageServer',
		'COBOL Language Server',
		serverOptions,
		clientOptions
	);

	// Add error handling and logging
	client.onDidChangeState((e) => {
		console.log(`Client state changed: ${e.newState}`);
	});




    
	// Start the client. This will also launch the server
	try {
		console.log('Starting COBOL Language Client...');
		client.start().then(() => {
		const config = vscode.workspace.getConfiguration('cobol');
		client.sendNotification('workspace/didChangeConfiguration', {
			settings: { cobol: config }
		});
		});
	} catch (err) {
		console.error('Failed to start COBOL Language Client:', err);
	}

	  context.subscriptions.push({ dispose: () => client.stop() });

}

// This method is called when your extension is deactivated
export function deactivate() {
	if (!client) {
		return undefined;
	}
	return client.stop();
}
