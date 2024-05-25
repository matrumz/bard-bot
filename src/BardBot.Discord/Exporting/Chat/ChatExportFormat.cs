namespace BardBot.Discord.Exporting.Chat;

internal enum ChatExportFormat
{
    Markdown
}

internal static class ChatExportFormatExtensions
{

    public static string GetFileExtension(this ChatExportFormat format) => format switch
    {
        ChatExportFormat.Markdown => "md",
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
    };

}
