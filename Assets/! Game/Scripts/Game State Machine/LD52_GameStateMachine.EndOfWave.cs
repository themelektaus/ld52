using UnityEngine;

namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[BeforeEnter(States.EndOfWave)]
		void BeforeEnter_EndOfWave()
		{
			global.money += Mathf.Max(0, global.altarValue - global.wave.minAltarValue);
			global.money = Mathf.Max(0, global.money - global.deadEnemiesValue);
		}

		[AfterEnter(States.EndOfWave)]
		void AfterEnter_EndOfWave()
		{
			gameStateInstances.Add(endOfWave);
			gameStateInstances.Add(endOfWaveUI, mainCanvas);
		}

		[BeforeExit(States.EndOfWave)]
		void BeforeExit_EndOfWave()
		{
			gameStateInstances.DestroyChildrenOf(endOfWaveUI);
		}

		[AfterExit(States.EndOfWave)]
		void AfterExit_EndOfWave()
		{
			gameStateInstances.Clear();

			wave.Next();
		}
	}
}