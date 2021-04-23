using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSpriteAnimator : MonoBehaviour
{
    public OSpriteAnimationProfile_SO animProfile;
    public Vector2 pivot;
    public float speed = 1;

    Dictionary<string, List<Sprite>> m_spriteLibrary;
    Dictionary<string, List<int>> m_animationFrames;
    Dictionary<string, OSpriteAnimation> m_animationLibrary;

    // Real time Animation
    float m_currentFrame;
    string m_currentAnimation;
    bool m_animate = false;
    Sprite m_currentSprite;

    // Start is called before the first frame update
    void Start()
    {

        OSpriteAnimationHelper.LoadAnimationProfile(animProfile, out m_spriteLibrary, out m_animationFrames, pivot);

        m_animationLibrary = new Dictionary<string, OSpriteAnimation>();
        foreach(var anim in animProfile.animations)
        {
            SetAnimation(anim.name);
            m_animationLibrary.Add(anim.name, anim);
        }

        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    public bool isPlaying
    {
        get
        {
            return m_animate;
        }
        set
        {
            m_animate = value;
        }
    }

    public void SetAnimation(string animation, bool reset = true, float _speed = 1)
    {
        m_currentAnimation = animation;
        speed = _speed;

        if(reset)
        {
            m_currentFrame = m_animationFrames[m_currentAnimation][0];
        }
    }

    void Animate()
    {
        if (!m_animate) m_animate = false;
        if (!m_animationLibrary.ContainsKey(m_currentAnimation)) return;

        m_currentFrame += RealTimer.DeltaTime(gameObject) * m_animationLibrary[m_currentAnimation].speed * speed;
        if (m_currentFrame >= m_animationFrames[m_currentAnimation].Count) m_currentFrame = 0;
        if (m_currentFrame < 0) m_currentFrame = m_animationFrames[m_currentAnimation].Count - 1;

        m_currentSprite = m_spriteLibrary[m_currentAnimation][m_animationFrames[m_currentAnimation][(int)m_currentFrame]];

        if (GetComponent<SpriteRenderer>()) GetComponent<SpriteRenderer>().sprite = m_currentSprite;
        else if (GetComponent<Image>()) GetComponent<Image>().sprite = m_currentSprite;
    }
}
