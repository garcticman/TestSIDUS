using System;
using UnityEngine;

namespace Behaviors
{
    public class MoveCubeBehavior : MonoBehaviour
    {
        public event Action OnReturnToStart;
    
        [SerializeField] private float _moveTime = 1;
    
        private bool _isMoving;
        private float _progress;

        private Vector3 _startPosition;
        private Vector3 _startForward;

        private Transform _targetPoint;
        private Vector3 _previousForward;
        private Vector3 _currentRotationNormal;
        private float _currentRotationAngle;

        private AlongArcMoveService _alongArcMoveService;

        public void StartMoveByPath(Vector3 pathPoint, Transform targetPoint)
        {
            _alongArcMoveService.InitPath(pathPoint, targetPoint.position);

            _isMoving = true;

            _targetPoint = targetPoint;

            var movingCubeForward = transform.forward;
            var targetForward = _targetPoint.forward;

            _currentRotationNormal = Vector3.Cross(movingCubeForward, targetForward);
            _currentRotationAngle = Vector3.SignedAngle(movingCubeForward, targetForward, _currentRotationNormal);
        }

        private void Awake()
        {
            _alongArcMoveService = new AlongArcMoveService(transform);

            _startPosition = transform.position;
            _startForward = _previousForward = transform.forward;
        }
    
        private void PathComplete()
        {
            _isMoving = false;
            _progress = 0;
            _previousForward = transform.forward;
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
            transform.forward = _previousForward = _startForward;
        
            OnReturnToStart?.Invoke();
        }

        private void MoveToTarget()
        {
            _alongArcMoveService.Move(_progress);
        }

        private void HandleRotation()
        {
            var angle = _currentRotationAngle * _progress;
            
            transform.forward = Quaternion.AngleAxis(angle, _currentRotationNormal) * _previousForward;
        }
    }
}