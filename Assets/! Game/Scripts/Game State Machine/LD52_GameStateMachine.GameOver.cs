namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[BeforeEnter(States.GameOver)]
		void BeforeEnter_GameOver()
		{
			DisableIngame();
		}

		[AfterEnter(States.GameOver)]
		void AfterEnter_GameOver()
		{
			gameStateInstances.Add(gameOver);
			gameStateInstances.Add(gameOverUI, mainCanvas);
		}

		[BeforeExit(States.GameOver)]
		void BeforeExit_GameOver()
		{
			gameStateInstances.DestroyChildrenOf(gameOverUI);
		}

		[AfterExit(States.GameOver)]
		void AfterExit_GameOver()
		{
			gameStateInstances.Clear();
		}
	}
}