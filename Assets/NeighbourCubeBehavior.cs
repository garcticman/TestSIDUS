using System;
using UnityEngine;

public class NeighbourCubeBehavior : MonoBehaviour
{
    public event Action<NeighbourCubeBehavior> OnCubePressed;
    
    public void OnMouseDown()
    {
        OnCubePressed?.Invoke(this);
    }
}
