using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Trigger Stay")]
    public class OnTriggerStay_ : On<Collider>
    {
        void OnTriggerStay(Collider collider)
        {
            Invoke(collider);
        }
    }
}