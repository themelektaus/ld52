using UnityEngine;

namespace Prototype
{
	public class LD52_ProjectileHitDestroy : MonoBehaviour, IObserver<LD52_Projectile.HitMessage>
	{
        [SerializeField] new Collider collider;
        
        void OnEnable()
        {
            LD52_Projectile.hitSubject.Register(this, x => x.reciever == collider);
        }

        void OnDisable()
        {
            LD52_Projectile.hitSubject.Unregister(this);
        }

        public void ReceiveNotification(LD52_Projectile.HitMessage message)
        {
            message.sender.gameObject.Destroy();
        }
    }
}