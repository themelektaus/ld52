using UnityEngine;

namespace Prototype.Pending
{
	[AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Slider))]
	public class Slider : MouseBehaviour
	{
        [Range(0, 1)] public float value = .5f;
        
        [SerializeField] SpriteRenderer bar;
        [SerializeField] Transform handle;
        [SerializeField] TMPro.TextMeshPro handleText;

        bool dragging;
        Vector3 mouseScreenPosition;
        Vector3 handleLocalPosition;
        Vector3 handleScreenPosition;

        float min => -max;
        float max => (bar.size.x - handle.localScale.x) / 2;
        float current => Mathf.Clamp(handle.localPosition.x, min, max);

        protected override void OnDown()
        {
            dragging = true;
            mouseScreenPosition = Input.mousePosition;
            handleLocalPosition = handle.localPosition;
            handleScreenPosition = Utils.mainCamera.WorldToScreenPoint(handle.position);
        }

        protected override void OnUp()
        {
            dragging = false;
        }

        protected override void OnUpdate()
        {
            handleText.text = value.ToFormattedString(FloatFormat.Percent);
            
            if (!bar || !handle)
                return;
            
            if (dragging)
            {
                var mouseDelta = Input.mousePosition - mouseScreenPosition;
                handle.position = Utils.mainCamera.ScreenToWorldPoint(handleScreenPosition + mouseDelta);
                handle.localPosition = new(current, handleLocalPosition.y, handleLocalPosition.z);
                value = (current - min) / (max - min);
                return;
            }

            var position = handle.localPosition;
            position.x = Mathf.Lerp(min, max, value);
            handle.localPosition = position;
        }
    }
}