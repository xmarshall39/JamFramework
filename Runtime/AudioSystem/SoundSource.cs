using UnityEngine;

namespace JamFramework
{
    [RequireComponent (typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {
        public AudioSource Source { get; private set; }
        public Sound SoundData { get; private set; }
    }
}
