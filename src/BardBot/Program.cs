namespace BardBot;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
            })
            .Build();

        host.Run();
    }
}
