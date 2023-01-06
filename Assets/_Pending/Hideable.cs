using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Hideable))]
    public class Hideable : MonoBehaviour
    {
        [SerializeField] Renderer[] renderers;
        [SerializeField] Behaviour[] behaviours;

        public void Hide()
        {
            foreach (var renderer in renderers)
                renderer.enabled = false;

            foreach (var behaviour in behaviours)
                behaviour.enabled = false;
        }

        public bool Show()
        {
            var result = false;

            foreach (var renderer in renderers)
            {
                if (renderer.enabled)
                    continue;

                renderer.enabled = true;
                result = true;
            }

            foreach (var behaviour in behaviours)
            {
                if (behaviour.enabled)
                    continue;

                behaviour.enabled = true;
                result = true;
            }

            return result;
        }
    }
}