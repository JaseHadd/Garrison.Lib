namespace Garrison.Lib.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> action)
    {
        if (enumerable is null)
            throw new ArgumentNullException(nameof(enumerable));
        if (action is null)
            throw new ArgumentNullException(nameof(action));

        foreach (var item in enumerable)
            action(item);
    }
}
