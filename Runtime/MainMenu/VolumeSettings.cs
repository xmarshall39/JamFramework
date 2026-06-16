using UnityEngine;
using UnityEngine.UI;

namespace JamFramework
{
    public class VolumeSettings : MonoBehaviour
    {
        public Slider musicSlider, SFXSlider;

        private void OnEnable()
        {
            musicSlider.minValue = AudioManager.MIN_SLIDER_VALUE;
            musicSlider.maxValue = 1;

            SFXSlider.minValue = AudioManager.MIN_SLIDER_VALUE;
            SFXSlider.maxValue = 1;

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            SFXSlider.onValueChanged.AddListener(SetSFXVolume);

            musicSlider.value = AudioManager.Instance.GetVolume(AudioManager.MUSIC_VOLUME_KEY);
            SFXSlider.value = AudioManager.Instance.GetVolume(AudioManager.SFX_VOLUME_KEY);
        }

        private void OnDisable()
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            SFXSlider.onValueChanged.RemoveAllListeners();
        }


        public void SetMasterVolume(float volume)
        {
            AudioManager.Instance.SetVolume(AudioManager.MASTER_VOLUME_KEY, volume);
        }

        public void SetMusicVolume(float volume)
        {
            AudioManager.Instance.SetVolume(AudioManager.MUSIC_VOLUME_KEY, volume);
        }

        public void SetSFXVolume(float volume)
        {
            AudioManager.Instance.SetVolume(AudioManager.SFX_VOLUME_KEY, volume);
        }
    }
}
