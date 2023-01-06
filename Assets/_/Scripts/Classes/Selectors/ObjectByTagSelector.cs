using UnityEngine;

namespace Prototype
{
    [System.Serializable]
    public class ObjectByTagSelector : ObjectSelector
    {
        public string tag;

        public override bool Match(Object value, bool _)
        {
            if (!value)
                return string.IsNullOrEmpty(tag);

            var gameObject = value.GetGameObject();
            if (!gameObject)
                return string.IsNullOrEmpty(tag);

            return gameObject.CompareTag(tag);
        }

        public override Object Find()
        {
            return GameObject.FindGameObjectWithTag(tag);
        }

        public override Object[] FindAll()
        {
            return GameObject.FindGameObjectsWithTag(tag);
        }
    }
}