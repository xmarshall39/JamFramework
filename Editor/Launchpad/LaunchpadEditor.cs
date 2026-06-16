#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace JamFramework.Editor
{
    [InitializeOnLoad]
    public static class LaunchpadEditor
    {
        const string PREF_KEY = "JAMFRAMEWORK_EditorPlayFromLaunchpad";
        const string PlayFromLaunchpadMenuStr = "JamFramework/Always Start From Scene 0 &p";
        const string START_SCENE_PREF = "JAMFRAMEWORK_StartScenePath";

        static bool PlayFromLaunchpad
        {
            get { return EditorPrefs.HasKey(PREF_KEY) && EditorPrefs.GetBool(PREF_KEY); }
            set 
            {
                if (value)
                {
                    SceneAsset launchpadAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneUtility.GetScenePathByBuildIndex(0));
                    if (launchpadAsset == null)
                    {
                        Debug.LogWarning($"Cannot find scene at build index 0. Please adjust your build settings to enable \"Play From Launchpad\"");
                        return;
                    }

                    EditorSceneManager.playModeStartScene = launchpadAsset;
                }
                else
                {
                    EditorSceneManager.playModeStartScene = null;

                }
                EditorPrefs.SetBool(PREF_KEY, value);
            }
        }

        static LaunchpadEditor()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode && PlayFromLaunchpad)
            {
                EditorPrefs.SetString(START_SCENE_PREF, string.Empty);
                string launchpadPath = SceneUtility.GetScenePathByBuildIndex(0);
                SceneAsset launchpadAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(launchpadPath);
                if (launchpadAsset == null)
                {
                    Debug.LogWarning($"Cannot find scene at build index 0. Please adjust your build settings!!");
                    return;
                }

                if (EditorSceneManager.GetActiveScene().path != launchpadPath)
                {
                    EditorPrefs.SetString(START_SCENE_PREF, EditorSceneManager.GetActiveScene().path);
                }
                EditorSceneManager.playModeStartScene = launchpadAsset;
            }
        }

        [MenuItem(PlayFromLaunchpadMenuStr, false, 150)]
        static void PlayFromLaunchpadCheckMenu()
        {
            PlayFromLaunchpad = !PlayFromLaunchpad;
            Menu.SetChecked(PlayFromLaunchpadMenuStr, PlayFromLaunchpad);

            ShowNotifyOrLog(PlayFromLaunchpad ? "Play from Launchpad" : "Play from Current Scene");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(PlayFromLaunchpadMenuStr, true)]
        static bool PlayFromFirstSceneCheckMenuValidate()
        {
            Menu.SetChecked(PlayFromLaunchpadMenuStr, PlayFromLaunchpad);
            return true;
        }

        // This method is called before any Awake. It's the perfect callback for this feature
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void LoadFirstSceneAtGameBegins()
        {
            string startScenePath = EditorPrefs.GetString(START_SCENE_PREF);
            if (!string.IsNullOrEmpty(startScenePath))
            {
                EditorSceneManager.LoadSceneAsyncInPlayMode(startScenePath, new LoadSceneParameters(LoadSceneMode.Single));
                EditorPrefs.SetString(START_SCENE_PREF, string.Empty);
            }
            /*
            if (!PlayFromLaunchpad)
                return;

            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogWarning("The scene build list is empty. " +
                    "Please ensure your \"Launchpad\" scene is set to scene 0.");
                return;
            }

            //foreach (GameObject go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            //    go.SetActive(false);

            Scene currentScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            if (currentScene.IsValid())
            {
                if (currentScene.buildIndex == 0) return;

                EditorPrefs.SetInt("DOKA_LAST_EDITOR_SCENE_INDEX", currentScene.buildIndex);
            }

            EditorSceneManager.LoadScene(0);
            EditorSceneManager.LoadSceneAsyncInPlayMode(currentScene.path, new LoadSceneParameters(LoadSceneMode.Additive));
            */
        }

        static void ShowNotifyOrLog(string msg)
        {
            if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}

#endif