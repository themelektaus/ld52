using System.Linq;

using UnityEngine;

namespace Prototype
{
    public class LD52_GlobalAlpha : MonoBehaviour
    {
        public const int CAPACITY = 3;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            Shader.SetGlobalFloatArray("_GlobalAlpha", Enumerable.Repeat(1f, CAPACITY).ToArray());
        }

        void OnValidate()
        {
            if (Application.isPlaying)
                return;

            if (GetComponents<Component>().Length > 2)
                return;

            InitializeOnLoad();

            System.Collections.IEnumerator _()
            {
                yield return null;
                Utils.DestroyImmediateInEditor(gameObject);
            }
            StartCoroutine(_());
        }
#endif

        static LD52_GlobalAlpha _instance;
        static LD52_GlobalAlpha instance
        {
            get
            {
                if (_instance is null)
                    _instance = new GameObject(nameof(LD52_GlobalAlpha)).AddComponent<LD52_GlobalAlpha>();

                return _instance;
            }
        }

        SmoothFloat[] alpha;
        float[] target;

        public static void SetValue(int i, float value)
        {
            if (SetValueInEditor(i, value))
                return;

            instance.target[i] = value;
            SetTarget(i, value);
            instance.alpha[i].value = value;
            instance.Update();
        }

        public static void SetTarget(int i, float value)
        {
            if (SetValueInEditor(i, value))
                return;

            System.Collections.IEnumerator _()
            {
                yield return new WaitForSeconds(.25f);
                instance.target[i] = value;
            }

            if (!instance)
                return;

            instance.StopAllCoroutines();
            instance.StartCoroutine(_());
        }

        static bool SetValueInEditor(int i, float value)
        {
            if (Application.isPlaying)
                return false;

            var alpha = Shader.GetGlobalFloatArray("_GlobalAlpha");
            alpha[i] = value;
            Shader.SetGlobalFloatArray("_GlobalAlpha", alpha);
            return true;
        }

        void Awake()
        {
            alpha = new SmoothFloat[CAPACITY];
            target = new float[CAPACITY];

            for (int i = 0; i < CAPACITY; i++)
                alpha[i] = new(0, .15f) { threshold = .05f };
        }

        void Update()
        {
            Shader.SetGlobalFloatArray("_GlobalAlpha", alpha.Select(x => x.current).ToArray());

            for (int i = 0; i < CAPACITY; i++)
            {
                target[i] = Mathf.Clamp01(target[i]);
                alpha[i].target = target[i];
                alpha[i].Update();
            }
        }
    }
}