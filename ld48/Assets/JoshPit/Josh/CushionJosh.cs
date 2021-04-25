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

            if(collision.collider.GetComponent<MovementController>().state == MovementController.State.I)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                gameObject.AddComponent<Rigidbody2D>();
                GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1000, 1000), 1500, 0));
                StartCoroutine("Die");
            }
        }
    }

    public void Kill()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1000, 1000), 1500, 0));
        StartCoroutine("Die");
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

    IEnumerator Die()
    {
        float timer = 1;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.eulerAngles += new Vector3(0, 0, 1) * 1000 * Time.deltaTime;
            yield return null;
        }

        SpriteRenderer sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        while(sr.color.a > 0)
        {
            sr.color -= new Color(0, 0, 0, 1) * 10 * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, 0, 1) * 1000 * Time.deltaTime;
            yield return null;

        }

        Destroy(gameObject);
    }
}
