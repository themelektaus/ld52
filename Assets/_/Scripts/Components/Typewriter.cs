using TMPro;
using UnityEngine;

namespace Prototype
{
    public class Typewriter : MonoBehaviour
    {
        public enum CharacterEffect { None, Alpha }

        public TMP_Text uiText;

        [SerializeField] CharacterEffect characterEffect = CharacterEffect.None;
        [SerializeField] float characterIndex;

        public bool finished
        {
            get
            {
                var textInfo = uiText.textInfo;

                var characterCount = textInfo.characterCount;
                if (0 >= characterCount)
                    return false;

                var characterInfo = textInfo.characterInfo;
                if (characterCount > characterInfo.Length)
                    return false;

                if (characterIndex != textInfo.characterCount)
                    return false;

                return true;
            }
        }

        void OnEnable()
        {
            ProcessColor(x => true, (_, _) => new());
        }

        void Update()
        {
            ProcessColor(
                
                info =>
                {
                    return info.isVisible;
                },

                (color, index) =>
                {
                    switch (characterEffect)
                    {
                        case CharacterEffect.None:
                            color.a = (byte) (((int) Mathf.Clamp01(characterIndex - index)) * 255);
                            break;

                        case CharacterEffect.Alpha:
                            color.a = (byte) (Mathf.Clamp01(characterIndex - index) * 255);
                            break;
                    }
                    return color;
                }
            );
        }

        void ProcessColor(System.Func<TMP_CharacterInfo, bool> isVisible, System.Func<Color32, int, Color32> process)
        {
            var color = uiText.color;
            color.a = 0;
            uiText.color = color;
            
            var textInfo = uiText.textInfo;
            
            for (var i = 0; i < textInfo.characterCount; i++)
            {
                var characterInfo = textInfo.characterInfo[i];
                
                if (isVisible(characterInfo))
                {
                    int vertexIndex = characterInfo.vertexIndex;
                    var materialIndex = characterInfo.materialReferenceIndex;
                    var colors = textInfo.meshInfo[materialIndex].colors32;

                    for (var j = 0; j < 4; j++)
                        colors[vertexIndex + j] = process(colors[vertexIndex + j], i);
                }
            }

            uiText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        public void IncreaseCharacterIndex(float value)
        {
            characterIndex = Mathf.Clamp(characterIndex + value, 0, uiText.text.Length);
        }
    }
}