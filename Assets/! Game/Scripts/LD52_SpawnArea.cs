using UnityEngine;

namespace Prototype
{
	public class LD52_SpawnArea : MonoBehaviour
	{
        [SerializeField] ObjectQuery query;

        public Vector2 radius = new(5, 10);

        void Awake()
        {
            query.ClearCache();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius.x);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius.y);
        }

        public Vector2 GetRandomPoint()
        {
            var direction = Random.insideUnitCircle.normalized;
            return transform.position.ToXZ() + Vector2.Lerp(direction * radius.x, direction * radius.y, Random.value);
        }
    }
}