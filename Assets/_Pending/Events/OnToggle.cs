using UnityEngine;
using UnityEngine.Events;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/On Toggle (Linked)")]
    public class OnToggle : MonoBehaviour
    {
        [SerializeField] Behaviour reference;

        [SerializeField] UnityEvent onEnable;
        [SerializeField] UnityEvent onDisable;

        bool? _enabled;

        void OnEnable()
        {
            _enabled = null;
        }

        void OnDisable()
        {
            if (!reference)
                onDisable.Invoke();
        }

        void Update()
        {
            var reference = this.reference;
            if (!reference)
                reference = this;

            if (_enabled.HasValue && _enabled.Value == reference.enabled)
                return;

            _enabled = reference.enabled;
            if (_enabled.Value)
                onEnable.Invoke();
            else
                onDisable.Invoke();
        }
    }
}