using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace BardBot.Common;

public partial class DateTimeFactory();


// String parsing
public partial class DateTimeFactory
{

    public delegate bool TryParser(string? input, out DateTime result);

    public DateTime Parse(string input, IEnumerable<TryParser>? parsers = null, IEnumerable<string>? formats = null) =>
        TryParse(input, out var result, parsers, formats) ? result : throw new FormatException("Failed to parse DateTime.");

    public bool TryParse(string input, [NotNullWhen(true)] out DateTime result, IEnumerable<TryParser>? parsers = null, IEnumerable<string>? formats = null)
    {

        // return the first successful parser
        foreach (var parser in parsers ?? [])
        {
            if (parser(input, out result))
            {
                return true;
            }
        }

        // return the first successful format
        foreach (var format in formats ?? [])
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return true;
            }
        }

        // try the default parser
        return DateTime.TryParse(input, out result);

    }

}

// Yaml parsing
public partial class DateTimeFactory
{

    public IYamlTypeConverter CreateYamlTypeConverter(
        IEnumerable<TryParser>? parsers = null,
        IEnumerable<string>? formats = null
    ) =>
        new YamlTypeConverter(
            factory: this,
            parsers: parsers,
            formats: formats
        );

    private class YamlTypeConverter(
        DateTimeFactory factory,
        IEnumerable<TryParser>? parsers = null,
        IEnumerable<string>? formats = null
    ) : IYamlTypeConverter
    {
        bool IYamlTypeConverter.Accepts(Type type) =>
            type == typeof(DateTime);
        object? IYamlTypeConverter.ReadYaml(IParser parser, Type type) =>
            parser.TryConsume<Scalar>(out var scalar)
                ? factory.Parse(scalar.Value, parsers, formats)
                : throw new FormatException("Failed to parse DateTime.");
        void IYamlTypeConverter.WriteYaml(IEmitter emitter, object? value, Type type) =>
            throw new NotImplementedException();
    }

}
