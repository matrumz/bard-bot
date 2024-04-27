using BardBot.Discord.Exporting.PathTokens;
using BardBot.Discord.Tests.Generators;

namespace BardBot.Discord.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class StringExtensions
    {

        [Theory]
        [MemberData(nameof(TemplateTokensResultGenerator.StringExtensions_ApplyTokens_FilePathSubstitutions_Success), MemberType = typeof(TemplateTokensResultGenerator))]
        public void ApplyTokens_FilePathSubstitutions_Success(string template, IEnumerable<Token> tokens, string expected)
        {
            // Arrange

            // Act
            var actual = template.ApplyTokens(tokens);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}

namespace BardBot.Discord.Tests.Generators
{
    public partial class TemplateTokensResultGenerator
    {
        public static IEnumerable<object[]> StringExtensions_ApplyTokens_FilePathSubstitutions_Success()
        {
            yield return new object[]
            {
                "/ttrpg/campaigns/are-we-excited/sessions/{before,:yyyy-MM-dd-HHmm}__{after,:yyyy-MM-dd-HHmm}/transcripts/{character,=common,pathsafe}.txt",
                new List<Token>
                {
                    Token.Character("maud"),
                    Token.Before(new DateTime(2022, 2, 22, 20, 0, 0)),
                    Token.After(new DateTime(2022, 2, 22, 20, 0, 0)),
                },
                "/ttrpg/campaigns/are-we-excited/sessions/2022-02-22-2000__2022-02-22-2000/transcripts/maud.txt"
            };
        }
    }
}
