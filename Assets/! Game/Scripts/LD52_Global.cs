using System;
using System.Collections.Generic;
using System.Linq;

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
            public Vector2 speed = new(6, 6);
            public float maxHealth = 1;
            public float maxHarvestLife = 1;
            public float maxBuried = 1;
        }

        public ObjectQuery playerQuery;
        public LD52_Player GetPlayer() => playerQuery.FindComponent<LD52_Player>();

        public ObjectQuery enemyQuery;
        public LD52_Enemy[] GetEnemies() => enemyQuery.FindComponents<LD52_Enemy>();

        public int playerItemCount
        {
            get
            {
                var player = GetPlayer();
                return player ? player.items.Count : 0;
            }
        }

        public GameOverState gameOverState;

        public int money;

        [Serializable]
        public class Ability
        {
            public AbilityType abilityType;
            public int level;
            public int maxLevel = 3;
            public InterpolationCurve.InterpolationCurve curve;
            public InterpolationCurve.InterpolationCurve costsCurve;

            public void Reset()
            {
                level = 0;
            }

            public void Upgrade()
            {
                var nextLevel = Mathf.Min(level + 1, maxLevel);
                var costs = GetCosts(nextLevel);
                if (instance.money >= costs)
                {
                    instance.money -= costs;
                    level = nextLevel;
                }
            }

            public float GetValue()
            {
                return curve.Evaluate(level / (float) maxLevel);
            }

            public int GetCosts(int level)
            {
                return Mathf.RoundToInt(costsCurve.Evaluate((level - 1) / (float) maxLevel));
            }
        }
        public List<Ability> abilities;

        public Ability GetAbility(AbilityType abilityType)
        {
            return abilities.FirstOrDefault(x => x.abilityType == abilityType);
        }

        public void ResetAbilities()
        {
            foreach (var ability in abilities)
                ability.Reset();
        }

        public bool IsFullyUpgraded()
        {
            foreach (var ability in abilities)
                if (ability.level < ability.maxLevel)
                    return false;
            return true;
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
            public int minAltarValue => Mathf.RoundToInt(minAltarValueCurve.Evaluate(index / (float) maxIndex));

            public void Reset()
            {
                index = 0;
                ResetTime();
            }

            public void ResetTime()
            {
                time = 0;
            }

            public void UpdateTime()
            {
                time = Mathf.Min(time + Time.deltaTime, duration);
            }

            public void Next()
            {
                index = Mathf.Min(maxIndex, index + 1);
            }
        }
        public Wave wave;

        public List<LD52_EnemyItem> altarItems;
        public int altarValue => altarItems.Sum(x => x.value);

        public int deadEnemiesValue;

        public SoundEffectCollection music;

        [Serializable]
        public class Sounds
        {
            public SoundEffectCollection digOut;
            public SoundEffectCollection harvest;
            public SoundEffectCollection hit;
            public SoundEffectCollection die;
            public SoundEffectCollection bullet;
            public SoundEffectCollection altar;
        }
        public Sounds sounds;

        protected override void Initialize()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            gameOverState = GameOverState.Canceled;

            money = 0;

            ResetAbilities();
            wave.Reset();
            
            altarItems.Clear();
        }

        public void GameStateMachineTrigger(string name)
        {
            LD52_GameStateMachine.instance.Trigger(name);
        }

        public static Vector2 GetInputAxis()
        {
            return new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        public static bool GetInputShoot()
        {
            return Utils.IsMouseButton_ButNotUI(0);
        }

        public static bool GetInputHarvest()
        {
            return Utils.IsMouseButton_ButNotUI(1);
        }

        public static RaycastHit? GetMouseGroundHit()
        {
            var ray = Utils.mainCamera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hitInfo, 1000f, LayerMask.GetMask("Ground")) ? hitInfo : null;
        }
    }
}