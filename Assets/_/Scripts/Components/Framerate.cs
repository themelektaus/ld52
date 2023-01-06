using System;
using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Framerate))]
    public class Framerate : MonoBehaviour
    {
        public int targetFrameRate = 60;
        public bool vSync;

        int hash;
        
        void Awake()
        {
            if (!PlayerPrefs.HasKey("vSync"))
                PlayerPrefs.SetInt("vSync", 1);

            ResetHash();

            if (!enabled)
                OnDisable();
        }

        void ResetHash()
        {
            hash = HashCode.Combine(-1, -1);
        }

        void OnEnable()
        {
            ResetHash();
            vSync = PlayerPrefs.GetInt("vSync") != 0;
            Update();
        }

        void OnDisable()
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
            ResetHash();
        }

        void Update()
        {
            var hash = HashCode.Combine(targetFrameRate, vSync);
            if (this.hash == hash)
                return;

            this.hash = hash;

            PlayerPrefs.SetInt("vSync", vSync ? 1 : 0);
            PlayerPrefs.Save();

            Application.targetFrameRate = vSync ? -1 : targetFrameRate;
            QualitySettings.vSyncCount = vSync ? 1 : 0;
        }
    }
}