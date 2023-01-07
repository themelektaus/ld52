using System;
using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu]
    public class LD52_Global : UnityEngine.ScriptableObject
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

        [Serializable]
        public class CharacterSettings
        {
            public float speed = 1;
            public float maxHealth = 1;
            public float maxHarvestLife = 1;
            public int itemCapacity = 10;
        }
        public CharacterSettings playerSettings;
    }
}