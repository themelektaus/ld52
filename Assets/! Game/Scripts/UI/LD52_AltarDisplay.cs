using UnityEngine;

namespace Prototype
{
    public class LD52_AltarDisplay : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text1;
        [SerializeField] TMPro.TextMeshProUGUI text2;
        [SerializeField] TMPro.TextMeshProUGUI text3;

        void Update()
        {
            var a = LD52_Global.instance.altarValue;
            var b = LD52_Global.instance.wave.minAltarValue;

            text1.text = a.ToString();
            text3.text = b.ToString();

            var color = a >= b ? Color.green : Color.yellow;
            text1.color = color;
            text2.color = color;
            text3.color = color;
        }
    }
}