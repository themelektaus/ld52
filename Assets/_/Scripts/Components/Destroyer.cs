using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Destroyer))]
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] float delay;
        
        public void DestroyTarget()
        {
            this.Wait(delay).Destroy(target).Start();
        }
    }
}