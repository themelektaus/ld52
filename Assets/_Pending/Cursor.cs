using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Cursor))]
    public class Cursor : MonoBehaviour
    {
        [System.Serializable]
        public struct State
        {
            public Texture2D texture;
            public Vector2 hotspot;

            public void Use()
            {
                if (texture)
                {
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.SetCursor(texture, hotspot,
#if UNITY_EDITOR
                            CursorMode.ForceSoftware
#else
                            CursorMode.Auto
#endif
                    );
                    UnityEngine.Cursor.visible = true;
                    return;
                }

                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.SetCursor(null, hotspot, CursorMode.Auto);
                UnityEngine.Cursor.visible = false;
            }
        }

        static readonly Stack<State> states = new();

        static bool paused;

        public static void Pause()
        {
            if (paused)
                return;

            paused = true;

            foreach (var state in states)
            {
                if (state.texture)
                {
                    state.Use();
                    return;
                }
            }

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            UnityEngine.Cursor.visible = true;
        }

        public static void Resume()
        {
            if (!paused)
                return;

            if (states.Count > 0)
                states.Peek().Use();

            paused = false;
        }

        [SerializeField] State state;

        void OnEnable()
        {
            states.Push(state);
            if (!paused)
                state.Use();
        }

        void OnDisable()
        {
            states.Pop();
            if (!paused)
                if (states.Count > 0)
                    states.Peek().Use();
        }
    }
}