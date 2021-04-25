using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = GameStats.highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
