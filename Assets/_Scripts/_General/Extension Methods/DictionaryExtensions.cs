using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static string Stringify<T, K>(this Dictionary<T, K> _dict)
    {
        int _count = _dict.Count;

        var _keysEnumerator = _dict.Keys.GetEnumerator();
        var _valuesEnumerator = _dict.Values.GetEnumerator();

        _keysEnumerator.MoveNext();
        _valuesEnumerator.MoveNext();

        string _string = "{";
        for (int i = 0; i < _count; i++)
        {
            _string += _keysEnumerator.Current.ToString() + "=";
            _string += _valuesEnumerator.Current.ToString();

            if (i != _count - 1)
            {
                _keysEnumerator.MoveNext();
                _valuesEnumerator.MoveNext();
                _string += ", ";
            }
        }

        return _string + "}";
    }
}