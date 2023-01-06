using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(TempValue))]
    public class TempValue : MonoBehaviour
    {
        [SerializeField] float duration;
        [SerializeField] Reference value;

        object originalValue;
        float? time;

        void Update()
        {
            if (!time.HasValue)
                return;

            time = Mathf.Max(0, time.Value - Time.deltaTime);
            if (time.Value != 0)
                return;

            time = null;
            value.Set(originalValue);
        }

        public void SetBool(bool value)
        {
            Set(value);
        }

        public void SetInteger(int value)
        {
            Set(value);
        }

        public void SetFloat(float value)
        {
            Set(value);
        }

        void Set(object value)
        {
            if (!time.HasValue)
                originalValue = this.value.Get();

            time = duration;
            this.value.Set(value);
        }
    }
}