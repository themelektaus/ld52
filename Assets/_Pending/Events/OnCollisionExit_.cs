using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Collision Exit")]
    public class OnCollisionExit_ : On<Collision>
    {
        void OnCollisionExit(Collision collision)
        {
            Invoke(collision);
        }
    }
}