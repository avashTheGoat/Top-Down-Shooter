using UnityEngine;
using System;

public static class TransformExtensions
{
    public static void DestroyChildren(this Transform _trans)
    {
        int _childCount = _trans.childCount;
        for (int i = 0; i < _childCount; i++)
            UnityEngine.Object.Destroy(_trans.GetChild(i).gameObject);
    }

    public static void DestroyChildren(this Transform _trans, Predicate<GameObject> _shouldDestroyObjectPredicate)
    {
        int _childCount = _trans.childCount;
        for (int i = 0; i < _childCount; i++)
        {
            GameObject _curChildObject = _trans.GetChild(i).gameObject;
            if (_shouldDestroyObjectPredicate.Invoke(_curChildObject))
                UnityEngine.Object.Destroy(_curChildObject);
        }
    }
}