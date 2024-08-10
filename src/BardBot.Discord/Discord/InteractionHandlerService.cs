using System.Reflection;

using BardBot.Discord.Common;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BardBot.Discord.Discord;

internal sealed partial class InteractionHandlerService(
    DiscordSocketClient discordClient,
    ILogger<InteractionHandlerService> logger,
    InteractionService interactionService,
    IServiceProvider services
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        discordClient.InteractionCreated += OnInteraction;
        discordClient.Ready += () => interactionService.RegisterCommandsGloballyAsync(deleteMissing: true);
        interactionService.InteractionExecuted += InteractionExecuted;

        await interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), services);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        interactionService.Dispose();
        return Task.CompletedTask;
    }

    private async Task OnInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(discordClient, interaction);
            var result = await interactionService.ExecuteCommandAsync(context, services);
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
                    await context.Interaction.RespondAsync("Invalid arguments", ephemeral: true);
                    break;
                case InteractionCommandError.ConvertFailed:
                    _ = logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage, ephemeral: true);
                    break;
                case InteractionCommandError.Exception:
                    _ = logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage, ephemeral: true);
                    break;
                case InteractionCommandError.ParseFailed:
                    _ = logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage, ephemeral: true);
                    break;
                case InteractionCommandError.UnknownCommand:
                    await context.Interaction.RespondAsync("Unknown command", ephemeral: true);
                    break;
                case InteractionCommandError.UnmetPrecondition:
                    await context.Interaction.RespondAsync($"You do not have permission to run this command: {result.ErrorReason}", ephemeral: true);
                    break;
                case InteractionCommandError.Unsuccessful:
                    _ = logger.LogAsync(result);
                    await context.Interaction.RespondAsync(seeMaintainerMessage, ephemeral: true);
                    break;
            }
        }
        else
        {
            _ = logger.LogAsync(result);
        }
    }

}
