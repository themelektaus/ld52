using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/Simple Animation")]
    public class SimpleAnimation : MonoBehaviour
    {
        [SerializeField] bool playOnAwake;

        public bool writeTargetTime = true;

        public InterpolationCurve.InterpolationCurve curve;
        [Range(0, 20)] public float speed = 1;
        public float min = 0;
        public float max = 1;

        [FormerlySerializedAs("boolThresholdPercentage")]
        [Range(0, 1)] public float boolThreshold = .5f;

        [Range(0, 1)] public float targetTime = 0;
        public bool loop;
        public ReferenceCollection references = new();

        [SerializeField] UnityEvent onFinish;

        float? playTime;

        public float time { get; private set; }
        bool reverse;

        [Button] public bool _Play;

        void Awake()
        {
            if (playOnAwake)
            {
                Play();
                return;
            }

            if (writeTargetTime)
            {
                Stop();
                Update();
                return;
            }
        }

        void Start()
        {
            curve = curve.ToRuntimeObject();
        }

        void Update()
        {
            curve.min = min;
            curve.max = max;

            if (playTime.HasValue)
            {
                playTime += (Time.deltaTime * (reverse ? -1 : 1)) * speed;

                if (writeTargetTime)
                {
                    targetTime = Mathf.Clamp01(playTime.Value);
                }

                if ((reverse && playTime <= 0) || (!reverse && playTime >= 1))
                {
                    if (loop)
                    {
                        playTime += reverse ? 1 : -1;
                    }
                    else
                    {
                        playTime = null;
                        onFinish.Invoke();
                    }
                }
                time = playTime ?? targetTime;
            }
            else
            {
                if (time < targetTime)
                {
                    time = Mathf.Min(time + Time.deltaTime * speed, targetTime);
                }
                else if (time > targetTime)
                {
                    time = Mathf.Max(time - Time.deltaTime * speed, targetTime);
                }
            }

            float value = curve.Evaluate(time);

            foreach (var reference in references.items)
            {
                var t = Mathf.Min(boolThreshold, .99f);

                if (min < max)
                {
                    reference.Set(value, Mathf.Lerp(min, max, t));
                    continue;
                }

                if (min > max)
                {
                    reference.Set(value, Mathf.InverseLerp(max, min, t));
                    continue;
                }

                reference.Set(value);
            }
        }

        public void Play() => Play(reverse: false);

        public void PlayReverse() => Play(reverse: true);

        void Play(bool reverse)
        {
            this.reverse = reverse;

            if (playTime is not null)
                return;

            if (reverse)
            {
                playTime = 1;
                time = 1;
                return;
            }

            playTime = 0;
            time = 0;
        }

        public void Stop()
        {
            playTime = null;

            time = reverse ? 1 : 0;

            if (writeTargetTime)
                targetTime = time;
        }

        public IEnumerator PlayRoutine() => PlayRoutine(reverse: false);

        public IEnumerator PlayReverseRoutine() => PlayRoutine(reverse: true);

        IEnumerator PlayRoutine(bool reverse)
        {
            Play(reverse);

            while (playTime.HasValue)
                yield return null;
        }
    }
}