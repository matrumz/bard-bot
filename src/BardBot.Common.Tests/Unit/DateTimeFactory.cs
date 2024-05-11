using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace BardBot.Common.Tests.Unit;

[Trait("Category", "Unit")]
public class DateTimeFactoryStringTests
{

    /// <summary>
    /// A vanilla DateTimeFactory should be able to parse strings compatible with DateTime.Parse.
    /// </summary>
    /// <param name="input"></param>
    [Theory]
    [InlineData("2022-02-22 20:00")]
    [InlineData("2022-02-22 20:00+00")]
    [InlineData("2022-02-22 20:00+00:00")]
    [InlineData("2022-02-22 20:00-00")]
    [InlineData("2022-02-22 20:00-00:00")]
    [InlineData("2022-02-22 20:00:00")]
    [InlineData("2022-02-22 20:00:00+00")]
    [InlineData("2022-02-22 20:00:00+00:00")]
    [InlineData("2022-02-22 20:00:00-00")]
    [InlineData("2022-02-22 20:00:00-00:00")]
    [InlineData("2022-02-22 20:00:00Z")]
    [InlineData("2022-02-22 20:00Z")]
    [InlineData("2022-02-22")]
    [InlineData("2022-02-22+00")]
    [InlineData("2022-02-22+00:00")]
    [InlineData("2022-02-22-00")]
    [InlineData("2022-02-22-00:00")]
    [InlineData("2022-02-22T20:00")]
    [InlineData("2022-02-22T20:00+00")]
    [InlineData("2022-02-22T20:00+00:00")]
    [InlineData("2022-02-22T20:00-00")]
    [InlineData("2022-02-22T20:00-00:00")]
    [InlineData("2022-02-22T20:00:00")]
    [InlineData("2022-02-22T20:00:00+00")]
    [InlineData("2022-02-22T20:00:00+00:00")]
    [InlineData("2022-02-22T20:00:00-00")]
    [InlineData("2022-02-22T20:00:00-00:00")]
    [InlineData("2022-02-22T20:00:00Z")]
    [InlineData("2022-02-22T20:00Z")]
    [InlineData("2022-02-22Z")]
    internal void Parse_NativeDateTimes_Success(string input)
    {
        // Arrange
        var factory = new DateTimeFactory();
        // Act
        var actual = factory.Parse(input);
        // Assert
        Assert.Equal(DateTime.Parse(input), actual);
    }

    /// <summary>
    /// Test the common case in Discord chat export parsing.
    /// </summary>
    /// <param name="input"></param>
    [Theory]
    [MemberData(nameof(Generator.Parse_CustomDateTimes_Success), MemberType = typeof(Generator))]
    internal void Parse_CustomDateTimes_Success(string input, DateTime expected)
    {
        // Arrange
        var factory = new DateTimeFactory();
        var parsers = new DateTimeFactory.TryParser[]
        {
                (string? input, out DateTime result) =>
                {
                    switch (input)
                    {
                        case "$now":
                            result = DateTime.MaxValue;
                            return true;
                        case "$last":
                            result = DateTime.MinValue;
                            return true;
                        default:
                            result = DateTime.MinValue;
                            return false;
                    }
                }
        };
        var formats = new string[] { "yyyy-MM-dd-HHmm" };
        // Act
        var actual = factory.Parse(input, parsers: parsers, formats: formats);
        // Assert
        Assert.Equal(expected, actual);
    }

    private class Generator
    {

        public static IEnumerable<object[]> Parse_CustomDateTimes_Success()
        {
            yield return new object[] { "$now", DateTime.MaxValue };
            yield return new object[] { "$last", DateTime.MinValue };
            yield return new object[] { "2022-02-22-2000", new DateTime(2022, 2, 22, 20, 0, 0) };
        }

    }

}

[Trait("Category", "Unit")]
public class DateTimeFactoryYamlTests
{

    private record ValueObject
    {
        public DateTime Value { get; set; }
    }

    [Theory]
    [MemberData(nameof(Generator.CreateYamlTypeConverter_DeserializeCommonYamlValues_Success), MemberType = typeof(Generator))]
    internal void CreateYamlTypeConverter_DeserializeCommonYamlValues_Success(string input, DateTime expected)
    {
        // Arrange
        var factory = new DateTimeFactory();
        var converter = factory.CreateYamlTypeConverter(
            parsers:
            [
                (string? input, out DateTime result) =>
                {
                    switch (input)
                    {
                        case "$now":
                            result = DateTime.MaxValue;
                            return true;
                        case "$last":
                            result = DateTime.MinValue;
                            return true;
                        default:
                            result = DateTime.MinValue;
                            return false;
                    }
                }
            ],
            formats: ["yyyy-MM-dd-HHmm"]
        );
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(converter)
            .Build();
        // Act
        var actual = deserializer.Deserialize<ValueObject>(input);
        // Assert
        Assert.Equal(expected, actual.Value);
    }

    private class Generator
    {

        public static IEnumerable<object[]> CreateYamlTypeConverter_DeserializeCommonYamlValues_Success()
        {
            yield return new object[] { "value: $now", DateTime.MaxValue };
            yield return new object[] { "value: $last", DateTime.MinValue };
            yield return new object[] { "value: 2022-02-22-2000", new DateTime(2022, 2, 22, 20, 0, 0) };
        }

    }
}
