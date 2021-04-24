using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoshCollider : MonoBehaviour
{
    MovementController m_josh;

    // Start is called before the first frame update
    void Start()
    {
        m_josh = GameObject.Find("Josh").GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 oldPos = transform.position;
        transform.parent.position = transform.position;
        transform.position = oldPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Cliff")
            m_josh.ReactCliff(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {
            m_josh.ReactSpike(collision);
        }
    }
}
