using UnityEngine;

namespace Prototype
{
	public class LD52_GameOver : MonoBehaviour
	{
        public GameObject canceledText;
        public GameObject failedText;
        public GameObject victoryText;

        void Awake()
        {
            canceledText.SetActive(LD52_Global.instance.gameOverState == GameOverState.Canceled);
            failedText.SetActive(LD52_Global.instance.gameOverState == GameOverState.Failed);
            victoryText.SetActive(LD52_Global.instance.gameOverState == GameOverState.Victory);
        }
    }
}