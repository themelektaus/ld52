using UnityEngine;

namespace Prototype
{
    public class LD52_ResolutionSelectorItem : Pending.MouseBehaviourUI
    {
        public LD52_ResolutionSelector owner;
        public TMPro.TextMeshProUGUI text;

        public Color textColor;
        public Color selectedTextColor;

        public event System.Action onClick;

        public bool selected { get; private set; }

        protected override void OnDown(int button)
        {
            if (button != 0)
                return;

            Select();
            onClick?.Invoke();
        }

        public void Select()
        {
            foreach (var item in owner.items)
                item.selected = false;

            selected = true;
        }

        void Update()
        {
            text.color = selected ? selectedTextColor : textColor;
        }
    }
}