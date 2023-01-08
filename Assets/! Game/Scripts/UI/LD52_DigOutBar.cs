using UnityEngine;

namespace Prototype
{
    public class LD52_DigOutBar : MonoBehaviour
    {
        [SerializeField] LD52_Character character;
        [SerializeField] GameObject digOutBar;
        [SerializeField] RectTransform digOutValue;

        float maxBuried;

        void Start()
        {
            maxBuried = character.settings.maxBuried;
        }

        void Update()
        {
            var x = 1 - Mathf.Clamp01(character.buried / maxBuried);
            var anchor = digOutValue.anchorMax;
            anchor.x = x;
            digOutValue.anchorMax = anchor;

            digOutBar.SetActive(x < 1);
        }
    }
}