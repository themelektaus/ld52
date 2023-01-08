using UnityEngine;

namespace Prototype
{
	public class LD52_GameOver : MonoBehaviour
	{
        public GameObject cancelText;
        public GameObject loseText;
        public GameObject winText;

        void Awake()
        {
            cancelText.SetActive(LD52_Global.instance.gameOverState == 0);
            loseText.SetActive(LD52_Global.instance.gameOverState == 1);
            winText.SetActive(LD52_Global.instance.gameOverState == 2);
        }
    }
}