using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Trigger Enter")]
    public class OnTriggerEnter_ : On<Collider>
    {
        void OnTriggerEnter(Collider collider)
        {
            Invoke(collider);
        }
    }
}