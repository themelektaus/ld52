using UnityEngine;

namespace Prototype
{
    public class LD52_GameStateInstance
    {
        public readonly LD52_GameStateMachine gameStateMachine;

        public GameObject originalGameObject;
        public GameObject gameObject;
        public Transform parent;

        public LD52_GameStateInstance(LD52_GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }
    }
}