using BardBot.Common.Hosting;
using BardBot.Common.Hosting.Extensions;
using BardBot.Discord.Models.Configuration;

using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Discord.Hosting;

public sealed class Startup : IStartup
{
    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        var discordSocketConfig = new DiscordSocketConfig()
        {
            UseInteractionSnowflakeDate = false, // My computer system clock often drifts and I cannot vouch for the accuracy of my users' system clocks. This will prevent the bot from rejecting interactions that are "too old" when they are not.
        };

        services.AddHostedService<ConnectionHandler>();
        services.AddHostedService<InteractionHandler>();
        services.AddOptions<DiscordOptions>()
            .BindConfiguration(DiscordOptions.Discord)
            .ValidateDataAnnotations()
            .AddValueAsSingleton();
        services.AddSingleton(discordSocketConfig);
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
    }
}
