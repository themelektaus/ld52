using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Disable")]
    public class OnDisable_ : On
    {
        protected override bool offloadCoroutine => true;

        void OnDisable()
        {
            Invoke();
        }
    }
}