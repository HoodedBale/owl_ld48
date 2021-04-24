using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public delegate void Phase();
    public Vector2 KickForce;
    public GameObject deathScreen;
    public GameObject josh;

    Phase m_start, m_update, m_end;
    bool m_reset = true;

    float m_timer;
    GameObject m_josh;

    public static GameMan gameMan
    {
        get
        {
            GameObject obj = GameObject.Find("GameMan");
            if (obj)
                return obj.GetComponent<GameMan>();
            else
                return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PhaseUpdate();
    }

    void PhaseUpdate()
    {
        if(m_reset)
        {
            m_reset = false;
            if (m_start != null) m_start();
        }
        else
        {
            if (m_update != null) m_update();
        }
    }

    public void ChangePhase(Phase start, Phase update, Phase end)
    {
        if (m_end != null) m_end();
        m_start = start;
        m_update = update;
        m_end = end;
        m_reset = true;
    }

    public void ResetPhases()
    {
        ChangePhase(PrologueStart, PrologueUpdate, PrologueEnd);
    }

    public void EndPhases()
    {
        ChangePhase(DeathStart, DeathUpdate, DeathEnd);
    }

    void PrologueStart()
    {
        m_josh = Instantiate(josh);
        m_josh.name = "Josh";
    }
    void PrologueUpdate()
    {
        ChangePhase(KickStart, KickUpdate, KickEnd);
    }
    void PrologueEnd()
    {
        m_timer = 1.0f;
    }

    void KickStart()
    {
        //m_josh.GetComponent<Rigidbody2D>().AddForce(KickForce);
        m_josh.GetComponent<MovementController>().AddForce(KickForce);
    }
    void KickUpdate()
    {
        m_timer -= Time.deltaTime;
        if(m_timer <= 0)
        {
            ChangePhase(GameStart, GameUpdate, GameEnd);
        }
    }
    void KickEnd()
    {

    }

    void GameStart()
    {
        m_josh.GetComponent<MovementController>().enabled = true;
        Debug.Log("Start");
    }
    void GameUpdate()
    {

    }
    void GameEnd()
    {

    }

    void DeathStart()
    {
        m_timer = 2.0f;
    }
    void DeathUpdate()
    {
        if (m_timer > 0) m_timer -= Time.deltaTime;
        else
        {
            deathScreen.SetActive(true);
        }
    }
    void DeathEnd()
    {

    }
}
