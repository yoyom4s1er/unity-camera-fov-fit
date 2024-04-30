using Unity.Mathematics;
using UnityEngine;

namespace Gilzoide.CameraFit
{
    [ExecuteAlways]
    public abstract class ACameraFit : MonoBehaviour
    {
        [SerializeField] protected Camera _targetCamera;
        [SerializeField] protected Vector2 _margins;

        public abstract Bounds? GetWorldBounds();

        private void Update()
        {
            RefreshFov();
        }

        public void RefreshFov()
        {
            if (_targetCamera && GetWorldBounds() is Bounds bounds)
            {

                bounds.Expand(_margins);
                _targetCamera.FitFov(bounds);
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (GetWorldBounds() is Bounds bounds)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        protected virtual void OnValidate()
        {
            RefreshFov();
        }
#endif
    }
}