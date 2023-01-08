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

        public int money;

        [Serializable]
        public class Upgrades
        {
            [Serializable]
            public class Ability
            {
                public int level;
                public int maxLevel = 3;
                public InterpolationCurve.InterpolationCurve curve;
                public InterpolationCurve.InterpolationCurve costsCurve;

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

                public float GetCurrent()
                {
                    return curve.Evaluate(level / (float) maxLevel);
                }

                public int GetCosts(int level)
                {
                    return Mathf.RoundToInt(costsCurve.Evaluate((level - 1) / (float) maxLevel));
                }
            }

            public Ability moveSpeed;
            public Ability harvestRadius;
            public Ability harvestStrength;
            public Ability shootSpeed;
            public Ability shootDamage;
            public Ability carryingCapacity;
        }

        public Upgrades upgrades;

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
        }
        public Wave wave;

        public List<LD52_EnemyItem> altarItems;
        public int altarValue => altarItems.Sum(x => x.value);
        public int deadEnemiesValue { get; private set; }
        public void UpdateDeadEnemiesValue()
        {
            enemyQuery.ClearCache();
            deadEnemiesValue = GetEnemies().Where(x => x && !x.character.enabled).Count();
        }

        [SerializeField] SoundEffectCollection digOutSound;
        [SerializeField] SoundEffectCollection harvestSound;
        [SerializeField] SoundEffectCollection hitSound;
        [SerializeField] SoundEffectCollection deathSound;
        [SerializeField] SoundEffectCollection bulletSound;

        public int gameOverState;

        protected override void Initialize()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            gameOverState = 0;

            money = 0;

            upgrades.moveSpeed.level = 0;
            upgrades.harvestRadius.level = 0;
            upgrades.harvestStrength.level = 0;
            upgrades.shootSpeed.level = 0;
            upgrades.shootDamage.level = 0;
            upgrades.carryingCapacity.level = 0;

            wave.index = 0;
            wave.time = 0;

            altarItems.Clear();
        }

        public Upgrades.Ability GetAbility(Ability ability) => ability switch
        {
            Ability.MoveSpeed => upgrades.moveSpeed,
            Ability.HarvestRadius => upgrades.harvestRadius,
            Ability.HarvestStrength => upgrades.harvestStrength,
            Ability.ShootSpeed => upgrades.shootSpeed,
            Ability.ShootDamage => upgrades.shootDamage,
            Ability.CarryingCapacity => upgrades.carryingCapacity,
            _ => null,
        };

        public void GameStateMachineTrigger(string name)
        {
            LD52_GameStateMachine.instance.Trigger(name);
        }

        public void PlaySound(SoundEffectCollection soundEffect)
        {
            soundEffect.PlayRandomClip();
        }

        public void PlayDigOutSound()
        {
            PlaySound(digOutSound);
        }

        public void PlayHarvestSound()
        {
            PlaySound(harvestSound);
        }

        public void PlayHitSound()
        {
            PlaySound(hitSound);
        }

        public void PlayDeathSound()
        {
            PlaySound(deathSound);
        }

        public void PlayBulledSound()
        {
            PlaySound(bulletSound);
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