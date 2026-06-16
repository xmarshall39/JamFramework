using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JamFramework
{
    [System.Serializable]
    public class Sound
    {
        public string name = string.Empty;
        public AudioClip clip;
        [Range(0, 1)]
        public float volume = 1f;

        [SerializeField, HideInInspector]
        internal int enumKey;

        public Sound(Sound other)
        {
            this.name = other.name;
            this.clip = other.clip;
            this.volume = other.volume;
        }
#if UNITY_EDITOR
        public static Regex rgx = new Regex("[^a-zA-Z0-9_]");
        public string GetCodegenName(string containerName) 
        {
            string strippedName = rgx.Replace(name, "");
            return $"E{containerName}_{strippedName}";
        }

#endif
    }
}
