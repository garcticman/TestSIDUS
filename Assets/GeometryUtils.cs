using UnityEngine;

public static class GeometryUtils
{
    public static Vector3 GetCircleCenter(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        var v1 = pointB - pointA;
        var v2 = pointC - pointA;

        var v1v1 = Vector3.Dot(v1, v1);
        var v2v2 = Vector3.Dot(v2, v2);
        var v1v2 = Vector3.Dot(v1, v2);

        var b = 0.5f / (v1v1 * v2v2 - v1v2 * v1v2);
        var k1 = b * v2v2 * (v1v1 - v1v2);
        var k2 = b * v1v1 * (v2v2 - v1v2);

        var center = pointA + v1 * k1 + v2 * k2;

        return center;
    }
}