using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSpring : MonoBehaviour
{

    public float damp = 0.2f;
    public float speed = 16;
    public float delay = 2;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("Spring");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpring()
    {
        StartCoroutine("Spring");
    }

    IEnumerator Spring()
    {
        while(true)
        {
            float timer = delay;
            Vector3 scale = new Vector3(0.5f, 2, 1);
            Vector3 vel = new Vector3();
            Vector3 target = transform.localScale;

            while(timer > 0)
            {
                NumericSpring.Spring(ref scale.x, ref vel.x, target.x, Time.deltaTime, damp, speed * Mathf.PI);
                NumericSpring.Spring(ref scale.y, ref vel.y, target.y, Time.deltaTime, damp, speed * Mathf.PI);
                transform.localScale = scale;

                timer -= Time.deltaTime;
                yield return null;
            }

            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
