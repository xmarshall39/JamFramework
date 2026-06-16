using System.Collections;
using UnityEngine;
using UnitySceneManagement = UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
namespace JamFramework
{
    /*
     * 
     * 
     */
    public class SceneManager : MonoBehaviour
    {
        #region Singleton
        private static SceneManager _instance;
        public static SceneManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        private Coroutine sceneLoadRoutine;
        public Animator transitionAnimator;

        // TODO: Allow the user to configure what scene transitions play
        public void LoadScene(int sceneIndex, UnitySceneManagement.LoadSceneMode mode = UnitySceneManagement.LoadSceneMode.Single, bool fadeMusicOut = true)
        {
            if (sceneLoadRoutine == null)
            {
                sceneLoadRoutine = StartCoroutine(DoLoadScene(sceneIndex, mode, fadeMusicOut));
            }
            else
            {
                Debug.Log("Cannot load a scene while another is being loaded!");
            }
        }

        private IEnumerator DoLoadScene(int sceneIndex, UnitySceneManagement.LoadSceneMode mode, bool fadeMusicOut)
        {
            // TODO: Create a better system for generalizing transition trigger names and getting animation durations
            transitionAnimator.SetTrigger("TransitionIn");
            if (fadeMusicOut) AudioManager.Instance.FadeMusicOut(0.85f);

            yield return transitionAnimator.WaitForAnimationComplete();
            transitionAnimator.ResetTrigger("TransitionIn");

            yield return UnitySceneManager.LoadSceneAsync(sceneIndex, mode);

            transitionAnimator.SetTrigger("TransitionOut");
            yield return transitionAnimator.WaitForAnimationComplete();
            transitionAnimator.ResetTrigger("TransitionOut");

            sceneLoadRoutine = null;
        }
    }
}
