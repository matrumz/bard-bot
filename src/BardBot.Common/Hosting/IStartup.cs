using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Common.Hosting;

public interface IStartup
{
    public void ConfigureAppConfiguration(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) { }

    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) { }
}
