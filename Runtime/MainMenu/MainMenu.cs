using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JamFramework
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Settings Page Basics")]
        public Slider SFXSlider, MusicSlider;

        public void PlayGame(int sceneIndex)
        {
            SceneManager.Instance.LoadScene(sceneIndex);
        }


        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
