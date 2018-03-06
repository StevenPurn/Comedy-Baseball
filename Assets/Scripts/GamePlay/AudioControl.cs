using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {

    public static AudioControl instance;
    public List<AudioMapping> audioMap = new List<AudioMapping>();
    public AudioSource src;

	// Use this for initialization
	void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        src = GetComponent<AudioSource>();
    }

    public void PlayAudio(string eventName)
    {
        foreach (var item in audioMap)
        {
            if(item.audioEvent == eventName)
            {
                src.clip = item.sfx;
                src.Play();
            }
        }
    }
}

[Serializable]
public struct AudioMapping
{
    public string audioEvent;
    public AudioClip sfx;
}
