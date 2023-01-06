using System.Linq;
using UnityEngine;

namespace Prototype
{
    [System.Serializable]
    public class ObjectByNameSelector : ObjectSelector
    {
        public string name;

        public override bool Match(Object value, bool _)
        {
            if (!value)
                return string.IsNullOrEmpty(name);

            return value.name == name;
        }

        public override Object Find()
        {
            return Object.FindObjectsOfType<GameObject>(true).FirstOrDefault(x => Match(x, false));
        }

        public override Object[] FindAll()
        {
            return Object.FindObjectsOfType<GameObject>(true).Where(x => Match(x, false)).ToArray();
        }
    }
}