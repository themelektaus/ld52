using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Enable")]
    public class OnEnable_ : On
    {
        void OnEnable()
        {
            Invoke();
        }
    }
}