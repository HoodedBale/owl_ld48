using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricBody : MonoBehaviour
{
    public Vector3 velocity;

    float deltaTime
    {
        get
        {
            RealTimer timer = GetComponent<RealTimer>();
            if (timer) return timer.timeScale * Time.deltaTime;
            return Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTransform()
    {
        transform.position += velocity * deltaTime;
    }
}
