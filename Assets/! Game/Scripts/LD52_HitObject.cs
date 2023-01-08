using UnityEngine;

namespace Prototype
{
	public class LD52_HitObject : MonoBehaviour
	{
        public float hitWeight;
        public Animator animator;

        void Update()
        {
            hitWeight = Mathf.Max(0, hitWeight - Time.deltaTime);
            animator.SetLayerWeight(1, hitWeight);
        }
    }
}