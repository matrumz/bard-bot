using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BardBot.Common.Hosting;

public interface IStartup
{
    public void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services) { }
}
