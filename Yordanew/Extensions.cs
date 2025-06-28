namespace YordanApi;

public static class Extensions {
    public static IEnumerable<T> InsertAfter<T>(this IEnumerable<T> source, int index, T item)
    {
        var i = 0;
        foreach (var element in source)
        {
            yield return element;
            if (i == index)
                yield return item;
            i++;
        }
    }
}