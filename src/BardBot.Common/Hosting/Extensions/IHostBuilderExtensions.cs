using Microsoft.Extensions.Hosting;

namespace BardBot.Common.Hosting.Extensions;

public static class IHostBuilderExtensions
{
    public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder) where TStartup : IStartup
    {
        var startup = Activator.CreateInstance<TStartup>();
        return hostBuilder
            .ConfigureAppConfiguration(startup.ConfigureAppConfiguration)
            .ConfigureServices(startup.ConfigureServices)
            ;
    }
}
