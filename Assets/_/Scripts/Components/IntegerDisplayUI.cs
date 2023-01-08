using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TMPro.TextMeshProUGUI))]
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(IntegerDisplayUI))]
    public class IntegerDisplayUI : MonoBehaviour
	{
        [SerializeField] Reference value;

		TMPro.TextMeshProUGUI tmp;

        void Awake()
        {
            tmp = GetComponent<TMPro.TextMeshProUGUI>();
        }

        void Update()
        {
            tmp.text = value.Get<int>().ToString();
        }
    }
}