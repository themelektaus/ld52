namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[AfterEnter(States.Intro)]
		void AfterEnter_Intro()
		{
			this.Wait(2).Start(() => ChangeMusic(global.music, .6f));

			gameStateInstances.Add(intro);
			gameStateInstances.Add(introUI, mainCanvas);

			this.Wait(5).Start(() => Trigger(Triggers.Next));
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