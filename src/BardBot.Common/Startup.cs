using BardBot.Common.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Common;

public class Startup : IStartup
{

    void IStartup.ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        services.AddSingleton<DateTimeFactory>();
    }

}
