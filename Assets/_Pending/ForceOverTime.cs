using UnityEngine;

namespace Prototype.Pending
{
    [RequireComponent(typeof(Rigidbody))]
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(ForceOverTime))]
    public class ForceOverTime : MonoBehaviour
    {
        public Vector3 force;

        Rigidbody body;

        void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            body.AddForce(force);
        }
    }
}