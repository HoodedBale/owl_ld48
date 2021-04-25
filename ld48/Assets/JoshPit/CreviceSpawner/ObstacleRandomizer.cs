using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount - 1; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(Random.Range(0, transform.childCount - 1)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
