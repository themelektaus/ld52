using UnityEngine;

namespace Prototype
{
    [CreateAssetMenu]
    public class LD52_EnemyItem : UnityEngine.ScriptableObject
    {
        public GameObject enemy;
        public ObjectQuery spawnAreaQuery;
        public int value = 1;
        public int weight = 1;
        [Range(0, 1)] public float rarity;
    }
}