using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Prototype.Destroy))]
    public class Destroy : MonoBehaviour
	{
        public float delay;

        Sequence.Instance sequence;

        void Awake()
        {
            sequence = this
                .Wait(delay)
                .Destroy(gameObject)
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