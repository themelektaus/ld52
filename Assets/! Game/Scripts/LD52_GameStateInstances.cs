using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Prototype
{
    public class LD52_GameStateInstances
    {
        readonly LD52_GameStateMachine gameStateMachine;

        readonly HashSet<LD52_GameStateInstance> instances = new();

        public LD52_GameStateInstances(LD52_GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        public LD52_GameStateInstance Add(GameObject originalGameObject, Transform parent = null, bool active = true)
        {
            if (!originalGameObject)
                return null;

            var instance = new LD52_GameStateInstance(gameStateMachine)
            {
                originalGameObject = originalGameObject,
                gameObject = originalGameObject.Instantiate(),
                parent = parent
            };

            if (!active)
                instance.gameObject.SetActive(false);

            instances.Add(instance);

            if (instance.gameObject.TryGetComponent(out LD52_IGameState x))
                x.gameStateInstance = instance;

            if (parent)
                instance.gameObject.transform.SetParent(parent, false);

            return instance;
        }

        public LD52_GameStateInstance Get(GameObject originalGameObject)
        {
            return instances.FirstOrDefault(x => x.originalGameObject == originalGameObject);
        }

        public void Remove(params GameObject[] originalGameObjects)
        {
            foreach (var originalGameObject in originalGameObjects)
            {
                var instance = Get(originalGameObject);
                if (instance is null)
                    continue;

                instance.gameObject.Destroy();
                instances.Remove(instance);
            }
        }

        public void DestroyChildrenOf(GameObject originalGameObject)
        {
            var destroyables = Get(originalGameObject).gameObject.GetComponentsInChildren<IDestroyable>();
            foreach (var destroyable in destroyables)
                destroyable.Destroy();
        }

        public void Clear(params GameObject[] except)
        {
            foreach (var instance in instances)
                if (!except.Contains(instance.originalGameObject))
                    instance.gameObject.Destroy();

            instances.RemoveWhere(x => !except.Contains(x.originalGameObject));
        }
    }
}