using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    public static MusicMan musicMan
    {
        get
        {
            GameObject obj = GameObject.Find("MusicMan");
            if (obj) return obj.GetComponent<MusicMan>();
            return null;
        }
    }

    public AudioClip startTrack, loopTrack;
    bool m_gameTrack = false;
    AudioSource m_source;

    // Start is called before the first frame update
    void Start()
    {
        m_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_gameTrack && !m_source.loop && !m_source.isPlaying)
        {
            m_source.loop = true;
            m_source.clip = loopTrack;
            m_source.Play();
        }
    }

    public void PlayGameMusic()
    {
        m_gameTrack = true;
        m_source.clip = startTrack;
        m_source.Play();
        m_source.loop = false;

    }
}
