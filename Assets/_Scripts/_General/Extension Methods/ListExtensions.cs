using System.Collections.Generic;

public static class ListExtensions
{
    public static string Stringify(this List<string> _list) => $"[{string.Join(", ", _list)}]";

    public static bool ContainsReference<T>(this List<T> _list, T _gameObject)
    {
        foreach (T _obj in _list)
        {
            if (ReferenceEquals(_obj, _gameObject))
                return true;
        }

        return false;
    }
}