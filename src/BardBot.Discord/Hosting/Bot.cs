using System.Reflection;

using BardBot.Discord.Logging.Extensions;
using BardBot.Discord.Models.Configuration;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using static BardBot.Discord.Models.Configuration.DiscordOptions;

namespace BardBot.Discord.Hosting;

public class Bot : IHostedService
{
    private BotConfiguration Configuration { get; init; }
    private DiscordSocketClient DiscordClient { get; init; }
    private ILogger<Bot> Logger { get; init; }
    private InteractionService InteractionService { get; init; }
    private IServiceProvider Services { get; init; }

    public Bot(
        DiscordOptions options,
        DiscordSocketClient discordClient,
        ILogger<Bot> logger,
        InteractionService interactionService,
        IServiceProvider services
    )
    {
        Configuration = options.Bot;
        DiscordClient = discordClient;
        InteractionService = interactionService;
        Logger = logger;
        Services = services;

        DiscordClient.InteractionCreated += OnInteraction;
        DiscordClient.Log += Logger.LogAsync;
        DiscordClient.Ready += () => InteractionService.RegisterCommandsGloballyAsync(deleteMissing: true);
        InteractionService.InteractionExecuted += InteractionExecuted;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Bot is starting...");

        var addModulesTask = InteractionService.AddModulesAsync(Assembly.GetExecutingAssembly(), Services);

        await DiscordClient.LoginAsync(TokenType.Bot, Configuration.Token);
        await DiscordClient.StartAsync();

        await addModulesTask;

        Logger.LogInformation("Bot is started!");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Bot is stopping...");

        InteractionService.Dispose();

        await DiscordClient.StopAsync();
        await DiscordClient.LogoutAsync();

        Logger.LogInformation("Bot is stopped!");
    }

    private async Task OnInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(DiscordClient, interaction);
            var result = await InteractionService.ExecuteCommandAsync(context, Services);

            if (!result.IsSuccess)
                await context.Interaction.RespondAsync(result.ToString());
        }
        catch
        {
            if (interaction.Type == InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(msg => msg.Result.DeleteAsync());
        }
    }

    private async Task InteractionExecuted(ICommandInfo info, IInteractionContext context, IResult result)
    {
        if (!result.IsSuccess)
        {
            const string seeMaintainerMessage = "There was an issue with this command. Please contact the bot maintainer.";
            switch (result.Error)
            {
                case InteractionCommandError.BadArgs:
                    await context.Interaction.RespondAsync("Invalid arguments");
                    break;
                case InteractionCommandError.ConvertFailed:
                    _ = Logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage);
                    break;
                case InteractionCommandError.Exception:
                    _ = Logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage);
                    break;
                case InteractionCommandError.ParseFailed:
                    _ = Logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage);
                    break;
                case InteractionCommandError.UnknownCommand:
                    await context.Interaction.RespondAsync("Unknown command");
                    break;
                case InteractionCommandError.UnmetPrecondition:
                    await context.Interaction.RespondAsync($"You do not have permission to run this command: {result.ErrorReason}");
                    break;
                case InteractionCommandError.Unsuccessful:
                    _ = Logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage);
                    break;
            }
        }
        else
        {
            _ = Logger.LogAsync(result);
        }
    }

}
