namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[BeforeEnter(States.MainMenu)]
		void BeforeEnter_MainMenu()
		{
			blurOverride.weight = 1;
		}

		[AfterEnter(States.MainMenu)]
		void AfterEnter_MainMenu()
		{
			gameStateInstances.Add(mainMenu);
			gameStateInstances.Add(mainMenuUI, mainCanvas);
		}

		[BeforeExit(States.MainMenu)]
		void BeforeExit_MainMenu()
		{
			gameStateInstances.DestroyChildrenOf(mainMenuUI);
		}

		[AfterExit(States.MainMenu)]
		void AfterExit_MainMenu()
		{
			gameStateInstances.Clear();
		}
	}
}