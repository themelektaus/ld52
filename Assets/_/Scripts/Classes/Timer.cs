using UnityEngine;

namespace Prototype
{
    public class Timer
    {
        public System.Func<float> getDeltaTime = () => Time.deltaTime;

        Vector2 interval;
        float time;

        public bool Update(float interval, bool clearTime = true)
        {
            return Update(new Vector2(interval, interval), clearTime);
        }

        public bool Update(Vector2 interval, bool clearTime = true)
        {
            if (!Utils.Approximately(this.interval, interval))
            {
                this.interval = interval;
                time = 0;
                if (!clearTime)
                    SetupTime();
            }

            bool result = false;

            if (time <= 0)
            {
                SetupTime();
                result = true;
            }

            time -= getDeltaTime();

            return result;
        }

        void SetupTime()
        {
            if (interval.y > interval.x)
                time += Utils.RandomRange(interval);
            else
                time += interval.x;
        }
    }
}