using UnityEngine;

public class PathManagerBehavior : MonoBehaviour
{
    [SerializeField] private MoveCubeBehavior _moveCubeBehavior;

    private NeighbourCubeBehavior[] _neighbourCubes;

    private NeighbourCubeBehavior _currentTargetNeighbour;

    private void Awake()
    {
        _neighbourCubes = GetComponentsInChildren<NeighbourCubeBehavior>();
    }

    private void OnEnable()
    {
        foreach (var neighbourCube in _neighbourCubes)
        {
            neighbourCube.OnCubePressed += OnCubePressedHandler;
        }
        
        _moveCubeBehavior.OnReturnToStart += OnCubeReturnToStart;
    }

    private void OnDisable()
    {
        foreach (var neighbourCube in _neighbourCubes)
        {
            neighbourCube.OnCubePressed -= OnCubePressedHandler;
        }
        
        _moveCubeBehavior.OnReturnToStart -= OnCubeReturnToStart;
    }

    private void OnCubePressedHandler(NeighbourCubeBehavior targetNeighbourCube)
    {
        if (targetNeighbourCube == _currentTargetNeighbour)
        {
            return;
        }
        
        var pathPoint = GetOrCreateOnPathPoint(targetNeighbourCube);
        _moveCubeBehavior.StartMoveByPath(pathPoint, targetNeighbourCube.transform);

        _currentTargetNeighbour = targetNeighbourCube;
    }

    private Vector3 GetOrCreateOnPathPoint(NeighbourCubeBehavior targetNeighbourCube)
    {
        var closestNeighbour = GetClosestNeighbourOnPathToTarget(targetNeighbourCube);
        var closestNeighbourPosition = closestNeighbour.transform.position;

        if (closestNeighbour == targetNeighbourCube)
        {
            closestNeighbourPosition = CreateAdditionalPointOnPath(targetNeighbourCube.transform.position);
        }

        return closestNeighbourPosition;
    }

    private NeighbourCubeBehavior GetClosestNeighbourOnPathToTarget(NeighbourCubeBehavior targetNeighbourCube)
    {
        var startPosition = _moveCubeBehavior.transform.position;
        var minDistanceNeighbour = targetNeighbourCube;

        var minDistanceToOrigin = Vector3.Distance(startPosition, targetNeighbourCube.transform.position);

        var minWeight = minDistanceToOrigin * 2;

        foreach (var neighbourCube in _neighbourCubes)
        {
            if (neighbourCube == _currentTargetNeighbour)
                continue;

            var currentNeighbourPosition = neighbourCube.transform.position;

            var distanceToOrigin = Vector3.Distance(startPosition, currentNeighbourPosition);
            var distanceToNeighbour =
                Vector3.Distance(targetNeighbourCube.transform.position, currentNeighbourPosition);

            var currentWeight = distanceToOrigin + distanceToNeighbour;

            if (distanceToOrigin < minDistanceToOrigin && currentWeight < minWeight)
            {
                minWeight = currentWeight;
                minDistanceNeighbour = neighbourCube;
            }
        }

        return minDistanceNeighbour;
    }

    private Vector3 CreateAdditionalPointOnPath(Vector3 targetPosition)
    {
        var startPosition = _moveCubeBehavior.transform.position;

        var planePartA = targetPosition - startPosition;
        var planePartB = Vector3.forward;

        var normal = Vector3.Cross(planePartA, planePartB).normalized;

        return startPosition + planePartA / 2 + normal;
    }

    private void OnCubeReturnToStart()
    {
        _currentTargetNeighbour = null;
    }
}