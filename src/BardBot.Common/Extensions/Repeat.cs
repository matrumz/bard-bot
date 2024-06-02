namespace BardBot.Common.Extensions;

public static class RepeatExtension
{

    public static string Repeat(this string item, int count) =>
        string.Concat(Enumerable.Repeat(item, count));

}
