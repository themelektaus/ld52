using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Trigger Exit")]
    public class OnTriggerExit_ : On<Collider>
    {
        void OnTriggerExit(Collider collider)
        {
            Invoke(collider);
        }
    }
}