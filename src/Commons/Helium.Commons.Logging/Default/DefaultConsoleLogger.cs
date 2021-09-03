namespace Helium.Commons.Logging.Default;

using System.Runtime.CompilerServices;

using Helium.Commons.Logging.Abstract;

public class DefaultConsoleLogger : ILogger
{
	/// <summary>
	/// The minimal log level required to post the entry to console
	/// </summary>
	public LogLevel MinimalLevel { get; set; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(String message)
	{
		this.LogInformation(message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(LogLevel level, String message)
	{
		this.LogMessage(new EventData(0, 0, 0, 0, null), level, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(EventData data, LogLevel level, String message)
	{
		switch (level)
		{
			case LogLevel.Debug:
				this.LogDebug(data, message);
				break;
			case LogLevel.Trace:
				this.LogTrace(data, message);
				break;
			case LogLevel.Info:
				this.LogInformation(data, message);
				break;
			case LogLevel.Warning:
				this.LogWarning(data, message);
				break;
			case LogLevel.Error:
				this.LogError(data, message);
				break;
			case LogLevel.Critical:
				this.LogCritical(data, message);
				break;
			case LogLevel.Fatal:
				this.LogFatal(data, message);
				break;

			default:
				throw new ArgumentException("The specified LogLevel was not recognized", nameof(level));
		}
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogDebug(String message)
	{
		this.LogDebug(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogDebug(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Debug)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Debug.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Debug.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogTrace(String message)
	{
		this.LogTrace(new EventData(0, 0, 0, 0, null), message);
	}

	public void LogTrace(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Trace)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Trace.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Trace.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogInformation(String message)
	{
		this.LogInformation(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogInformation(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Info)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Info.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Info.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogWarning(String message)
	{
		this.LogWarning(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogWarning(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Warning)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Warn.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Warn.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogError(String message)
	{
		this.LogError(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogError(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Error)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Error.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Error.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogCritical(String message)
	{
		this.LogCritical(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogCritical(EventData data, String message)
	{
		if (this.MinimalLevel > LogLevel.Critical)
		{
			return;
		}

		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.ForegroundColor = DefaultFormatting.Crit.ForegroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Crit.LogName}]"));
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($" {data}: {message}");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogFatal(String message)
	{
		this.LogFatal(new EventData(0, 0, 0, 0, null), message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogFatal(EventData data, String message)
	{
		Console.Write($"[{DateTimeOffset.Now}] ");
		Console.BackgroundColor = DefaultFormatting.Fatal.BackgroundColor;
		Console.Write(String.Format("{0:-5}", $"[{DefaultFormatting.Fatal.LogName}]"));
		Console.BackgroundColor = ConsoleColor.Black;
		Console.WriteLine($" {data}: {message}");
	}
}
