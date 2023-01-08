using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Prototype
{
    public partial class LD52_GameStateMachine : AnimatorStateBehaviour
    {
        [SerializeField] List<LD52_EnemyItem> enemyItems;

        [BeforeEnter(States.Ingame)]
        void BeforeEnter_Ingame()
        {
            blurOverride.weight = 0;

            EnableIngame();
        }

        [AfterEnter(States.Ingame)]
        void AfterEnter_Ingame(AnimatorStateInfo lastState)
        {
            if (!lastState.IsName(States.Pause))
            {
                var ingameInstance = gameStateInstances.Add(ingame);
                if (ingameInstance is not null)
                    ingameInstance.gameObject.name = "Ingame";

                var global = LD52_Global.instance;
                global.wave.time = 0;
                global.altarItems.Clear();

                int breaker = 10000;

                int budget = global.wave.budget;

                // budget must be greater than one because the cheapest enemy costs 2
                while (budget > 1)
                {
                    breaker--;
                    if (breaker < 0)
                    {
                        Debug.LogError($"{budget} 10000 oida, was is");
                        Debug.Break();
                        break;
                    }

                    var t = Random.value;

                    var enemyItem = enemyItems
                        .Where(x => x.rarity <= t)
                        .OrderByDescending(x => x.rarity)
                        .FirstOrDefault();

                    if (enemyItem.value > budget)
                        continue;

                    var spawnAreas = enemyItem.spawnAreaQuery.FindComponents<LD52_SpawnArea>();
                    var spawnArea = Utils.RandomPick(spawnAreas);

                    enemyItem.enemy.Instantiate(position: spawnArea.GetRandomPoint().ToX0Z());

                    budget -= enemyItem.value;
                }
            }

            gameStateInstances.Add(ingameUI, mainCanvas);
        }

        [Update(States.Ingame)]
        void Update_Ingame()
        {
            var global = LD52_Global.instance;

            UpdateWave(global);
        }

        void UpdateWave(LD52_Global global)
        {
            global.wave.time = Mathf.Min(global.wave.time + Time.deltaTime, global.wave.duration);

            if (global.wave.time == global.wave.duration)
            {
                if (
                    global.upgrades.moveSpeed.maxLevel <= global.upgrades.moveSpeed.level &&
                    global.upgrades.harvestRadius.maxLevel <= global.upgrades.harvestRadius.level &&
                    global.upgrades.harvestStrength.maxLevel <= global.upgrades.harvestStrength.level &&
                    global.upgrades.shootSpeed.maxLevel <= global.upgrades.shootSpeed.level &&
                    global.upgrades.shootDamage.maxLevel <= global.upgrades.shootDamage.level &&
                    global.upgrades.carryingCapacity.maxLevel <= global.upgrades.carryingCapacity.level
                )
                {
                    LD52_Global.instance.gameOverState = 2;
                    Trigger(Triggers.GameOver);
                }
                else
                {
                    LD52_Global.instance.gameOverState = 1;
                    Trigger(Triggers.EndOfWave);
                }
            }
        }

        [BeforeExit(States.Ingame)]
        void BeforeExit_Ingame()
        {
            LD52_Global.instance.UpdateDeadEnemiesValue();

            gameStateInstances.DestroyChildrenOf(ingameUI);
        }

        [AfterExit(States.Ingame)]
        void AfterExit_Ingame(AnimatorStateInfo nextState)
        {
            if (!nextState.IsName(States.Pause))
                gameStateInstances.Remove(ingame);

            gameStateInstances.Remove(ingameUI);

            blurOverride.weight = 1;
        }
    }
}