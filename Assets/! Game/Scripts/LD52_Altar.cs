using UnityEngine;

namespace Prototype
{
	public class LD52_Altar : MonoBehaviour
	{
        public static Observable<WantItemMessage> wantItemSubject = new();

        public struct WantItemMessage
        {
            public LD52_Altar sender;
            public Collider reciever;
        }

        [SerializeField] float radius = 2;

        int colliderCount;
        readonly Collider[] colliders = new Collider[20];

        void Update()
        {
            colliderCount = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders);
            for (int i = 0; i < colliderCount; i++)
            {
                wantItemSubject.Notify(new()
                {
                    sender = this,
                    reciever = colliders[i]
                });
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new(0, 1, 1, .25f);
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}