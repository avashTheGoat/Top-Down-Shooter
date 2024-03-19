using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyChildren(this Transform _trans)
    {
        int _childCount = _trans.childCount;
        for (int i = 0; i < _childCount; i++)
            Object.Destroy(_trans.GetChild(i).gameObject);
    }
}