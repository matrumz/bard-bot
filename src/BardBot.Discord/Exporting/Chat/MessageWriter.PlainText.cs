using System.Text;

using Discord;

namespace BardBot.Discord.Exporting.Chat;

internal class PlainTextMessageWriter(
    Stream stream
) : MessageWriter(stream)
{

    private readonly TextWriter _writer = new StreamWriter(stream);

    public override async ValueTask WriteMessageAsync(
        IMessage message,
        CancellationToken cancellationToken = default
    )
    {
        // await base.WriteMessageAsync(message, cancellationToken);

        // var content = message.Content;
        // if (string.IsNullOrWhiteSpace(content))
        // {
        //     return;
        // }

        // await using var writer = new StreamWriter(Stream, Encoding.UTF8, leaveOpen: true);
        // await writer.WriteLineAsync(content);
    }

    private record Epilogue
    {
        public override string ToString() => string.Empty;
    }

}
