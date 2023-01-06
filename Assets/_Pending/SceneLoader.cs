using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.Pending
{
    [AddComponentMenu(Const.PROTOTYPE_PENDING + "/" + nameof(SceneLoader))]
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] int[] scenes;

        int loaded;
        
        void OnEnable()
        {
            foreach (int scene in scenes)
            {
                var process = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                process.completed += OnSceneLoadCompleted;
            }
        }

        void OnSceneLoadCompleted(AsyncOperation _)
        {
            loaded++;
            
            if (loaded != scenes.Length)
                return;

            Destroy(gameObject);

            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        }
    }
}