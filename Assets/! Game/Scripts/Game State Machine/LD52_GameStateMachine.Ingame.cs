using UnityEngine;

namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
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
				gameStateInstances.Add(ingame);
			gameStateInstances.Add(ingameUI, mainCanvas);
		}

		[Update(States.Ingame)]
		void Update_Ingame()
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