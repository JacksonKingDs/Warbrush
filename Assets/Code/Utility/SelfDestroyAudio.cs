using UnityEngine;
using System.Collections;

//Attach to audio prefabs
public class SelfDestroyAudio : MonoBehaviour
{
    AudioSource _audio;
    float _length;

    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        _length = _audio.clip.length;

    }

    void Update()
    {
        if (_audio.time >= _length)
        {
            Destroy(gameObject);
        }
    }
}

