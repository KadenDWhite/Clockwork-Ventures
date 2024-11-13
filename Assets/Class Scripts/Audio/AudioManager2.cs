using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager2 : MonoBehaviour
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
        if (_sound.source == null)
        {
            return;
        }

        _sound.source.volume = 0;
    }
}
