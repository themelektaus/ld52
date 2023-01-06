using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(SliderBinding))]
    public class SliderBinding : MonoBehaviour
	{
		[SerializeField] Reference floatReference;

        Slider slider;
        UnityEngine.UI.Slider uiSlider;

        void Awake()
        {
            slider = GetComponent<Slider>();
            if (slider)
                slider.value = floatReference.Get<float>();

            uiSlider = GetComponent<UnityEngine.UI.Slider>();
            if (uiSlider)
                uiSlider.value = floatReference.Get<float>();
        }
        
        void Update()
        {
            if (slider)
                floatReference.Set(slider.value);

            if (uiSlider)
                floatReference.Set(uiSlider.value);
        }
    }
}