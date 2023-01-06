using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
    [ExecuteAlways]
    [RequireComponent(typeof(AspectRatioFitter))]
    public class LD52_AspectRatioLimiter : MonoBehaviour
    {
        [SerializeField] Vector2 minAspectRatio = new(1, 1);
        [SerializeField] Vector2 maxAspectRatio = new(16, 9);

        AspectRatioFitter fitter;

        void Awake()
        {
            fitter = GetComponent<AspectRatioFitter>();
        }

        void Update()
        {
            fitter.aspectRatio = Mathf.Clamp(
                Screen.width / (float) Screen.height,
                minAspectRatio.x / minAspectRatio.y,
                maxAspectRatio.x / maxAspectRatio.y
            );
        }
    }
}