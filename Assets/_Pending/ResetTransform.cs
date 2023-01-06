using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(ResetTransform))]
    public class ResetTransform : MonoBehaviour
    {
        [SerializeField] bool rememberOnAwake = true;

        [SerializeField] bool position = true;
        [SerializeField] bool rotation = true;

        [SerializeField] bool localPosition = false;
        [SerializeField] bool localRotation = false;

        [SerializeField] bool localScale = true;

        [SerializeField] bool velocity = false;

        TransformInfo transformInfo;

        void Awake()
        {
            if (rememberOnAwake)
                Remember();
        }

        public void Remember()
        {
            transformInfo = (TransformInfo) transform;
        }

        void OnEnable()
        {
            if (position)
                transform.position = transformInfo.position;

            if (rotation)
                transform.rotation = transformInfo.rotation;

            if (localPosition)
                transform.localPosition = transformInfo.localPosition;

            if (localRotation)
                transform.localRotation = transformInfo.localRotation;

            if (localScale)
                transform.localScale = transformInfo.localScale;

            if (velocity && TryGetComponent<Rigidbody>(out var body))
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }

            enabled = false;
        }
    }
}