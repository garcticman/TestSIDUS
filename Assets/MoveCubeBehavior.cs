using System;
using UnityEngine;

public class MoveCubeBehavior : MonoBehaviour
{
    public event Action OnReturnToStart;
    
    [SerializeField] private float _moveTime = 1;
    
    private bool _isMoving;
    private float _progress;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Transform _targetPoint;
    private Quaternion _previousRotation;

    private AlongArcMoveService _alongArcMoveService;

    public void StartMoveByPath(Vector3 pathPoint, Transform targetPoint)
    {
        _alongArcMoveService.InitPath(pathPoint, targetPoint.position);

        _isMoving = true;

        _targetPoint = targetPoint;
    }

    private void Awake()
    {
        _alongArcMoveService = new AlongArcMoveService(transform);

        _startPosition = transform.position;
        _startRotation = _previousRotation = transform.rotation;
    }
    
    private void PathComplete()
    {
        _isMoving = false;
        _progress = 0;
        _previousRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReturnToStart();
        }
        
        if (!_isMoving)
        {
            return;
        }
        
        _progress += Time.deltaTime / _moveTime;

        MoveToTarget();
        HandleRotation();
        
        if (_progress > 1)
        {
            PathComplete();
        }
    }

    private void ReturnToStart()
    {
        _isMoving = false;
        _progress = 0;
        
        transform.position = _startPosition;
        transform.rotation = _previousRotation = _startRotation;
        
        OnReturnToStart?.Invoke();
    }

    private void MoveToTarget()
    {
        _alongArcMoveService.Move(_progress);
    }

    private void HandleRotation()
    {
        transform.rotation = Quaternion.Lerp(_previousRotation, _targetPoint.rotation, _progress);
    }
}