namespace BardBot.Discord.Exporting.TextChannel;

public enum ExportFormat
{
    PlainText
}

internal static class ExportFormatExtensions
{

    public static string GetFileExtension(this ExportFormat format) => format switch
    {
        ExportFormat.PlainText => ".txt",
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
    };

}
