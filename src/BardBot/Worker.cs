using Discord;
using Discord.WebSocket;

namespace BardBot;

public class Worker : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<Worker> _logger;
    private readonly DiscordSocketClient _client;

    public Worker(IConfiguration config, ILogger<Worker> logger)
    {
        _config = config;
        _logger = logger;
        _client = new DiscordSocketClient();
        _client.Log += Log;
    }

    private Task Log(LogMessage msg)
    {
        _logger.LogInformation(msg.Message);
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _client.LoginAsync(TokenType.Bot, _config["Bot:Token"]);
        await _client.StartAsync();

        await Task.Delay(-1, stoppingToken);
    }
}
