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

                int budget = global.wave.budget;
                while (budget > 0)
                {
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

            if (!UpdateWave(global))
                return;

            UpdateSpawn(global);
        }

        bool UpdateWave(LD52_Global global)
        {
            global.wave.time = Mathf.Min(global.wave.time + Time.deltaTime, global.wave.duration);

            if (global.wave.time == global.wave.duration)
            {
                Trigger(Triggers.EndOfWave);
                return false;
            }

            return true;
        }

        void UpdateSpawn(LD52_Global global)
        {

        }

        [BeforeExit(States.Ingame)]
        void BeforeExit_Ingame()
        {
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