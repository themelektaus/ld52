using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(Trigger))]
    public class Trigger : MonoBehaviour
    {
        [SerializeField] ObjectQuery requirements;
        [SerializeField] UnityEvent<Collider> onEnter;
        [SerializeField] UnityEvent<Collider> onStay;
        [SerializeField] UnityEvent<Collider> onExit;

        void OnTriggerEnter(Collider collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onEnter.Invoke(collision);
            onStay.Invoke(collision);
        }

        void OnTriggerStay(Collider collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onStay.Invoke(collision);
        }

        void OnTriggerExit(Collider collision)
        {
            if (requirements && !requirements.Match(collision))
                return;

            onExit.Invoke(collision);
        }
    }
}