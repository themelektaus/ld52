namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.Settings)]
		void AfterEnter_Settings()
		{
			gameStateInstances.Add(settings);
			gameStateInstances.Add(settingsUI, mainCanvas);
		}

		[BeforeExit(States.Settings)]
		void BeforeExit_Settings()
		{
			gameStateInstances.DestroyChildrenOf(settingsUI);
		}

		[AfterExit(States.Settings)]
		void AfterExit_Settings()
		{
			gameStateInstances.Remove(settings);
			gameStateInstances.Remove(settingsUI);
		}
	}
}