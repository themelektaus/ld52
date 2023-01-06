using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Start")]
    public class OnStart : On
    {
        void Start() => Invoke();
    }
}