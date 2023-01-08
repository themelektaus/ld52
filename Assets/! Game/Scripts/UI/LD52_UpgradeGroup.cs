using System.Collections.Generic;

using UnityEngine;

namespace Prototype
{
    public class LD52_UpgradeGroup : MonoBehaviour
    {
        public string title;
        [SerializeField] GameObject buttonPrefab;

        readonly List<Pending.ButtonUI> buttons = new();

        public Ability ability;

        LD52_Global.Upgrades.Ability _ability;

        void Awake()
        {
            _ability = LD52_Global.instance.GetAbility(ability);

            gameObject.KillChildren();

            buttons.Clear();
            for (int i = 0; i < _ability.maxLevel; i++)
            {
                var button = buttonPrefab.Instantiate(parent: transform).GetComponent<Pending.ButtonUI>();
                button.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = title;
                button.transform.Find("Level").GetComponent<TMPro.TextMeshProUGUI>().text = $"+{i + 1}";
                button.transform.Find("Costs").GetComponent<TMPro.TextMeshProUGUI>().text = $"$ {_ability.GetCosts(i + 1)}";
                button.onLeftClick.AddListener(_ability.Upgrade);
                buttons.Add(button);
            }
        }

        void Update()
        {
            for (int i = 0; i < _ability.maxLevel; i++)
            {
                buttons[i].enabled = i <= _ability.level;
                buttons[i].active = i < _ability.level;
                buttons[i].transform.Find("Costs").gameObject.SetActive(!buttons[i].active && buttons[i].enabled);
            }
        }
    }
}