using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Collision Stay")]
    public class OnCollisionStay_ : On<Collision>
    {
        void OnCollisionStay(Collision collision)
        {
            Invoke(collision);
        }
    }
}