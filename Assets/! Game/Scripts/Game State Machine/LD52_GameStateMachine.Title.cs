namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.Title)]
		void AfterEnter_Title()
		{
			gameStateInstances.Add(title);
			gameStateInstances.Add(titleUI, mainCanvas);
		}

		[BeforeExit(States.Title)]
		void BeforeExit_Title()
		{
			gameStateInstances.DestroyChildrenOf(titleUI);
		}

		[AfterExit(States.Title)]
		void AfterExit_Title()
		{
			gameStateInstances.Clear();
		}
	}
}