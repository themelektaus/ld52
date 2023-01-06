using UnityEngine;

namespace Prototype
{
    public partial class LD52_GameStateMachine : AnimatorStateBehaviour
    {
        [AfterEnter(States.Pause)]
        void AfterEnter_Pause()
        {
            gameStateInstances.Add(pause);
            gameStateInstances.Add(pauseUI, mainCanvas);
        }

        [BeforeExit(States.Pause)]
        void BeforeExit_Pause()
        {
            gameStateInstances.DestroyChildrenOf(pauseUI);
        }

        [AfterExit(States.Pause)]
        void AfterExit_Pause(AnimatorStateInfo nextState)
        {
            if (nextState.IsName(States.GameOver))
            {
                gameStateInstances.Clear();
                return;
            }

            gameStateInstances.Remove(pause);
            gameStateInstances.Remove(pauseUI);
        }
    }
}