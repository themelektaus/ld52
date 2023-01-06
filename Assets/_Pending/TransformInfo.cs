using UnityEngine;

namespace Prototype.Pending
{
    public struct TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 lossyScale;

        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;

        public static explicit operator TransformInfo(Transform transform)
        {
            return new()
            {
                position = transform.position,
                rotation = transform.rotation,
                lossyScale = transform.lossyScale,

                localPosition = transform.localPosition,
                localRotation = transform.localRotation,
                localScale = transform.localScale
            };
        }

        public static TransformInfo Lerp(TransformInfo a, TransformInfo b, float t)
        {
            return new()
            {
                position = Vector3.Lerp(a.position, b.position, t),
                rotation = Quaternion.Lerp(Quaternion.identity, a.rotation, t),
                lossyScale = Vector3.Lerp(a.lossyScale, b.lossyScale, t),

                localPosition = Vector3.Lerp(a.localPosition, b.localPosition, t),
                localRotation = Quaternion.Lerp(Quaternion.identity, a.localRotation, t),
                localScale = Vector3.Lerp(a.localScale, b.localScale, t),
            };
        }

    }
}