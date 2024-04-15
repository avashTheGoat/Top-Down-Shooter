using System.Collections.Generic;

public static class ListExtensions
{
    public static string Stringify(this List<string> _list) => $"[{string.Join(", ", _list)}]";
}