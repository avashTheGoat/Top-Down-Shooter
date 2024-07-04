using UnityEngine;

public static class BoundsExtensions
{
    public static Vector2 GetRandPointInBounds(this Bounds bounds)
    {
        float _minX = bounds.min.x;
        float _maxX = bounds.max.x;
        float _minY = bounds.min.y;
        float _maxY = bounds.max.y;

        Vector2 _rand = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));
        return bounds.ClosestPoint(_rand);
    }

    public static Vector2 GetRandPointInBounds(this Bounds bounds, Vector2 _bottomLeft, Vector2 _topRight)
    {
        float _minX = _bottomLeft.x;
        float _maxX = _topRight.x;
        float _minY = _bottomLeft.y;
        float _maxY = _topRight.y;

        Vector2 _rand = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));

        /*Debug.Log("Rand: " + _rand);
        Debug.Log("Closest point: " + bounds.ClosestPoint(_rand));*/

        return bounds.ClosestPoint(_rand);
    }

    public static void RotateAroundX(this Bounds bounds, float _rotationAngle)
    {
        bounds.extents = new Vector3(bounds.extents.x,
            bounds.extents.y * Mathf.Cos(_rotationAngle) - bounds.extents.z * Mathf.Sin(_rotationAngle),
            bounds.extents.y * Mathf.Sin(_rotationAngle) + bounds.extents.z * Mathf.Cos(_rotationAngle));
        
        bounds.center = new Vector3(bounds.center.x,
            bounds.center.y * Mathf.Cos(_rotationAngle) - bounds.center.z * Mathf.Sin(_rotationAngle),
            bounds.center.y * Mathf.Sin(_rotationAngle) + bounds.center.z * Mathf.Cos(_rotationAngle));
    }
}