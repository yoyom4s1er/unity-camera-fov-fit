using UnityEngine;

namespace Gilzoide.CameraFit
{
    [ExecuteAlways]
    public abstract class AFovFitter : MonoBehaviour
    {
        [Tooltip("Target Camera that will have the FOV adjusted. If null, nothing will happen.")]
        [SerializeField] public Camera _targetCamera;

        [Tooltip("If true, the target camera's FOV will be fit in this object's Start message.")]
        [SerializeField] public bool _applyOnStart = true;

        [Tooltip("If true, the target camera's FOV will be fit in this object's Update message.")]
        [SerializeField] public bool _applyOnUpdate = false;

        [Tooltip("Margins to add to the bounds when fitting the FOV, in world units.")]
        [SerializeField] public Vector3 _margins;

        protected abstract Bounds? GetWorldBounds();

        protected void Start()
        {
            if (_applyOnStart)
            {
                RefreshFov();
            }
        }

        protected void Update()
        {
            if (!_applyOnUpdate && Application.isPlaying)
            {
                // disable refresh on Play mode to avoid no-op Updates
                enabled = false;
                return;
            }

            RefreshFov();
        }

        public void RefreshFov()
        {
            if (_targetCamera && GetTargetBounds() is Bounds bounds)
            {
                _targetCamera.FitFovToBounds(bounds);
            }
        }

        public Bounds? GetTargetBounds()
        {
            if (GetWorldBounds() is Bounds bounds)
            {
                bounds.Expand(_margins);
                return bounds;
            }
            else
            {
                return null;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (GetTargetBounds() is Bounds bounds)
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
