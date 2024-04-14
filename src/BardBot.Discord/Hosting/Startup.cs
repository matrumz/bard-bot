using BardBot.Common.Hosting;
using BardBot.Common.Hosting.Extensions;
using BardBot.Discord.Models.Configuration;

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

        services.AddSingleton(discordSocketConfig);
        services.AddSingleton<DiscordSocketClient>();
        services.AddOptions<DiscordOptions>()
            .BindConfiguration(DiscordOptions.Discord)
            .ValidateDataAnnotations()
            .AddValueAsSingleton();
        services.AddHostedService<Bot>();
    }
}
