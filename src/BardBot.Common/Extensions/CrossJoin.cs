namespace BardBot.Common.Extensions;

public static class CrossJoinExtension
{
    public static IEnumerable<(T1, T2)> CrossJoin<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
    {
        return first.SelectMany(x => second, (x, y) => (x, y));
    }
}
