namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[BeforeEnter(States.Exit)]
		void BeforeEnter_Exit()
        {
			gameStateInstances.Clear();
		}

		[AfterEnter(States.Exit)]
		void AfterEnter_Exit()
		{
			gameStateInstances.Add(exit);
			gameStateInstances.Add(exitUI, mainCanvas);

#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif
		}
	}
}