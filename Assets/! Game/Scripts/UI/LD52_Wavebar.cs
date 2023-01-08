using UnityEngine;

namespace Prototype
{
    public class LD52_Wavebar : MonoBehaviour
    {
        [SerializeField] RectTransform value;

        void Update()
        {
            var anchor = value.anchorMax;
            anchor.x = Mathf.Clamp01(LD52_Global.instance.wave.time / LD52_Global.instance.wave.duration);
            value.anchorMax = anchor;
        }
    }
}