using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMan : MonoBehaviour
{
    public static SFXMan sfxMan
    {
        get
        {
            return GameObject.Find("SFXMan").GetComponent<SFXMan>();
        }
    }

    public enum Feedback
    {
        AWESOME,
        FLINCH,
        WIN,
        DIE
    }

    public List<AudioClip> shapeClip;
    public List<AudioClip> feedbackClip;

    AudioSource m_source;

    // Start is called before the first frame update
    void Start()
    {
        m_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShape(int shape)
    {
        m_source.clip = shapeClip[shape];
        m_source.Play();
    }

    public void PlayFeedback(Feedback feedback)
    {
        m_source.clip = feedbackClip[(int)feedback];
        m_source.Play();
    }
}
