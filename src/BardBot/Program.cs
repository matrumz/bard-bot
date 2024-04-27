using BardBot.Common.Hosting.Extensions;

namespace BardBot;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) => config.AddJsonFile("appsettings.local.json", optional: true))
            .UseStartup<Discord.Hosting.Startup>()
            .Build()
            .Run();
}
