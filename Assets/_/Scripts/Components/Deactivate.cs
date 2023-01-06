using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Deactivate))]
    public class Deactivate : MonoBehaviour
	{
		public float delay;

        Sequence.Instance sequence;

        void Awake()
        {
            sequence = this
                .Wait(delay)
                .Deactivate(gameObject)
                .Build();
        }

        void OnEnable()
        {
            sequence.Start();
        }

        void OnDisable()
        {
            sequence.Stop();
        }
    }
}