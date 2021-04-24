using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CushionJosh : MonoBehaviour
{
    public float damp = 0.2f;
    public float speed = 16;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine("Spring");
        }
    }

    IEnumerator Spring()
    {
        Vector3 vel = new Vector3();
        Vector3 scale = new Vector3(2, 1, 0);

        while(true)
        {
            NumericSpring.Spring(ref scale.x, ref vel.x, 1, Time.deltaTime, damp, speed * Mathf.PI);
            NumericSpring.Spring(ref scale.y, ref vel.y, 1, Time.deltaTime, damp, speed * Mathf.PI);
            transform.localScale = scale;
            yield return null;
        }
    }
}
