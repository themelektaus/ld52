using System.Collections.Generic;

using UnityEngine;

namespace Prototype
{
    public class LD52_UpgradeGroup : MonoBehaviour
    {
        public string title;
        public GameObject buttonPrefab;
        public AbilityType abilityType;

        LD52_Global.Ability ability;

        readonly List<Pending.ButtonUI> buttons = new();

        void Awake()
        {
            ability = LD52_Global.instance.GetAbility(abilityType);

            gameObject.KillChildren();

            buttons.Clear();
            for (int i = 0; i < ability.maxLevel; i++)
            {
                var button = buttonPrefab.Instantiate(parent: transform).GetComponent<Pending.ButtonUI>();
                SetTextOf(button, "Text", title);
                SetTextOf(button, "Level", $"+{i + 1}");
                SetTextOf(button, "Costs", $"$ {ability.GetCosts(i + 1)}");
                button.onLeftClick.AddListener(ability.Upgrade);
                buttons.Add(button);
            }
        }

        void Update()
        {
            for (int i = 0; i < ability.maxLevel; i++)
            {
                buttons[i].enabled = i <= ability.level;
                buttons[i].active = i < ability.level;

                Get(buttons[i], "Costs").gameObject.SetActive(!buttons[i].active && buttons[i].enabled);
            }
        }

        static Transform Get(Pending.ButtonUI button, string name)
        {
            return button.transform.Find(name);
        }

        static void SetTextOf(Pending.ButtonUI button, string name, string text)
        {
            Get(button, name).GetComponent<TMPro.TextMeshProUGUI>().text = text;
        }
    }
}