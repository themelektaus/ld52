using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(FollowTransform))]
    public class FollowTransform : MonoBehaviour
    {
        public enum UpdateMode
        {
            None,
            Update,
            FixedUpdate,
            LateUpdate
        }

        [SerializeField] Transform target;

        [Header("Position")]
        [SerializeField] UpdateMode positionUpdateMode;
        [SerializeField, ReadOnly(duringEditMode = false)] float positionSmoothTime;
        [SerializeField] Vector3 positionOffset;

        [Header("Rotation")]
        [SerializeField] UpdateMode rotationUpdateMode;
        [SerializeField, ReadOnly(duringEditMode = false)] float rotationSmoothTime;

        public SmoothTransformPosition transformPosition { get; private set; }
        public SmoothTransformRotation transformRotation { get; private set; }

        void Awake()
        {
            transformPosition = new(transform, positionSmoothTime);
            transformRotation = new(transform, rotationSmoothTime);
        }

        void Update()
        {
            if (!target)
                return;

            if (positionUpdateMode == UpdateMode.Update)
                UpdatePosition();

            if (rotationUpdateMode == UpdateMode.Update)
                UpdateRotation();
        }

        void FixedUpdate()
        {
            if (!target)
                return;

            if (positionUpdateMode == UpdateMode.FixedUpdate)
                UpdatePosition();

            if (rotationUpdateMode == UpdateMode.FixedUpdate)
                UpdateRotation();
        }

        void LateUpdate()
        {
            if (!target)
                return;

            if (positionUpdateMode == UpdateMode.LateUpdate)
                UpdatePosition();

            if (rotationUpdateMode == UpdateMode.LateUpdate)
                UpdateRotation();
        }

        public void UpdatePosition()
        {
            if (!transformPosition)
                return;

            transformPosition.target = GetTargetPosition();
            transformPosition.Update();
        }

        public void UpdateRotation()
        {
            if (!transformRotation)
                return;

            transformRotation.target = GetTargetRotation();
            transformRotation.Update();
        }

        Vector3 GetTargetPosition()
        {
            return target.position + positionOffset;
        }

        Quaternion GetTargetRotation()
        {
            return target.rotation;
        }

        public void UpdateTarget(Transform target)
        {
            if (this.target == target)
                return;

            this.target = target;

            if (transformPosition)
                transformPosition.value = GetTargetPosition();

            if (transformRotation)
                transformRotation.value = GetTargetRotation();
        }
    }
}