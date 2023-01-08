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
        [SerializeField] Light light1;
        [SerializeField] Light light2;

        [SerializeField] GameObject altarEffect;
        [SerializeField] Transform altarEffectPosition;

        int colliderCount;
        readonly Collider[] colliders = new Collider[20];

        readonly SmoothFloat feed = new(0, .2f);

        void Update()
        {
            feed.target = Mathf.Max(0, feed.target - Time.deltaTime);
            feed.Update();

            light1.intensity = 1 + feed * 4;
            light2.intensity = 1 + feed * 4;

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

        public void Feed()
        {
            if (feed.target == 0)
                altarEffect.Instantiate(position: altarEffectPosition.position);

            feed.target = 1;
        }
    }
}