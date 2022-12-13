namespace Helium.Api.Commands;

using System;

/// <summary>
/// Controls the different flags that can be associated with a Helium command
/// </summary>
[Flags]
public enum HeliumCommandNodeFlags : Byte
{
	/// <summary>
	/// This node is a literal command node.
	/// </summary>
	Command = 0b0000_0001,

	/// <summary>
	/// This node is an argument node.
	/// </summary>
	Argument = 0b0000_0010,

	/// <summary>
	/// This node is the final node of the command tree.
	/// </summary>
	Final = 0b0000_0100,

	/// <summary>
	/// This node can be executed.
	/// </summary>
	Executable = 0b0000_1000,

	/// <summary>
	/// This node redirects to a different node.
	/// </summary>
	Redirect = 0b0001_0000,

	/// <summary>
	/// The client should ask the server under any circumstances for suggestions.
	/// </summary>
	SuggestionsAskServer = 0b0010_0000,

	/// <summary>
	/// The client should never ask the server for suggestions.
	/// </summary>
	SuggestionsClientside = 0b0100_0000,

	/// <summary>
	/// Suggestions should not be rendered for this command.
	/// </summary>
	SuggestionsDisabled = 0b1000_0000,


	/// <summary>
	/// This argument lasts until the end of the string.
	/// </summary>
	UnterminatedArgument = Argument | Final,

	/// <summary>
	/// This redirection is directly executable.
	/// </summary>
	ExecutableRedirect = Executable | Redirect
}
