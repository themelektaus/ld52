namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.Credits)]
		void AfterEnter_Credits()
		{
			gameStateInstances.Add(credits);
			gameStateInstances.Add(creditsUI, mainCanvas);
		}

		[BeforeExit(States.Credits)]
		void BeforeExit_Credits()
		{
			gameStateInstances.DestroyChildrenOf(creditsUI);
		}

		[AfterExit(States.Credits)]
		void AfterExit_Credits()
		{
			gameStateInstances.Clear();
		}
	}
}