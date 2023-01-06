using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(SetFloatOverTime))]
    public class SetFloatOverTime : MonoBehaviour
	{
		[SerializeField] Object @object;
        [SerializeField] AnimationCurve curve = new(new(0, 0), new(1, 1));
        [SerializeField] float speed = 1;

        [SerializeField] Reference value;

        void Update()
        {
            var value = Mathf.Abs(1 - Time.time * speed % 2);
            value = curve.Evaluate(value);
            this.value.Set(value);
        }
    }
}