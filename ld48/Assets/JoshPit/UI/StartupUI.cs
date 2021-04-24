using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupUI : MonoBehaviour
{
    public float fadeSpeed = 10;
    Image m_fade;
    bool m_triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        m_fade = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CreviceSpawner.generationProgress >= 1.0 && !m_triggered)
        {
            m_triggered = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        while(m_fade.color.a > 0)
        {
            m_fade.color -= new Color(0, 0, 0, 1) * fadeSpeed * Time.deltaTime;
            yield return null;
        }
        GameMan.gameMan.ResetPhases();
        Destroy(gameObject);
    }
}
