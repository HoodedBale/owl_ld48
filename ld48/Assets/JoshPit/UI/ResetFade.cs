using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetFade : MonoBehaviour
{
    public float fadeSpeed = 10;
    public bool reset = false;

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
        reset = false;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(m_image.color.a < 1)
        {
            m_image.color += new Color(0, 0, 0, 1) * fadeSpeed * Time.deltaTime;
            yield return null;
        }
        m_image.color = Color.black;

        reset = true;
        yield return new WaitForSeconds(0.5f);

        while (m_image.color.a > 0)
        {
            m_image.color -= new Color(0, 0, 0, 1) * fadeSpeed * Time.deltaTime;
            yield return null;
        }
        m_image.color = new Color(0, 0, 0, 0);
    }
}
