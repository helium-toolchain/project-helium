namespace Helium.Commons.Logging.Default;

using System.Runtime.CompilerServices;

using Helium.Commons.Logging.Abstract;

public class DefaultFileLogger : ILogger
{
	/// <summary>
	/// All log calls will log into this file
	/// </summary>
	public StreamWriter FileStream { get; set; }

	/// <summary>
	/// Minimal log level required for an entry to log
	/// </summary>
	public LogLevel MinimalLevel { get; set; }

	/// <summary>
	/// Create a new FileLogger instance using the name of the logfile
	/// </summary>
	public DefaultFileLogger(String Filename)
	{
		this.FileStream = new(Filename);
	}

	/// <summary>
	/// Create a new FileLogger instance using an already existing stream
	/// </summary>
	public DefaultFileLogger(Stream FileStream)
	{
		this.FileStream = new(FileStream);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Info, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(LogLevel level, String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), level, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogMessage(EventData data, LogLevel level, String message)
	{
		if (level < this.MinimalLevel)
		{
			return;
		}

		this.FileStream.Write($"[{DateTimeOffset.Now}] ");
		this.FileStream.Write(String.Format("{0:-5}", $"[{formatting[level].LogName}]"));
		this.FileStream.WriteLine($" {data}: {message}");
		this.FileStream.Flush();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogDebug(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Debug, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogDebug(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Debug, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogTrace(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Trace, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogTrace(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Trace, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogInformation(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Info, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogInformation(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Info, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogWarning(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Warning, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogWarning(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Warning, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogError(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Error, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogError(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Error, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogCritical(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Critical, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogCritical(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Critical, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogFatal(String message)
	{
		this.LogMessage(new(0, 0, 0, 0, null), LogLevel.Fatal, message);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void LogFatal(EventData data, String message)
	{
		this.LogMessage(data, LogLevel.Fatal, message);
	}

	private static readonly Dictionary<LogLevel, LogFormatting> formatting = new()
	{
		{ LogLevel.Debug, DefaultFormatting.Debug },
		{ LogLevel.Trace, DefaultFormatting.Trace },
		{ LogLevel.Info, DefaultFormatting.Info },
		{ LogLevel.Warning, DefaultFormatting.Warn },
		{ LogLevel.Error, DefaultFormatting.Error },
		{ LogLevel.Critical, DefaultFormatting.Crit },
		{ LogLevel.Fatal, DefaultFormatting.Fatal }
	};

	~DefaultFileLogger()
	{
		this.FileStream.Close();
	}
}
