# Scene Manager
The Jam Framework SceneManager is a Singleton component that allows for scene transitions with built-in animated transitions.
## Setup
Add this component wherever you keep your Singletons. In the Launchpad, it should be setup already.
# Use
 `public void LoadScene(int sceneIndex, UnitySceneManagement.LoadSceneMode mode = UnitySceneManagement.LoadSceneMode.Single, bool fadeMusicOut = true)`  
 Loads a scene while playing a transition animation, and optionally fades out the current music track playing on the AudioManager
