using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusIcon : MonoBehaviour
{
    public float damp = 0.1f;
    public float speed = 4;

    // Start is called before the first frame update
    void Start()
    {
        
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
        float timer = 5;
        Vector3 scale = transform.localScale + new Vector3(-1, 1, 0);
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

        transform.localScale = target;
    }
}
