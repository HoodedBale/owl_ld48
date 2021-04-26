using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public enum State
    {
        Z,
        X,
        C,
        I,
        IMPALED
    }

    [Header("Physics")]
    public float speed = 10;
    public Vector2 moveForce;
    public float cliffKnockForceX = 40;
    public float cliffKnockForceY = 400;
    public float fallCap = 20;
    public float fallCcap = 10;
    public float fallZcap = 40;
    public bool inControl = true;
    public float springDamp = 0.2f;
    public float springSpeed = 16;

    [Header("Sprites")]
    public GameObject impaledJosh;
    public GameObject facePlantJosh;

    [Header("States")]
    public State state;
    public List<GameObject> poses;
    public List<GameObject> poseVFX;

    Rigidbody2D m_rb;
    float m_recoverTimer;
    float m_fallCap;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_fallCap = fallCap;
        SetState(State.C);
        //ResetRB();
    }

    // Update is called once per frame
    void Update()
    {
        if(inControl)
        {
            StateControl();
            Movement();
        }
        Recover();
    }

    void Movement()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            m_rb.AddForce(-moveForce);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            m_rb.AddForce(moveForce);
            transform.localScale = new Vector3(1, 1, 1);
        }

        Vector2 vel = m_rb.velocity;
        if (vel.y < m_fallCap) vel.y = m_fallCap;
        if(state == State.IMPALED)
        {
            vel = Vector2.zero;
        }    
        m_rb.velocity = vel;
    }

    void StateControl()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            SetState(State.Z);
        else if (Input.GetKeyDown(KeyCode.X))
            SetState(State.X);
        else if (Input.GetKeyDown(KeyCode.C))
            SetState(State.C);
        else if (Input.GetKeyDown(KeyCode.Space))
            SetState(State.I);
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

    public void AddForce(Vector2 force)
    {
        if (!m_rb) m_rb = GetComponent<Rigidbody2D>();
        m_rb.AddForce(force);
    }

    public void SetState(State _state)
    {
        state = _state;
        foreach(GameObject pose in poses)
        {
            pose.SetActive(false);
        }
        m_fallCap = fallCap;
        poses[(int)state].SetActive(true);
        if(gameObject.activeSelf)
            StartCoroutine(ShapeParticles((int)state));
        GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
        SFXMan.sfxMan.PlayShape((int)state);
        switch(state)
        {
            case State.Z:
                break;
            case State.X:
                break;
            case State.C:
                m_fallCap = fallCcap;
                break;
            case State.I:
                m_fallCap = fallZcap;
                GetComponent<BoxCollider2D>().size = new Vector2(2, 5);
                break;
            case State.IMPALED:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if(col.tag == "Cliff" || (col.tag == "Cushion" && state != State.I))
        {
            inControl = false;
            m_recoverTimer = 1.0f;

            if (col.transform.position.x > transform.position.x)
            {
                m_rb.AddForce(new Vector2(-cliffKnockForceX, cliffKnockForceY));
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                m_rb.AddForce(new Vector2(cliffKnockForceX, cliffKnockForceY));
                transform.localScale = new Vector3(1, 1, 1);
            }
            StopCoroutine("Spring");
            StartCoroutine("Spring");
            SetState(State.C);
            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.FLINCH);
        }
        else if(col.tag == "Flat")
        {
            if (GameMan.win) return;
            if (transform.position.y < col.transform.parent.position.y) return;

            GameMan.gameMan.EndPhases();
            GameObject flat = Instantiate(facePlantJosh);
            flat.transform.position = transform.position + new Vector3(0, 0, -2);
            flat.transform.localScale = transform.localScale;
            gameObject.SetActive(false);
            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.DIE);
        }
        else if(col.tag == "Cushion")
        {
            //Destroy(col.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {
            if (transform.position.y < collision.transform.position.y) return;
            if (GameMan.win) return;

            GameMan.gameMan.EndPhases();
            GameObject impaled = Instantiate(impaledJosh);
            impaled.transform.position = transform.position + new Vector3(0, 0, -2);
            gameObject.SetActive(false);

            if (collision.transform.position.x < transform.position.x)
            {
                //impaled.transform.position -= new Vector3(2.5f, 2.5f, 0);
                impaled.transform.position = collision.transform.position + new Vector3(2.5f, 2f, 0) + new Vector3(0, 0, -2);
                impaled.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                //impaled.transform.position -= new Vector3(2.5f, 2.5f, 0);
                impaled.transform.position = collision.transform.position + new Vector3(-2.5f, 2f, 0) + new Vector3(0, 0, -2);
                impaled.transform.localScale = new Vector3(-1, 1, 1);
            }

            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.DIE);
            SetState(State.IMPALED);
        }
        else if(collision.tag == "Bonus")
        {
            if(state == State.Z)
            {
                BonusMan.bonusMan.StartBonus();
                Destroy(collision.gameObject);
            }
        }
        else if(collision.tag == "Cactus")
        {
            if (GameMan.win) return;
            if (state == State.X)
            {
                collision.GetComponent<Cactus>().Kill();
                SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.AWESOME);
                GameStats.bonusPoints += 250;
                StartCoroutine(ShapeParticles(1));
            }
            else
            {
                GameMan.gameMan.EndPhases();
                GameObject flat = Instantiate(facePlantJosh);
                flat.transform.position = transform.position;
                flat.transform.localScale = transform.localScale;
                flat.GetComponent<CushionJosh>().Kill();
                gameObject.SetActive(false);
                SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.DIE);
            }
        }
    }

    IEnumerator Spring()
    {
        Vector3 vel = new Vector3();
        Vector3 scale = new Vector3(2, 1, 0);
        Vector3 targetScale = transform.localScale;
        float timer = 1.0f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            NumericSpring.Spring(ref scale.x, ref vel.x, targetScale.x, Time.deltaTime, springDamp, springSpeed * Mathf.PI);
            NumericSpring.Spring(ref scale.y, ref vel.y, targetScale.y, Time.deltaTime, springDamp, springSpeed * Mathf.PI);
            transform.localScale = scale;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    IEnumerator ShapeParticles(int pose)
    {
        GameObject particle = Instantiate(poseVFX[pose]);
        particle.transform.position = transform.position;
        yield return new WaitForSeconds(1f);
        Destroy(particle);
    }

}
