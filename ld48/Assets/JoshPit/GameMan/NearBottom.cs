using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NearBottom : MonoBehaviour
{
    public List<Color> colors;
    public float delay;
    public float speed;
    int m_currentColor = 0;
    float m_timer = 0;
    Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        m_text = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            m_text.color += (colors[m_currentColor] - m_text.color) * speed * Time.deltaTime;
        }
        else
        {
            ++m_currentColor;
            m_currentColor %= colors.Count;
            m_timer = delay;
        }
    }
}
