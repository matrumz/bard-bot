namespace BardBot.Common.Extensions;

public static class IfNotNullExtension
{

    public static void IfNotNull<T>(this T? obj, Action<T> action) where T : class
    {
        if (obj is not null)
        {
            action(obj);
        }
    }

    public static void IfNotNull<T>(this T? obj, Action<T> action) where T : struct
    {
        if (obj is not null)
        {
            action(obj.Value);
        }
    }

}
