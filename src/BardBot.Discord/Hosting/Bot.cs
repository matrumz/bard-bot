using BardBot.Discord.Models.Configuration;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using static BardBot.Discord.Models.Configuration.DiscordOptions;

namespace BardBot.Discord.Hosting;

public class Bot : IHostedService
{
    private BotConfiguration Configuration { get; init; }
    private DiscordSocketClient Client { get; init; }
    private ILogger<Bot> Logger { get; init; }

    public Bot(
        DiscordOptions options,
        DiscordSocketClient client,
        ILogger<Bot> logger
    )
    {
        Client = client;
        Configuration = options.Bot;
        Logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Bot is starting...");

        await Client.LoginAsync(TokenType.Bot, Configuration.Token);
        await Client.StartAsync();

        Logger.LogInformation("Bot is started!");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Bot is stopping...");

        await Client.StopAsync();
        await Client.LogoutAsync();

        Logger.LogInformation("Bot is stopped!");
    }
}
