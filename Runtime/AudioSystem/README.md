# Audio System
The Jam Framework Audio System is the preferred way of broadcasting Music and SFX to the player.

It handles the collection, configuration, management, and playing of audio events.

This guide will break down how to begin using the Audio System and the features within it.

## Creating Audio Libraries
- The first thing you want to do when setting up your project's audio is create Audio Libraries for Music and SFX
- If you've chosen to import essential samples, two will be included by default with minimal configuration
- Additional Libraries can be created by right-clicking the project window and selecting Create > Scriptable Objects > Jam Framework > Audio Library
- In the object's Inspector give the Library a simple name indicative of its purpose (like Music, SFX, or Ambients)
- Next, populate the "Fallback Sound". If there's ever an error when attempting to play a sound from this library (like if the programmer tries to play a sound that doesn't exist) this sound will play instead.
- Now all that's left to do is add as many Sounds as your heart desires :)
### Sounds

- A Sound is a struct that contains the following fields:
	- "Name" : A name unique to this sound within the library. Note that there are certain restrictions to what characters can be included in a Sound Name
	- "Clip" : The audio clip played by this Sound
	- "Volume" : The base volume of this Sound and will be used by the AudioSource whenever this Sound is played
- TODO : To help configure the volume, you can "Play" sounds
- Anytime you make a change to the number or name of available Sounds, click the "Run Codgen" button.
- This will create/edit a file called AudioLibrary.Codegen.cs. Please make sure to push any changes to that file if you see it in github.
## Audio Codegen & Enums
The AudioLibrary Codgen process creates/edits a file (Assets > Codegen > AudioLibrary.Codegen.cs) with unqiue enums for every sound in every library. Please refrain from editing the contents of that file directly.

- In the Global Namespace, an enum is created called "AudioEnums" that can be accessed from any script
- Each enum in the library is named as follows: E[LIBRARY_NAME]_[SOUND_NAME]
- The enum value is generated from an MD5 hash of it's name, and is useless outside of its uniqueness
- AudioEnums can be used in calls to the AudioManager to play sounds. So when codegen is up to date, you won't need to remember/provide the same of you sounds as strings when writing code

## Audio Manager & Audio Sources
- Whever possible, we should avoid using Audio Source components without going throught the Audio Manager Singleton. This is largely to ensure all audio is properly hooked up to the correct Mixers and interact consistently with Audio Libraries.
- The Audio Manager manages a pool of Audio Sources and can distribute them on request. Object pool settings can be adjusted on the Audio Manager component instance.
- By default, one Audio Source is allocated for the currently playing Music
- There are functions for one-shot SFX, looping SFX, and playing music with multiple styles of song transition
For example, a simple call to play SFX may look like this:
```
AudioManager.Instance.PlayOneShotSFX(AudioEnums.ESFX_dummy_clip);
```
And Music Like this:
```
AudioManager.Instance.FadeMusicIn("just put the fallback in the bag", 3f, false);
```
### Setup
Note: If you're using the Launchpad template, and Audio Manager will be present in the Launchpad and may be partially configured.
- Create an AudioManager wherever you keep your Singletons
- Assign references to all Objects in the "References" category in the component's inspector
- Configure the Audio Source Pool Settings if desired. The deafults should be fine.
### API Reference
Note: All functions that play sounds from an AudioLibrary will attempt to use the Fallback Sound if the user-provided sound cannot be found.
`public AudioSource GetAudioSource()`  
Fetches and AudioSource from the manager's pool. You generally won't need to draw from the pool directly very often.

`Public AudioSource ReturnAudioSource()`  
Returns an AudioSource for reuse in the pool.

`public AudioSource MainMusicSource()`  
Returns the special AudioSource designated as the main music player. Property auto-initializes when first referenced.

`public void PlayMusicImmidiate(string songName, bool playFromCurrentPlaybackPosition)`  
Plays a provided song from the configured Music Library without any transition or fanfare. Uses the MainMusicSource

`public Coroutine FadeMusicOut(float duration)`  
Interpolates the MainMusicSource's volume from current to 0 over `duration` seconds and pauses the clip when muted.

`public Coroutine FadeMusicIn(string songName, float duration, bool playFromCurrentPlaybackPosition)`  
Selectes a Sound from the MusicLibrary using `songName`, sets the MainMusicSource's clip, and interpolates the MainMusicSource's volume from 0 to that song's volume over `duration`

`public void FadeMusicSimultaneous(string songName, float duration, bool playFromCurrentPlaybackPosition = false)`  
Selectes a new song on a temporary AudioSource using `songName` and fades that song in while fading the other song out. When completed, the new AudioSource becomes the MainMusicSource
This creates a seamless transition between tracks and works well for alternative music tracks that add/remove instruments from a common base song.


`public void PlayOneShotSFX(Enum audioEnum, float pitch = 1f)`  
Fetches a tempoarary AudioSource and uses it to play a Sound from the SFXLibrary matching the provided `audioEnum` value. Tries to use the fallback if not found.

`public void PlayOneShotSFX(string name, float pitch = 1f)`  
Fetches a tempoarary AudioSource and uses it to play a Sound from the SFXLibrary matching the provided `name`. Tries to use the fallback if not found.

`public bool StartLoopingSFX(string name, out AudioSource source, float pitch = 1f)`  
Plays a looping Sound from the SFXLibrary determined by `name`. The SFX `source` provided by this call is designed to be ended with a call to EndLoopingSFX(). 

`public void EndLoopingSFX(AudioSource source, bool waitUntilLoopComplete = false)`  
Stops a looping Sound provided by StartLoopingSFX(). If `waitUntilLoopComplete` is true, the sfx will stop after completing its current iteration.

`public void SetVolume(string mixerKey, float sliderValue)`  
On the main AudioMixer, sets the Volume of a given `mixerKey` parameter to the provided 0-1 `sliderValue`. Use this on your Settings Menu audio sliders!

`public void SetVolume(string mixerKey, float sliderValue)`  
Using the proivded `mixerKey` parameter, fetch a Float value from the main AudioMixer and return it as a simple 0-1 value that can be represented on a slider.

## Programming Tips
- If there's a point in the gameplay where a sound *should* play, but the sound isn't implemented yet, put a call to PlaySound anyway. This way, we can hear the fallback sound during tests and know what needs to be added.
- Implement Sounds Early: Even if Foley hasn't decided what sounds play and when, play SFX wherever it seems to make sense. We can always remove extra calls later