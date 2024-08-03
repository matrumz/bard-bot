using Microsoft.Extensions.DependencyInjection;

namespace BardBot.Discord.Exporting.TextChannel;

public sealed class ExportJobFactory(
    IServiceProvider serviceProvider
)
{

    public ExportJob GetExportJob(ExportContext context)
        => ActivatorUtilities.CreateInstance<ExportJob>(serviceProvider, context);

}
