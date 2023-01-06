namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.Intro)]
		void AfterEnter_Intro()
		{
			gameStateInstances.Add(intro);
			gameStateInstances.Add(introUI, mainCanvas);
		}

		[BeforeExit(States.Intro)]
		void BeforeExit_Intro()
		{
			gameStateInstances.DestroyChildrenOf(introUI);
		}

		[AfterExit(States.Intro)]
		void AfterExit_Intro()
        {
			gameStateInstances.Clear();
		}
	}
}