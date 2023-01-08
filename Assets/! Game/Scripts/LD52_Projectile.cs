using UnityEngine;

namespace Prototype
{
    public class LD52_Projectile : MonoBehaviour
    {
        public static Observable<HitMessage> hitSubject = new();

        public struct HitMessage
        {
            public LD52_Projectile sender;
            public Collider reciever;
            public float damage;
        }

        [SerializeField] float speed = 20;
        [SerializeField] float radius = .25f;
        [SerializeField] float damage = 1;

        [HideInInspector] public Vector2 direction;

        int colliderCount;
        readonly Collider[] colliders = new Collider[20];

        void Update()
        {
            transform.localScale = Vector3.one * (radius * 2);
            transform.position += direction.ToX0Z().normalized * (Time.deltaTime * speed);

            colliderCount = Physics.OverlapSphereNonAlloc(transform.position, radius, colliders);
            for (int i = 0; i < colliderCount; i++)
            {
                hitSubject.Notify(new()
                {
                    sender = this,
                    reciever = colliders[i],
                    damage = damage
                });
            }
        }
    }
}