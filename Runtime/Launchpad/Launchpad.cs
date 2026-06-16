using UnityEngine;

namespace JamFramework
{
    /*
     * The Launchpad is a scene that will occupy scene 0 of your project and will be persistent while the application is running.
     * Everything in the Launchpad will be marked with DontDestroyOnLoad to avoid restrictions on how you manage your scenes.
     * If allowed, the Launchpad will be injected in the editor before your scene when you press play
     * Use the Launchpad to house your singletons and other persistent objects.
     * With this, there will be no need to have game managers in every scene just for testing.
     * Still, you should avoid accessing Singletons in Awake() as usual to prevent red-herring Editor errors.
     * In builds, the Launchpad will automatically load scene 1 after it's initialized itself.
     */
    public class Launchpad : MonoBehaviour
    {
        private static Launchpad instance = null;
        public static Launchpad Instance { get => instance; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                foreach (GameObject go in roots)
                {
                    DontDestroyOnLoad(go);

                    // I want to avoid running repeat Awake() calls on our game managers.
                    // This is specifically done so we don't initialize ImGui more than once.
                    foreach (Transform child in this.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
#if !UNITY_EDITOR
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
#endif
        }
    }
}
