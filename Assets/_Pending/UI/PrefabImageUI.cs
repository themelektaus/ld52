using UnityEngine;
using UnityEngine.UI;

namespace Prototype.Pending
{
    [RequireComponent(typeof(RawImage))]
    public class PrefabImageUI : MonoBehaviour
    {
        static int index;

        [ReadOnly(duringEditMode = false)]
        public GameObject prefab;

        void Start()
        {
            if (!prefab)
                return;

            if (!TryGetComponent<RawImage>(out var image))
                return;

            var a = image.color.a;

            image.color = Color.clear;

            var i = (index++ - 50) * 20;
            var t = Utils.mainCamera.transform;
            var position = t.position - t.forward * 100 + t.right * i;

            var toRenderTexture = prefab.GetComponentInChildren<ToRenderTexture>();

            var gameObject = prefab.Instantiate(position);

            var outOfBoundsTrigger = gameObject.GetComponentInChildren<OutOfBoundsTrigger>();
            if (outOfBoundsTrigger)
                outOfBoundsTrigger.enabled = false;

            toRenderTexture = gameObject.GetComponentInChildren<ToRenderTexture>();
            toRenderTexture.onRender += renderTexture =>
            {
                image.texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
                Graphics.CopyTexture(renderTexture, image.texture);
                image.color = new(1, 1, 1, a);
                Destroy(gameObject);
            };

            toRenderTexture.enabled = true;
        }
    }
}