namespace Helium.Commons.Logging.Default;

public static class DefaultFormatting
{
	public static readonly LogFormatting Debug = new()
	{
		LogName = "Debug",
		ForegroundColor = ConsoleColor.DarkGreen,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Trace = new()
	{
		LogName = "Trace",
		ForegroundColor = ConsoleColor.Green,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Info = new()
	{
		LogName = "Info",
		ForegroundColor = ConsoleColor.DarkMagenta,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Warn = new()
	{
		LogName = "Warn",
		ForegroundColor = ConsoleColor.Yellow,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Error = new()
	{
		LogName = "Error",
		ForegroundColor = ConsoleColor.Red,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Crit = new()
	{
		LogName = "Crit",
		ForegroundColor = ConsoleColor.DarkRed,
		BackgroundColor = ConsoleColor.Black
	};

	public static readonly LogFormatting Fatal = new()
	{
		LogName = "Fatal",
		ForegroundColor = ConsoleColor.White,
		BackgroundColor = ConsoleColor.DarkRed
	};
}
