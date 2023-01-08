using UnityEngine;

namespace Prototype
{
    public class LD52_Stats : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI altarValue;
        [SerializeField] TMPro.TextMeshProUGUI minAltarValue;

        [SerializeField] GameObject moneyWon;
        [SerializeField] TMPro.TextMeshProUGUI moneyWonText;

        [SerializeField] GameObject deadButNotHarvested;
        [SerializeField] TMPro.TextMeshProUGUI deadButNotHarvestedText;
        
        [SerializeField] GameObject earnedMoney;
        [SerializeField] TMPro.TextMeshProUGUI earnedMoneyText;

        [SerializeField] GameObject lostMoney;
        [SerializeField] TMPro.TextMeshProUGUI lostMoneyText;

        [SerializeField] GameObject nextButton;
        [SerializeField] GameObject gameOverButton;

        void Update()
        {
            var global = LD52_Global.instance;

            var a = global.altarValue;
            var b = global.wave.minAltarValue;
            var ok = a >= b;

            altarValue.text = a.ToString();
            minAltarValue.text = b.ToString();

            a = Mathf.Max(0, global.altarValue - global.wave.minAltarValue);
            b = global.deadEnemiesValue;

            Debug.Log(global.altarValue);
            Debug.Log(global.wave.minAltarValue);
            Debug.Log(b);

            var c = a - b;

            moneyWon.SetActive(a > 0);
            moneyWonText.text = a.ToString();

            deadButNotHarvested.SetActive(b > 0);
            deadButNotHarvestedText.text = b.ToString();

            earnedMoney.SetActive(c > 0);
            earnedMoneyText.text = c.ToString();

            var d = Mathf.Min(LD52_Global.instance.money, -c);
            lostMoney.SetActive(d > 0);
            lostMoneyText.text = d.ToString();

            if (ok)
                nextButton.SetActive(true);
            else
                gameOverButton.SetActive(true);
        }
    }
}