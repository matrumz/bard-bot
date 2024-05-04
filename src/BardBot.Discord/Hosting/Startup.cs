using BardBot.Common.Hosting;
using BardBot.Common.Hosting.Extensions;
using BardBot.Discord.Database;
using BardBot.Discord.Discord;
using BardBot.Discord.Exporting;
using BardBot.Discord.Exporting.Chat;
using BardBot.Discord.Interactions;
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

        // Discord.NET
        services.AddSingleton(new DiscordSocketConfig()
        {
            UseInteractionSnowflakeDate = false // My computer system clock often drifts and I cannot vouch for the accuracy of my users' system clocks. This will prevent the bot from rejecting interactions that are "too old" when they are not.
        });
        services.AddSingleton(new InteractionServiceConfig()
        {
            InteractionCustomIdDelimiters = [':'],
            DefaultRunMode = RunMode.Async
        });
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<InteractionService>();

        // BardBot
        services.AddOptions<DiscordOptions>()
            .BindConfiguration(DiscordOptions.Discord)
            .ValidateDataAnnotations()
            .AddValueAsSingleton();
        services.AddHostedService<ConnectionHandlerService>();
        services.AddHostedService<InteractionHandlerService>();
        services.AddSingleton<ExportJobFactory>();
        services.AddTransient<ChatExportJob>();
        services.AddTransient<ICampaignRepository, MockCampaignRepository>();

    }
}
