using Cinemachine;
using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE_CAMERA + "/Camera Info")]
    public class CameraInfo : MonoBehaviour
    {
        CinemachineVirtualCameraBase _virtualCameraBase;
        protected CinemachineVirtualCameraBase virtualCameraBase
        {
            get
            {
                if (!_virtualCameraBase)
                    _virtualCameraBase = GetComponent<CinemachineVirtualCameraBase>();
                return _virtualCameraBase;
            }
        }

        [SerializeField] ObjectQuery targetQuery;

        Transform targetQueryFollowTransform;
        Transform targetQueryLookAtTransform;

        void Update()
        {
            if (!targetQuery)
                return;

            var target = targetQuery.FindInterface<Pending.ICameraTarget>();
            if (target is not null)
            {
                if (!targetQueryFollowTransform)
                {
                    var follow = new GameObject("[ FOLLOW ]");
                    targetQueryFollowTransform = follow.transform;
                }

                if (!targetQueryLookAtTransform)
                {
                    var lookAt = new GameObject("[ LOOK AT ]");
                    targetQueryLookAtTransform = lookAt.transform;
                }

                targetQueryFollowTransform.position = target.GetFollowPosition();
                targetQueryLookAtTransform.position = target.GetLookAtPosition();

                virtualCameraBase.Follow = targetQueryFollowTransform;
                virtualCameraBase.LookAt = targetQueryLookAtTransform;

                return;
            }

            if (targetQueryFollowTransform)
                targetQueryFollowTransform.gameObject.Destroy();

            if (targetQueryLookAtTransform)
                targetQueryLookAtTransform.gameObject.Destroy();

            var targetQueryTransform = targetQuery.FindTransform();
            virtualCameraBase.Follow = targetQueryTransform;
            virtualCameraBase.LookAt = targetQueryTransform;
        }

        public void Follow(Transform target)
        {
            if (targetQuery)
                throw new System.Exception("A target query is already set");

            virtualCameraBase.Follow = target;
        }

        public void LookAt(Transform target)
        {
            if (targetQuery)
                throw new System.Exception("A target query is already set");

            virtualCameraBase.LookAt = target;
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            virtualCameraBase.ForceCameraPosition(position, rotation);
        }
    }
}