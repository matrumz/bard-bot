using BardBot.Common.Hosting;
using BardBot.Common.Hosting.Extensions;
using BardBot.Discord.Models.Configuration;

using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Discord.Hosting;

public class Startup : IStartup
{
    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        var discordSocketConfig = new DiscordSocketConfig()
        {
            // ...
        };

        var interactionServiceConfig = new InteractionServiceConfig()
        {
            DefaultRunMode = RunMode.Async,
        };

        services.AddHostedService<Bot>();
        services.AddOptions<DiscordOptions>()
            .BindConfiguration(DiscordOptions.Discord)
            .ValidateDataAnnotations()
            .AddValueAsSingleton();
        services.AddSingleton(discordSocketConfig);
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();
    }
}
