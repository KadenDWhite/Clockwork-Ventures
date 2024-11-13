using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPupSystems.Audio
{
    [System.Serializable]
    public class AudioInfo
    {
        public string name = "";
        public AudioClip clip;
        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        [Range(0.1f, 3.0f)]
        public float pitch = 1.0f;
        public bool loop = false;
        [HideInInspector]
        public AudioSource source;
        public AudioChannel channel;

        public enum AudioChannel
        {
            Music,
            SFX
        }

    }
}
