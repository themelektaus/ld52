using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Post Render")]
    public class OnPostRender_ : On
    {
        void OnPostRender()
        {
            Invoke();
        }
    }
}