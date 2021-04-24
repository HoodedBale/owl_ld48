using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetFade : MonoBehaviour
{
    public float fadeSpeed = 10;

    Image m_image;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFade(float speed = 5)
    {
        fadeSpeed = speed;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        if(m_image.color.a < 1)
        {
            m_image.color += new Color(0, 0, 0, 1) * fadeSpeed * Time.deltaTime;
            yield return null;
        }

        if(m_image.color.a > 0)
        {
            m_image.color -= new Color(0, 0, 0, 1) * fadeSpeed * Time.deltaTime;
        }
    }
}
