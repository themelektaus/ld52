using UnityEngine;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(PlayOnAwake))]
    public class PlayOnAwake : MonoBehaviour
    {
        [SerializeField] float fadeIn = 3;

        AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.time = Random.Range(0, audioSource.clip.length);
            audioSource.Play();
        }

        void Update()
        {
            if (audioSource.volume == 1)
            {
                Destroy(this);
                return;
            }

            audioSource.volume = Mathf.Clamp01(audioSource.volume + Time.deltaTime / fadeIn);
        }
    }
}