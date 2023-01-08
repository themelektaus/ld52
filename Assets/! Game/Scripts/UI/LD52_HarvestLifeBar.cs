using UnityEngine;

namespace Prototype
{
    public class LD52_HarvestLifeBar : MonoBehaviour
    {
        [SerializeField] LD52_Character character;
        [SerializeField] GameObject harvestLifeBar;
        [SerializeField] RectTransform harvestLifeValue;

        float maxHarvestLife;

        void Start()
        {
            maxHarvestLife = character.settings.maxHarvestLife;
        }

        void Update()
        {
            var x = Mathf.Clamp(character.harvestLife / maxHarvestLife, 0.075f, 1);
            var anchor = harvestLifeValue.anchorMax;
            anchor.x = x;
            harvestLifeValue.anchorMax = anchor;

            harvestLifeBar.SetActive(character.health == 0);
        }
    }
}