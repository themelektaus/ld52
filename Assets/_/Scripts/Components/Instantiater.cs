using UnityEngine;
using UnityEngine.Events;

namespace Prototype
{
    using Pending;
    using System.Collections.Generic;
    using System.Linq;

    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Instantiater))]
    public class Instantiater : MonoBehaviour
    {
        [SerializeField] GameObject prefab;
        [SerializeField] Transform parent;
        [SerializeField] UnityEvent onDestroy;

        readonly List<GameObject> instances = new();

        public void Instantiate()
        {
            var gameObject = prefab.Instantiate(parent);
            gameObject.AddComponent<OnDestroy_>().AddListener(() =>
            {
                instances.Remove(gameObject);
                onDestroy.Invoke();
            });
            instances.Add(gameObject);
        }

        public void DestroyInstances()
        {
            foreach (var instance in instances.ToList())
                instance.Destroy();
        }
    }
}