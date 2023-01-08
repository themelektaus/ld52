using UnityEngine;

namespace Prototype
{
    public class LD52_MoneyDisplay : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text;

        void Update()
        {
            text.text = LD52_Global.instance.money.ToString();
        }
    }
}