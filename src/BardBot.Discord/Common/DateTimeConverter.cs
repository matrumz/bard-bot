using System.Globalization;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace BardBot.Discord.Common;

internal sealed class DateTimeConverter : IYamlTypeConverter
{

    public DateTime Now { get; init; } = DateTime.Now;

    public DateTime? Last { get; init; }

    public IEnumerable<string> CheckFormats { get; init; } = [];

    bool IYamlTypeConverter.Accepts(Type type)
    {
        return type == typeof(DateTime);
    }

    object? IYamlTypeConverter.ReadYaml(IParser parser, Type type)
    {
        var value = (parser.Current as Scalar)?.Value;
        parser.MoveNext();
        return TryParse(value, out var result) ? result : null;
    }

    void IYamlTypeConverter.WriteYaml(IEmitter emitter, object? value, Type type)
    {
        throw new NotImplementedException();
    }

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
