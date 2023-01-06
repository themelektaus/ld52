using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TMPro.TextMeshPro))]
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(FloatDisplay))]
    public class FloatDisplay : MonoBehaviour
	{
        [SerializeField] FloatFormat format;
        [SerializeField] Reference value;

		TMPro.TextMeshPro tmp;

        void Awake()
        {
            tmp = GetComponent<TMPro.TextMeshPro>();
        }

        void Update()
        {
            tmp.text = value.Get<float>().ToFormattedString(format);
        }
    }
}