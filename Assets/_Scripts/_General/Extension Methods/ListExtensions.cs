using System;
using System.Collections.Generic;

public static class ListExtensions
{
    public static string Stringify(this List<string> _list) => $"[{string.Join(", ", _list)}]";

    public static string Stringify<T>(this List<T> _list, Func<T, string> _toStringFunc)
    {
        int _size = _list.Count;

        string _string = "";
        for (int i = 0; i < _size; i++)
        {
            _string += _toStringFunc(_list[i]);

            if (i != _size - 1)
                _string += ", ";
        }

        return _string;
    }

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