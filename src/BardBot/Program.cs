using BardBot.Common.Hosting.Extensions;

namespace BardBot;

public class Program
{
    public static void Main(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseStartup<Discord.Hosting.Startup>()
            .Build()
            .Run();
}
