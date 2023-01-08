namespace Prototype
{
	public partial class LD52_GameStateMachine : AnimatorStateBehaviour
	{
		[BeforeEnter(States.EndOfWave)]
		void BeforeEnter_EndOfWave()
		{
			LD52_Global.instance.wave.index++;
		}
		
		[AfterEnter(States.EndOfWave)]
		void AfterEnter_EndOfWave()
		{
			gameStateInstances.Add(endOfWave);
			gameStateInstances.Add(endOfWaveUI, mainCanvas);
		}

		[BeforeExit(States.EndOfWave)]
		void BeforeExit_EndOfWave()
		{
			gameStateInstances.DestroyChildrenOf(endOfWaveUI);
		}

		[AfterExit(States.EndOfWave)]
		void AfterExit_EndOfWave()
		{
			gameStateInstances.Clear();
		}
	}
}