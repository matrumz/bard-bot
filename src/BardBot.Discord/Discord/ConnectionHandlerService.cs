using BardBot.Discord.Common;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Discord;

internal sealed class ConnectionHandlerService(
    DiscordOptions discordOptions,
    DiscordSocketClient discordClient,
    ILogger<ConnectionHandlerService> logger
) : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discordClient.Log += logger.LogAsync;

        await discordClient.LoginAsync(TokenType.Bot, discordOptions.Bot.Token);
        await discordClient.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }

}
