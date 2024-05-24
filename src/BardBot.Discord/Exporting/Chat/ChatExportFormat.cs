namespace BardBot.Discord.Exporting.Chat;

internal enum ChatExportFormat
{
    PlainText
}

internal static class ChatExportFormatExtensions
{
    public static string GetFileExtension(this ChatExportFormat format) => format switch
    {
        ChatExportFormat.PlainText => "txt",
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
    };
}
