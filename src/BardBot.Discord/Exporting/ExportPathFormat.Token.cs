namespace BardBot.Discord.Exporting;

internal sealed partial class ExportPathFormat : string
{
    internal abstract class Token
    {
        public abstract string Format(string value);
    }
}
