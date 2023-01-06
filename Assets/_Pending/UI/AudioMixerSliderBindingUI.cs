using UnityEngine;
using UnityEngine.Audio;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/Audio Mixer Slider Binding (UI)")]
    public class AudioMixerSliderBindingUI : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] string parameter;
        [SerializeField] bool persistent;

        UnityEngine.UI.Slider slider;
        
        float decibel;

        void Awake()
        {
            slider = GetComponent<UnityEngine.UI.Slider>();
            
            if (!persistent)
                return;

            if (PlayerPrefs.HasKey($"{mixer.name}__{parameter}"))
            {
                decibel = PlayerPrefs.GetFloat($"{mixer.name}__{parameter}");
                mixer.SetFloat(parameter, decibel);
            }
        }

        void OnEnable()
        {
            if (mixer.GetFloat(parameter, out float decibel))
                slider.value = FromDecibel(decibel);
        }

        void Update()
        {
            var decibel = ToDecibel(slider.value);
            mixer.SetFloat(parameter, decibel);

            if (this.decibel == decibel)
                return;

            this.decibel = decibel;

            if (!persistent)
                return;

            PlayerPrefs.SetFloat($"{mixer.name}__{parameter}", decibel);
            PlayerPrefs.Save();
        }

        static float FromDecibel(float decibel)
        {
            return Mathf.Pow(10, decibel / 80);
        }

        static float ToDecibel(float value)
        {
            return Mathf.Log10(Mathf.Max(value, .0001f)) * 80;
        }
    }
}