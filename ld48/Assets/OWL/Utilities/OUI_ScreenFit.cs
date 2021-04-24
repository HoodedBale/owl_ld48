using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OUI_ScreenFit : MonoBehaviour
{
    public bool widthFit = true;
    public bool heightFit = true;
    RectTransform canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = widthFit ? canvas.rect.width : GetComponent<RectTransform>().rect.width;
        float height = heightFit ? canvas.rect.height : GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
