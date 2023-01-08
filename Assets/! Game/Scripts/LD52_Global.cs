using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu]
    public class LD52_Global : ScriptableObject
    {
        static LD52_Global[] _instances;
        static LD52_Global _instance;

        public static LD52_Global instance
        {
            get
            {
                if (_instances is null)
                {
                    _instances = Resources.LoadAll<LD52_Global>("");
                    if (_instances.Length > 0)
                        _instance = _instances[0];
                }
                return _instance;
            }
        }

        [Serializable]
        public class CharacterSettings
        {
            public Vector2 speed = new(10, 10);
            public float maxHealth = 1;
            public float maxHarvestLife = 1;
            public float maxBuried = 1;
            public int itemCapacity = 10;
        }
        public CharacterSettings playerSettings;

        public ObjectQuery playerQuery;
        public LD52_Player GetPlayer() => playerQuery.FindComponent<LD52_Player>();

        public int playerItemCount
        {
            get
            {
                var player = GetPlayer();
                return player ? player.items.Count : 0;
            }
        }

        [Serializable]
        public class Wave
        {
            public int index;
            public int maxIndex = 10;
            public float time;
            public InterpolationCurve.InterpolationCurve budgetCurve;
            public InterpolationCurve.InterpolationCurve durationCurve;
            public InterpolationCurve.InterpolationCurve minAltarValueCurve;

            public int budget => (int) budgetCurve.Evaluate(index / (float) maxIndex);
            public float duration => durationCurve.Evaluate(index / (float) maxIndex);
            public float minAltarValue => minAltarValueCurve.Evaluate(index / (float) maxIndex);
        }
        public Wave wave;
        
        public List<LD52_EnemyItem> altarItems;

        protected override void Initialize()
        {
            playerSettings.speed = new(6, 6);
            playerSettings.itemCapacity = 3;
            wave.index = 0;
            wave.time = 0;
            altarItems.Clear();
        }

        public void GameStateMachineTrigger(string name)
        {
            LD52_GameStateMachine.instance.Trigger(name);
        }

        public void PlaySound(SoundEffectCollection soundEffect)
        {
            soundEffect.PlayRandomClip();
        }

        public static Vector2 GetInputAxis()
        {
            return new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        public static bool GetInputShoot()
        {
            return Input.GetMouseButton(0);
        }

        public static bool GetInputHarvest()
        {
            return Input.GetMouseButton(1);
        }

        public static RaycastHit? GetMouseGroundHit()
        {
            var ray = Utils.mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hitInfo, 1000f, LayerMask.GetMask("Ground")) ? hitInfo : null;
        }
    }
}