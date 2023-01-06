using System.Linq;

using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/Simple Animation Player")]
    public class SimpleAnimationPlayer : MonoBehaviour
    {
        [SerializeField] SimpleAnimation[] animations;

        public float avgTime => animations.Average(x => x.time);

        void OnEnable()
        {
            foreach (var animation in animations)
                animation.Play();
        }

        void OnDisable()
        {
            foreach (var animation in animations)
                animation.PlayReverse();
        }
    }
}