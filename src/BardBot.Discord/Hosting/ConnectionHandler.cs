using BardBot.Discord.Logging.Extensions;
using BardBot.Discord.Models.Configuration;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Hosting;

internal class ConnectionHandler(
    DiscordOptions discordOptions,
    DiscordSocketClient discordClient,
    ILogger<ConnectionHandler> logger
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
