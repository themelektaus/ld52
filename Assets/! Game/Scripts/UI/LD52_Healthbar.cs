using UnityEngine;

namespace Prototype
{
    public class LD52_Healthbar : MonoBehaviour
    {
        [SerializeField] LD52_Character character;
        [SerializeField] GameObject healthbar;
        [SerializeField] RectTransform healthbarValue;

        float maxHealth;

        void Start()
        {
            maxHealth = character.settings.maxHealth;
        }

        void Update()
        {
            var x = Mathf.Clamp01(character.health / maxHealth);
            var anchor = healthbarValue.anchorMax;
            anchor.x = x;
            healthbarValue.anchorMax = anchor;

            healthbar.SetActive(x > 0 && x < 1);
        }
    }
}