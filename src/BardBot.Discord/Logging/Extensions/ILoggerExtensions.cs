using Discord;
using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Logging.Extensions;

public static class ILoggerExtensions
{
    public async static Task LogAsync(this ILogger logger, LogMessage message)
    {
        var severity = message.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };
        logger.Log(logLevel: severity, eventId: new EventId(), state: message, exception: message.Exception, formatter: (msg, _) => msg.Message);
        await Task.CompletedTask;
    }

    public async static Task LogAsync(this ILogger logger, IResult result)
    {
        if (result.IsSuccess)
        {
            logger.LogTrace(result.ToString());
        }
        else
        {
            logger.LogError(result.ToString());
        }
        await Task.CompletedTask;
    }
}
