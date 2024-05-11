using System.Globalization;

namespace BardBot.Discord.Common;

internal sealed class DateTimeConverter
{

    public DateTime Now { get; init; } = DateTime.Now;

    public DateTime? Last { get; init; }

    public IEnumerable<string> CheckFormats { get; init; } = [];

    public bool TryParse(string? input, out DateTime? result)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            result = null;
            return false;
        }

        if (input.Equals("$now"))
        {
            result = Now;
            return true;
        }

        if (input.Equals("$last"))
        {
            result = Last;
            return Last is not null;
        }

        foreach (var format in CheckFormats)
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                result = parsed;
                return true;
            }
        }

        if (DateTime.TryParse(input, out var parsedDefault))
        {
            result = parsedDefault;
            return true;
        }

        result = null;
        return false;
    }

}
