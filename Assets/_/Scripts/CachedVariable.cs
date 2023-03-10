using System;
using UnityEngine;

namespace Prototype
{
    public abstract class CachedVariable<T>
    {
        T current;
        
        readonly Func<T, T> getCurrent;

        public CachedVariable(Func<T> getCurrent) : this(_ => getCurrent())
        {

        }

        public CachedVariable(Func<T, T> getCurrent)
        {
            current = Activator.CreateInstance<T>();
            this.getCurrent = getCurrent;
        }

        protected abstract bool Update();

        void UpdateInternal()
        {
            if (!Update())
                return;

            current = getCurrent(current);
        }

        public static implicit operator T(CachedVariable<T> @this)
        {
            @this.UpdateInternal();
            return @this.current;
        }
    }

    public class FrameTimeVariable<T> : CachedVariable<T>
    {
        int? frameCount;

        public FrameTimeVariable(Func<T> getCurrent) : base(getCurrent) { }
        public FrameTimeVariable(Func<T, T> getCurrent) : base(getCurrent) { }

        protected override bool Update()
        {
            if (frameCount != Time.frameCount)
            {
                frameCount = Time.frameCount;
                return true;
            }
            return false;
        }
    }
}