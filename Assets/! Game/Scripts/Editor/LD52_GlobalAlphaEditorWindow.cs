using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Prototype.Editor
{
    public class LD52_GlobalAlphaEditorWindow : EditorWindow
    {
        [MenuItem("Tools/LD52/Global Alpha")]
        public static void Open()
        {
            GetWindow<LD52_GlobalAlphaEditorWindow>("LD52 - Global Alpha");
        }

        void CreateGUI()
        {
            for (int i = 0; i < LD52_GlobalAlpha.CAPACITY; i++)
            {
                var slider = new Slider($"_GlobalAlpha[{i}]", 0, 1) { userData = i };
                slider.RegisterValueChangedCallback(x =>
                {
                    var alpha = Shader.GetGlobalFloatArray("_GlobalAlpha");
                    alpha[(int) slider.userData] = x.newValue;
                    Shader.SetGlobalFloatArray("_GlobalAlpha", alpha);
                });
                rootVisualElement.Add(slider);
            }
        }

        void Update()
        {
            var alpha = Shader.GetGlobalFloatArray("_GlobalAlpha");
            rootVisualElement.Query<Slider>().ForEach(x => x.SetValueWithoutNotify(alpha[(int) x.userData]));
        }
    }
}