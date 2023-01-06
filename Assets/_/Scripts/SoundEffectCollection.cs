using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace Prototype
{
    [CreateAssetMenu]
    public class SoundEffectCollection : ScriptableObject
    {
        [SerializeField] AudioMixerGroup defaultAudioMixerGroup;
        [SerializeField] List<AudioClip> clips;

        float lastPlayTime;

        protected override void Initialize()
        {
            lastPlayTime = -1;
        }

        public AudioClip GetClip() => clips[0];

        public SoundEffectInstance PlayClip(SoundEffectOptions options = new())
        {
            options.distanceRange = new();
            return PlayClipAt(new(), options);
        }

        public SoundEffectInstance PlayClipAt(Vector3 position, SoundEffectOptions options = new())
        {
            return PlayClipAt(GetClip(), position, options);
        }

        public SoundEffectInstance PlayRandomClip(SoundEffectOptions options = new())
        {
            options.distanceRange = new();
            return PlayRandomClipAt(new(), options);
        }

        public SoundEffectInstance PlayRandomClipAt(Vector3 position, SoundEffectOptions options = new())
        {
            return PlayClipAt(Utils.RandomPick(clips), position, options);
        }

        SoundEffectInstance PlayClipAt(AudioClip clip, Vector3 position, SoundEffectOptions options = new())
        {
            var time = Time.time;
            if (time - lastPlayTime < .1f)
                return null;

            lastPlayTime = time;

            var gameObject = new GameObject(clip.name);
            gameObject.SetActive(false);
            gameObject.transform.position = position;

            var reparent = gameObject.AddComponent<Reparent>();
            reparent.parentPath = $"Sound Effects/{name}";

            if (!options.audioMixerGroup)
                options.audioMixerGroup = defaultAudioMixerGroup;

            var soundEffectInstance = gameObject.AddComponent<SoundEffectInstance>();
            soundEffectInstance.clip = clip;
            soundEffectInstance.options = options;

            gameObject.SetActive(true);

            return soundEffectInstance;
        }
    }
}