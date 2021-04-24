using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(GameStats.timeCompleted);
        string message = string.Format("{0:D2}:{1:D2}:{2:D1}", time.Minutes, time.Seconds, time.Milliseconds);

        GetComponent<Text>().text = message;
        transform.GetChild(0).GetComponent<Text>().text = message;
    }
}
