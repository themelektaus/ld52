using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Trigger2D))]
    public class Trigger2D : MonoBehaviour
    {
        [SerializeField] ObjectQuery requirements;
        [SerializeField] UnityEvent<Collider2D> onEnter;
        [SerializeField] UnityEvent<Collider2D> onStay;
        [SerializeField] UnityEvent<Collider2D> onExit;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onEnter.Invoke(collision);
            onStay.Invoke(collision);
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onStay.Invoke(collision);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onExit.Invoke(collision);
        }
    }
}