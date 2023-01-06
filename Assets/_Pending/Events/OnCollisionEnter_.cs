using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Collision Enter")]
    public class OnCollisionEnter_ : On<Collision>
    {
        void OnCollisionEnter(Collision collision)
        {
            Invoke(collision);
        }
    }
}