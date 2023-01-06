using System.Collections.Generic;

namespace Prototype
{
    public abstract class Query<T> : ScriptableObject
    {
        [UnityEngine.SerializeReference]
        protected List<T> selectors;
    }
}