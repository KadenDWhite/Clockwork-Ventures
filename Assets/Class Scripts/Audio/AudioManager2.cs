using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPupSystems.Audio
{
    public class AudioManager2
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        IEnumerator FadeIn(AudioInfo _info)
        {
            if (_info.source == null)
            {
                return null;
            }

            _info.source.volume = 0;
            return null;
        }
    }
}