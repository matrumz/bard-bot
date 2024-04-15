using Discord;
using Discord.Interactions;

using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Logging.Extensions;

public static class ILoggerExtensions
{
    public static Task LogAsync(this ILogger logger, LogMessage message)
    {
        switch (message.Severity)
        {
#pragma warning disable CA2254 // Template should be a static expression
            case LogSeverity.Verbose:
                logger.LogTrace(message.Exception, message.Message, message.Source);
                break;
            case LogSeverity.Debug:
                logger.LogDebug(message.Exception, message.Message, message.Source);
                break;
            case LogSeverity.Info:
                logger.LogInformation(message.Exception, message.Message, message.Source);
                break;
            case LogSeverity.Warning:
                logger.LogWarning(message.Exception, message.Message, message.Source);
                break;
            case LogSeverity.Error:
                logger.LogError(message.Exception, message.Message, message.Source);
                break;
            case LogSeverity.Critical:
                logger.LogCritical(message.Exception, message.Message, message.Source);
                break;
#pragma warning restore CA2254 // Template should be a static expression
        }
        return Task.CompletedTask;
    }

    public static Task LogAsync(this ILogger logger, IResult result)
    {
        if (result.IsSuccess)
        {
            logger.LogTrace(result.ToString());
        }
        else
        {
            logger.LogError(result.ToString());
        }
        return Task.CompletedTask;
    }
}
