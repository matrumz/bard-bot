using Discord;
using Discord.WebSocket;

namespace BardBot.Discord.Discord;

public static class IMessageChannelExtensions
{

    public static async IAsyncEnumerable<IMessage> GetMessagesAsync(
        this IMessageChannel channel,
        DateTime? after = null,
        DateTime? before = null,
        int batchSize = 100,
        RequestOptions? options = null
    )
    {
        after ??= Snowflake.MinValue;
        before ??= Snowflake.MaxValue;
        ulong scanStart = SnowflakeUtils.ToSnowflake(after.Value);
        var done = false;
        do
        {
            // fetch the next batch of messages
            var messagesBatch = await channel.GetMessagesAsync(scanStart, Direction.After, limit: batchSize, options: options).FlattenAsync();
            // note where we'll start the next batch (if applicable)
            scanStart = messagesBatch.Last().Id;
            // reduce the batch based on the before parameter
            messagesBatch = messagesBatch.Where(m => m.Timestamp < before.Value);
            // stop if there are no more messages
            done = !messagesBatch.Any();
            // yield the messages
            foreach (var message in messagesBatch)
                yield return message;
        } while (!done);
    }

    public static ICategoryChannel? GetCategory(this IMessageChannel channel)
        => channel is INestedChannel nestedChannel
            ? nestedChannel.GetCategoryAsync().Result
            : null
            ;

    public static string? GetTopic(this IMessageChannel channel)
        => channel is ITextChannel textChannel
            ? textChannel.Topic
            : null
            ;

    public static IMessageChannel GetSelfChannelOrParentChannel(this IMessageChannel channel)
        => channel is SocketThreadChannel threadChannel
            ? (IMessageChannel)threadChannel.ParentChannel
            : channel
            ;

    public static IThreadChannel? GetThread(this IMessageChannel channel)
        => channel is IThreadChannel threadChannel
            ? threadChannel
            : null
            ;

}
