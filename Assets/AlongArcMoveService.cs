using UnityEngine;

public class AlongArcMoveService
{
    private readonly Transform _movingObject;

    private Vector3 _startPoint;
    private Vector3 _pathPoint;
    private Vector3 _targetPoint;

    private Vector3 _circleCenter;
    
    private Vector3 _vectorA;
    private Vector3 _vectorB;
    
    private Vector3 _normalVector;
    
    private float _abFullAngle;

    public AlongArcMoveService(Transform movingObject)
    {
        _movingObject = movingObject;
    }

    public void InitPath(Vector3 pathPoint, Vector3 targetPoint)
    {
        _targetPoint = targetPoint;
        _pathPoint = pathPoint;

        _startPoint = _movingObject.position;

        _circleCenter = GeometryUtils.GetCircleCenter(_startPoint, _pathPoint, _targetPoint);
        
        _vectorA = _startPoint - _circleCenter;
        _vectorB = _targetPoint - _circleCenter;
        
        _normalVector = GetNormalVector();
        _abFullAngle = GetFullAngle();
    }

    private Vector3 GetNormalVector()
    {
        var planePartA = _pathPoint - _startPoint;
        var planePartB = _pathPoint - _targetPoint;

        return Vector3.Cross(planePartB, planePartA);
    }

    private float GetFullAngle()
    {
        return Vector3.SignedAngle(_vectorA, _vectorB, _normalVector);
    }

    public void Move(float process)
    {
        _movingObject.position = GetCurrentPosition(process);
    }

    private Vector3 GetCurrentPosition(float progress)
    {
        var currentAngle = _abFullAngle * progress;

        var processVector = RotateProcessVector(currentAngle, _normalVector);
        var currentPosition = _circleCenter + processVector;

        return currentPosition;
    }

    private Vector3 RotateProcessVector(float rotateAngle, Vector3 axis)
    {
        var processVector = Quaternion.AngleAxis(rotateAngle, axis) * _vectorA;
        return processVector;
    }
}