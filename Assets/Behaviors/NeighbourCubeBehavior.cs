using System;
using UnityEngine;

namespace Behaviors
{
    public class NeighbourCubeBehavior : MonoBehaviour
    {
        public event Action<NeighbourCubeBehavior> OnCubePressed;
    
        public void OnMouseDown()
        {
            OnCubePressed?.Invoke(this);
        }
    }
}
