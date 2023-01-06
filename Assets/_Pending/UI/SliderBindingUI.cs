using UnityEngine;

namespace Prototype.Pending
{
    using SliderUI = UnityEngine.UI.Slider;

    [RequireComponent(typeof(SliderUI))]
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/Slider Binding (UI)")]
    public class SliderBindingUI : MonoBehaviour
	{
		[SerializeField] Reference floatReference;

        SliderUI slider;

        void Awake()
        {
            slider = GetComponent<SliderUI>();
        }
        
        void Update()
        {
            floatReference.Set(slider.value);
        }
    }
}