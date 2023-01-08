using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Destroyer))]
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] float delay;
        [SerializeField] bool destroyOnAwake;

        void Awake()
        {
            if (destroyOnAwake)
                DestroyTarget();
        }

        public void DestroyTarget()
        {
            this.Wait(delay).Destroy(target).Start();
        }
    }
}