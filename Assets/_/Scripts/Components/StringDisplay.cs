using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(TMPro.TMP_Text))]
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(StringDisplay))]
    public class StringDisplay : MonoBehaviour
	{
        [SerializeField] Reference value;

		TMPro.TMP_Text tmp;

        void Awake()
        {
            tmp = GetComponent<TMPro.TMP_Text>();
        }

        void Update()
        {
            tmp.text = value.Get<string>();
        }
    }
}