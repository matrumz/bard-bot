using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BardBot.Common.Hosting.Extensions;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> AddValueAsSingleton<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
        where TOptions : class
    {
        optionsBuilder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);
        return optionsBuilder;
    }
}
