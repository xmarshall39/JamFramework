using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Pool;
using System.Collections;
using System.Threading;
using System;

namespace JamFramework
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        private static AudioManager instance;
        public static AudioManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                InitializeSourcePool();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        #endregion

        public static readonly string MASTER_VOLUME_KEY = "MasterVolume";
        public static readonly string MUSIC_VOLUME_KEY = "MusicVolume";
        public static readonly string SFX_VOLUME_KEY = "SFXVolume";

        public static readonly string VOLUME_PREF_PREFIX = "JAM_FRAMEWORK_";

        // Slider values fail at 0 due to logarithmic scaling
        public static readonly float MIN_SLIDER_VALUE = 0.0001f;

        [Header("References")]
        [SerializeField] AudioLibrary musicLibrary;
        [SerializeField] AudioLibrary sfxLibrary;
        public AudioMixer audioMixer;
        public AudioMixerGroup sfxMixerGroup;
        public AudioMixerGroup musicMixerGroup;

        [Header("Audio Source Pool Config")]
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        //[SerializeField] int maxSoundInstances = 30;

        private AudioSource _mainMusicSource = null;
        private Coroutine musicFadeRoutine = null;

        public AudioLibrary MusicLibrary { get => musicLibrary; }
        public AudioLibrary SFXLibrary { get => sfxLibrary; }
        public AudioSource MainMusicSource { 
            get
            {
                if (_mainMusicSource == null) 
                {
                    _mainMusicSource = GetAudioSource();
                    _mainMusicSource.outputAudioMixerGroup = musicMixerGroup;
                    _mainMusicSource.loop = true;
                }

                return _mainMusicSource;
            } 
        }

        /*
         * AudioSource pooling
         * I like not having to return AudioSources to the pool
         * But I also like not modifying the behavior of individual AudioSources
         * I could also inherit from AudioSource and keep the base behaviors
         * How would I handle automatic returning to pools? Probably ovveride the play function?
         * But if we're automatically returning to the pool not playing, what happens when we pause
         * I think I don't want that at all. Perhaps I really just want pooled sources, no wrapper
         * Maybe extended 
         */

        private void Start()
        {
            string masterPref = VOLUME_PREF_PREFIX + MASTER_VOLUME_KEY;
            string musicPref = VOLUME_PREF_PREFIX + MUSIC_VOLUME_KEY;
            string sfxPref = VOLUME_PREF_PREFIX + SFX_VOLUME_KEY;

            // Don't use the 'defaultValue' pref parameter so we default to whatever
            // the mixer is set to in editor.

            if (PlayerPrefs.HasKey(masterPref))
            {
                audioMixer.SetFloat(MASTER_VOLUME_KEY, PlayerPrefs.GetFloat(masterPref));
            }

            if (PlayerPrefs.HasKey(musicPref))
            {
                audioMixer.SetFloat(MUSIC_VOLUME_KEY, PlayerPrefs.GetFloat(musicPref));
            }

            if (PlayerPrefs.HasKey(sfxPref))
            {
                audioMixer.SetFloat(SFX_VOLUME_KEY, PlayerPrefs.GetFloat(sfxPref));
            }
        }

        #region AudioPool

        internal IObjectPool<AudioSource> sourcePool;
        internal List<AudioSource> activeSources = new List<AudioSource>();
        internal List<AudioSource> allSources = new List<AudioSource>();

        public AudioSource GetAudioSource() => sourcePool.Get();
        public void ReturnAudioSource(AudioSource source) => sourcePool.Release(source);


        AudioSource CreateAudioSource()
        {
            var newGo = new GameObject();
            newGo.transform.parent = this.transform;
            var ret = newGo.AddComponent<AudioSource>();
            ret.playOnAwake = false;
            newGo.SetActive(false);
            allSources.Add(ret);
            return ret;
        }

        void OnTakeFromPool(AudioSource source)
        {
            source.gameObject.SetActive(true);
            activeSources.Add(source);
        }

        void OnReturnedFromPool(AudioSource source)
        {
            source.Stop();
            source.gameObject.SetActive(false);
            activeSources.Remove(source);
        }

        void OnDestroyPoolObject(AudioSource source)
        {
            allSources.Remove(source);
            Destroy(source.gameObject);
        }

        private void InitializeSourcePool()
        {
            sourcePool = new ObjectPool<AudioSource>(CreateAudioSource,
                OnTakeFromPool,
                OnReturnedFromPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize);
        }
        #endregion

        #region Music

        public void PlayMusicImmidiate(string songName, bool playFromCurrentPlaybackPosition)
        {
            if (!MusicLibrary.Find(songName, out Sound sound)) return;

            if (MainMusicSource.clip == sound.clip && MainMusicSource.isPlaying) return;

            if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);
            
            MainMusicSource.clip = sound.clip;
            MainMusicSource.volume = sound.volume;
            if (!playFromCurrentPlaybackPosition)
            {
                MainMusicSource.time = 0;
            }

            MainMusicSource.Play();
        }

        public Coroutine FadeMusicOut(float duration)
        {
            if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);

            musicFadeRoutine = StartCoroutine(DoFadeMusicOut(duration));
            return musicFadeRoutine;
        }

        private IEnumerator DoFadeMusicOut(float duration)
        {
            float startVolume = MainMusicSource.volume;
            float timer = 0;

            while (timer < duration)
            {
                MainMusicSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            MainMusicSource.volume = 0;
            MainMusicSource.Pause();

            musicFadeRoutine = null;
        }

        public Coroutine FadeMusicIn(string songName, float duration, bool playFromCurrentPlaybackPosition)
        {
            if (!MusicLibrary.Find(songName, out Sound sound)) return null;

            if (MainMusicSource.clip == sound.clip && MainMusicSource.isPlaying) return null;

            if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);

            musicFadeRoutine = StartCoroutine(DoFadeMusicIn(sound, duration, playFromCurrentPlaybackPosition));

            return musicFadeRoutine;
        }

        private IEnumerator DoFadeMusicIn(Sound newMusic, float duration, bool playFromCurrentPlaybackPosition)
        {
            MainMusicSource.volume = 0;

            MainMusicSource.clip = newMusic.clip;
            if (!playFromCurrentPlaybackPosition) MainMusicSource.time = 0;

            MainMusicSource.Play();
            float timer = 0;

            while (timer < duration)
            {
                MainMusicSource.volume = Mathf.Lerp(0, newMusic.volume, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            MainMusicSource.volume = newMusic.volume;

            musicFadeRoutine = null;
        }

        public void FadeMusicSimultaneous(string songName, float duration, bool playFromCurrentPlaybackPosition = false)
        {
            if (!MusicLibrary.Find(songName, out Sound sound)) return;

            if (MainMusicSource.clip == sound.clip && MainMusicSource.isPlaying) return;

            if (musicFadeRoutine != null) StopCoroutine(musicFadeRoutine);

            musicFadeRoutine = StartCoroutine(DoFadeMusicSimultaneous(sound, duration, playFromCurrentPlaybackPosition));

        }

        private IEnumerator DoFadeMusicSimultaneous(Sound newMusic, float transitionTime, bool playFromCurrentPlaybackPosition)
        {
            AudioSource risingTrack = GetAudioSource();
            AudioSource fallingTrack = MainMusicSource;
            risingTrack.clip = newMusic.clip;
            risingTrack.volume = 0;
            risingTrack.outputAudioMixerGroup = musicMixerGroup;
            risingTrack.loop = true;

            if (playFromCurrentPlaybackPosition) risingTrack.time = fallingTrack.time;
            risingTrack.Play();

            float fallingStartVol = fallingTrack.volume;

            float timer = 0;
            while (timer < transitionTime)
            {
                fallingTrack.volume = Mathf.Lerp(fallingStartVol, 0, timer / transitionTime);
                risingTrack.volume = Mathf.Lerp(0, newMusic.volume, timer / transitionTime);
                timer += Time.deltaTime;
                yield return null;
            }

            fallingTrack.volume = 0;
            risingTrack.volume = newMusic.volume;


            _mainMusicSource = risingTrack;
            ReturnAudioSource(fallingTrack);
            musicFadeRoutine = null;
        }
        #endregion

        #region SFX

        public void PlayOneShotSFX(Enum audioEnum, float pitch = 1f)
        {
            if (!SFXLibrary.Find(audioEnum, out Sound sound))
            {
                return;
            }

            PlayOnShotSFX_Internal(sound, pitch);
        }
        public void PlayOneShotSFX(string name, float pitch = 1f)
        {
            if (!SFXLibrary.Find(name, out Sound sound))
            {
                return;
            }

            PlayOnShotSFX_Internal(sound, pitch);
        }

        private void PlayOnShotSFX_Internal(Sound sound, float pitch)
        {
            AudioSource source = GetAudioSource();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = false;
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.pitch = pitch;

            source.PlayOneShot(source.clip);
            StartCoroutine(Helpers.ReleaseWhenComplete(source));
        }

        public bool StartLoopingSFX(Enum audioEnum, out AudioSource source, float pitch = 1f)
        {
            source = null;
            if (!SFXLibrary.Find(audioEnum, out Sound sound)) return false;

            return StartLoopingSFX_Internal(sound, source, pitch);

        }

        public bool StartLoopingSFX(string name, out AudioSource source, float pitch = 1f)
        {
            source = null;
            if (!SFXLibrary.Find(name, out Sound sound)) return false;

            return StartLoopingSFX_Internal(sound, source, pitch);

        }

        public bool StartLoopingSFX_Internal(Sound sound, AudioSource source, float pitch)
        {
            source = GetAudioSource();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = true;
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.pitch = pitch;

            source.Play();

            return true;
        }

        public void EndLoopingSFX(AudioSource source, bool waitUntilLoopComplete = false)
        {
            if (waitUntilLoopComplete)
            {
                source.loop = false;
                StartCoroutine(Helpers.ReleaseWhenComplete(source));
            }
            else
            {
                source.Stop();
                ReturnAudioSource(source);
            }
        }
        #endregion

        #region Volume

        public void SetVolume(string mixerKey, float sliderValue)
        {
            float targetVolume = Mathf.Log10(sliderValue) * 20;
            audioMixer.SetFloat(mixerKey, targetVolume);
            PlayerPrefs.SetFloat(VOLUME_PREF_PREFIX + mixerKey, targetVolume);
        }

        public float GetVolume(string key)
        {
            audioMixer.GetFloat(key, out float vol);
            return Mathf.Pow(10, vol / 20);
        }
        #endregion
    }
}
