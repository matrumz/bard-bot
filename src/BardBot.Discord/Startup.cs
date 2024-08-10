using BardBot.Common.Hosting;
using BardBot.Common.Hosting.Extensions;
using BardBot.Discord.Common;
using BardBot.Discord.Discord;
using BardBot.Discord.Exporting;
using BardBot.Discord.Exporting.TextChannel;
using BardBot.Discord.Mocks;

using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Discord;

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
            .BindConfiguration(DiscordOptions.ConfigSectionPath)
            .ValidateDataAnnotations()
            .AddValueAsSingleton();
        services.AddHostedService<ConnectionHandlerService>();
        services.AddHostedService<InteractionHandlerService>();
        services.AddSingleton<ExportWriterFactory>();

        // Mocks
        services.AddTransient<ICampaignRepository, MockCampaignRepository>();
        services.AddTransient<IChatExportHistoryRepository, MockChatExportHistoryRepository>();

    }
}
