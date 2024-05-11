using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Common.Hosting;

public class Startup : IStartup
{

    void IStartup.ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddSingleton<DateTimeFactory>();
    }

}
