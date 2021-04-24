using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Physics")]
    public float speed = 10;
    public Vector2 moveForce;
    public float cliffKnockForceX = 40;
    public float cliffKnockForceY = 400;
    public float fallCap = 20;
    public bool inControl = true;

    [Header("Sprites")]
    public GameObject impaledJosh;

    Rigidbody2D m_rb;
    float m_recoverTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inControl)
            Movement();
        Recover();
    }

    void Movement()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            m_rb.AddForce(-moveForce);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            m_rb.AddForce(moveForce);
        }

        Vector2 vel = m_rb.velocity;
        if (vel.y < fallCap) vel.y = fallCap;
        m_rb.velocity = vel;
    }

    void Recover()
    {
        if(m_recoverTimer > 0)
        {
            m_recoverTimer -= Time.deltaTime;
        }
        else
        {
            inControl = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if (col.tag == "Cliff")
        {
            inControl = false;
            m_recoverTimer = 2.0f;

            if (col.transform.position.x > transform.position.x)
            {
                m_rb.AddForce(new Vector2(-cliffKnockForceX, cliffKnockForceY));
            }
            else
            {
                m_rb.AddForce(new Vector2(cliffKnockForceX, cliffKnockForceY));
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {

            GameMan.gameMan.EndPhases();
            GameObject impaled = Instantiate(impaledJosh);
            impaled.transform.position = transform.position;
            gameObject.SetActive(false);
        }
    }

}
