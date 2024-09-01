using BardBot.Discord.Common;
using BardBot.Discord.Discord;

using Discord;
using Discord.WebSocket;

using FluentAssertions;
using FluentAssertions.Execution;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BardBot.Discord.Tests.Integration.Discord
{
    [Trait("Category", "Integration")]
    public sealed class IMessageChannelExtensions : IDisposable
    {

        public IMessageChannelExtensions()
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
                .Build();

            // Bind configuration to DiscordOptions
            _options = new();
            configuration.GetSection(DiscordOptions.ConfigSectionPath).Bind(_options);

            // Initialize DiscordSocketClient
            _client = new(new DiscordSocketConfig() { UseInteractionSnowflakeDate = false });

            // Create a TaskCompletionSource to wait for the Connected event
            TaskCompletionSource<bool> connectedTcs = new();
            _client.Connected += () =>
            {
                connectedTcs.SetResult(true);
                return Task.CompletedTask;
            };

            // Login and start the client
            _client.LoginAsync(TokenType.Bot, _options.Bot.Token).Wait();
            _client.StartAsync().Wait();

            // Wait to be connected
            connectedTcs.Task.Wait(60000);
        }

        private readonly DiscordOptions _options;
        private readonly DiscordSocketClient _client;

        [Fact]
        internal void ClientLoginStart_DoneInConstructor_HaveWorkingClient()
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(ConnectionState.Connected, _client.ConnectionState);
        }

        [Fact]
        internal async void GetMessagesAsync_KnownRangeFromChannelInTestServer_ReturnsMessages()
        {
            // Arrange
            var channelId = _options
                .MockCampaignRepository
                .Single()
                .Channels.Values
                .Where(c => (c.ExportableGameChat ?? false) && c.Character == null)
                .Single()
                .Id
                ;
            var channel = (IMessageChannel)await _client.GetChannelAsync(channelId);

            // Act
            var messages = await channel.GetMessagesAsync(
                // after: new DateTime(2024, 4, 18, 21, 18, 0),
                // before: new DateTime(2024, 4, 18, 21, 20, 0),
                batchSize: 1,
                options: new RequestOptions()
                {
                    RetryMode = RetryMode.AlwaysRetry
                }
            ).ToListAsync();

            // Assert
            messages.Should()
                .NotBeEmpty("because I posted messages in the test channel")
                .And.HaveCount(2)
                ;
            using (new AssertionScope())
            {
                messages[0].Content.Should().Be("Test message 1");
                messages[1].Content.Should().Be("Test message 2");
            }
        }

        void IDisposable.Dispose()
        {
            _client.StopAsync().Wait();
            _client.LogoutAsync().Wait();
        }
    }
}
