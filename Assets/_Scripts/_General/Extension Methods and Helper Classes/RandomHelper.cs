using UnityEngine;
using System.Collections.Generic;

public static class RandomHelper
{
    public static List<T> Choices<T>(List<T> _list, int _numChoices, bool _areRepetitionsAllowed)
    {
        List<T> _choices = new();

        while (_choices.Count < _numChoices)
        {
            int _index = Random.Range(0, _list.Count);
            _choices.Add(_list[_index]);

            if (!_areRepetitionsAllowed)
                _list.RemoveAt(_index);
        }

        return _choices;
    }
}