using TelegramBotApi.Models;
using LogLevel = TelegramBotApi.Models.LogLevel;

namespace TelegramBotApi.Services;

/// <summary>
///  Logger service to log the api and send it to the database.
/// </summary>
public class LoggerService
{
	private readonly LogService _logService;
	/// <summary>
	/// The constructor for the logger service.
	/// </summary>
	/// <param name="logService">The logservice instance</param>
	public LoggerService(LogService logService)
	{
		_logService = logService;
	}

	/// <summary>
	/// Create a log.
	/// </summary>
	/// <param name="message">The message</param>
	/// <param name="level">The log level</param>
	public async Task Log(LogLevel level,string message)
	{
			long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			Log log = new Log
			{
				Level = level,
				Message = message,
				Timestamp = timestamp
			};
			
			await _logService.CreateAsync(log);
	}
	
	
}