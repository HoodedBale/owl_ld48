using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float Josh
    {
        get
        {
            GameObject josh = GameObject.Find("Josh");
            if (josh) return josh.transform.position.y;
            else return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = transform.position;
        camPos.y = Josh;
        if (camPos.y < CreviceSpawner.minimumY) camPos.y = CreviceSpawner.minimumY;
        transform.position = camPos;
    }
}
