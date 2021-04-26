using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoodleColor : MonoBehaviour
{
    public List<Color> colors;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
        float h, s, v;
        Color.RGBToHSV(GetComponent<SpriteRenderer>().color, out h, out s, out v);
        Debug.Log(h);
        h = Random.Range(0.0f, 255f);

        GetComponent<SpriteRenderer>().color = Color.HSVToRGB(h, s, v);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
