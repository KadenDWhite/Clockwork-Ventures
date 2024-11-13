using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPupSystems.Audio
{
    [CreateAssetMenu(fileName = "AudioSet", menuName = "Data/AudioSet", order = 1)]
    public class AudioSet : ScriptableObject
    {
        public List<AudioInfo> sFX;
        public List<AudioInfo> music;
        public bool shuffleMusic = true;
    }
}
