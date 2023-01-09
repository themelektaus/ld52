using UnityEngine;

namespace Prototype
{
    public class LD52_CarryingCapacityDisplay : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text1;
        [SerializeField] TMPro.TextMeshProUGUI text2;
        [SerializeField] TMPro.TextMeshProUGUI text3;

        void Update()
        {
            var player = LD52_Global.instance.GetPlayer();
            if (!player)
                return;

            var a = player.items.Count;
            var b = LD52_Global.instance.GetAbility(AbilityType.CarryingCapacity).GetValue();

            text1.text = a.ToString();
            text3.text = b.ToString();

            var color = a < b ? Color.white : Color.red;
            text1.color = color;
            text2.color = color;
            text3.color = color;
        }
    }
}