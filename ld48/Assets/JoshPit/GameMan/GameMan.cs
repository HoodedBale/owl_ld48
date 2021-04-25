using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public delegate void Phase();
    public Vector2 KickForce;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject josh;
    public GameObject resetFade;
    public GameObject keg;
    public GameObject cactus;
    public int kegDistance = 250;
    public float cactusDelay = 10;

    [Header("UI")]
    public GameObject warning;

    Phase m_start, m_update, m_end;
    bool m_reset = true;
    float m_timeElapsed;

    float m_timer;
    float m_cactusTimer = 0;
    GameObject m_josh;
    public static bool win = false;

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
        GameStats.Reset();
        m_timeElapsed = 0;
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

    public void Clean()
    {
        resetFade.GetComponent<ResetFade>().StartFade();
        deathScreen.SetActive(false);
        deathScreen.GetComponent<SpringAppearance>().ResetUI();
        ChangePhase(ResetStart, ResetUpdate, ResetEnd);
    }


    void PrologueStart()
    {
        m_josh = Instantiate(josh);
        m_josh.name = "Josh";

        for(int i = kegDistance; i < 750; i += kegDistance)
        {
            GameObject obj = Instantiate(keg);
            obj.transform.position = Vector3.zero - new Vector3(0, i, 0);
        }
        GameStats.bonusPoints = 0;
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
    }
    void GameUpdate()
    {
        if (!win)
        {
            m_timeElapsed += Time.deltaTime;
            GameStats.timeCompleted = m_timeElapsed;
        }
        if (m_josh.transform.position.y < CreviceSpawner.minimumY - 10)
        {
            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.WIN);
            ChangePhase(WinStart, WinUpdate, WinEnd);
        }

        if (m_josh.transform.position.x < -30 ||
            m_josh.transform.position.x > 30 ||
            m_josh.transform.position.y > 50)
        {
            m_josh.SetActive(false);
            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.DIE);
            EndPhases();
        }

        if(m_cactusTimer > cactusDelay && Random.Range(0, 2) > 0)
        {
            StartCoroutine("ShootCactus");
        }
        else
        {
            m_cactusTimer += Time.deltaTime;
        }
    }
    void GameEnd()
    {

    }

    void DeathStart()
    {
        m_timer = 2.0f;
        ++GameStats.deadBodies;
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

    void WinStart()
    {
        winScreen.SetActive(true);
        //GameStats.timeCompleted = Time.time - m_timeElapsed;
        win = true;
    }
    void WinUpdate()
    {

    }
    void WinEnd()
    {

    }

    void ResetStart()
    {

    }
    void ResetUpdate()
    {
        if(resetFade.GetComponent<ResetFade>().reset)
        {
            ResetPhases();
        }
    }
    void ResetEnd()
    {

    }

    IEnumerator ShootCactus()
    {
        m_cactusTimer = 0;
        warning.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        warning.SetActive(false);
        GameObject cac = Instantiate(cactus);
        cac.transform.position = Vector3.zero + new Vector3(0, 1, 0) * (m_josh.transform.position.y - 100);

    }
}
