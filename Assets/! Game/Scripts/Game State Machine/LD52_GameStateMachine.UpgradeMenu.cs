namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.UpgradeMenu)]
		void AfterEnter_UpgradeMenu()
		{
			gameStateInstances.Add(upgradeMenu);
			gameStateInstances.Add(upgradeMenuUI, mainCanvas);
		}

		[BeforeExit(States.UpgradeMenu)]
		void BeforeExit_UpgradeMenu()
		{
			gameStateInstances.DestroyChildrenOf(upgradeMenuUI);
		}

		[AfterExit(States.UpgradeMenu)]
		void AfterExit_UpgradeMenu()
		{
			gameStateInstances.Clear();
		}
	}
}