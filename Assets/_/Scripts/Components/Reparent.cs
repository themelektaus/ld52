using UnityEngine;

namespace Prototype
{
    [AddComponentMenu(Const.PROTOTYPE + "/" + nameof(Reparent))]
    public class Reparent : MonoBehaviour
    {
        public string parentPath;

        bool hasOwner;
        GameObject owner;

        void Awake()
        {
            var parent = transform.parent;
            if (parent)
            {
                hasOwner = true;
                owner = parent.gameObject;
            }

            if (string.IsNullOrEmpty(parentPath))
            {
                transform.parent = null;
                return;
            }

            var hierarchy = parentPath.Split('/');
            foreach (var node in hierarchy)
            {
                var n = node.Trim();
                if (string.IsNullOrEmpty(n))
                    continue;

                var currentParent = parent;
                if (currentParent)
                {
                    parent = currentParent.Find(n);
                }
                else
                {
                    var parentGameObject = GameObject.Find($"/{n}");
                    if (parentGameObject)
                        parent = parentGameObject.transform;
                }

                if (!parent)
                    parent = new GameObject(n).transform;

                parent.parent = currentParent;
            }

            transform.parent = parent;
        }

        void Update()
        {
            if (!hasOwner)
                return;

            if (owner)
                gameObject.SetActive(owner.activeInHierarchy);
            else
                gameObject.Destroy();
        }
    }
}