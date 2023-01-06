using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Awake")]
    public class OnAwake : On
    {
        void Awake()
        {
            Invoke();
        }
    }
}