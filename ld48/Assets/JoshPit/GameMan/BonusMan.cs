using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusMan : MonoBehaviour
{
    public static BonusMan bonusMan
    {
        get
        {
            return GameObject.Find("BonusMan").GetComponent<BonusMan>();
        }
    }

    public RealTimer_SO playerTimer;
    public List<GameObject> buttonIcons;
    public Transform iconStart;
    public GameObject bonusPanel;
    public float time;

    List<int> m_currentPattern;
    List<GameObject> m_currentIcons;
    bool m_active = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active)
        {
            GameObject josh = GameObject.Find("Josh");
            Vector2 vel = josh.GetComponent<Rigidbody2D>().velocity;
            if (vel.y < -1) vel.y = -1;
            josh.GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    public void StartBonus()
    {
        if (m_active) return;
        bonusPanel.SetActive(true);
        playerTimer.timeScale = 0.05f;
        m_active = true;
        RandomizePattern();
        StartCoroutine("WaitInput");
    }

    IEnumerator WaitInput()
    {
        bool clear = true;
        float timer = time;

        for(int i = 0; i < m_currentPattern.Count; ++i)
        {
            bool correct = false; ;
            while(!GetPose(m_currentPattern[i], ref correct))
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    clear = false;
                    break;
                }    

                yield return null;
            }
            if (timer <= 0)
            {
                clear = false;
                break;
            }

            if (!correct)
            {
                clear = false;
                m_currentIcons[i].GetComponent<Image>().color = Color.black;
                break;
            }
            else
            {
                m_currentIcons[i].GetComponent<Image>().color = Color.white;
                m_currentIcons[i].GetComponent<BonusIcon>().StartSpring();
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        if(clear)
        {
            GameStats.bonusPoints += 1000;
            SFXMan.sfxMan.PlayFeedback(SFXMan.Feedback.AWESOME);
        }

        yield return new WaitForSeconds(2.0f);

        m_active = false;
        bonusPanel.SetActive(false);

        playerTimer.timeScale = 1f;
    }

    void RandomizePattern()
    {
        m_currentPattern = new List<int>();
        for(int i = 0; i < 4; ++i)
        {
            m_currentPattern.Add(Random.Range(0, 4));
        }

        if (m_currentIcons == null) m_currentIcons = new List<GameObject>();
        foreach(var obj in m_currentIcons)
        {
            Destroy(obj);
        }
        m_currentIcons.Clear();

        for(int i = 0; i < m_currentPattern.Count; ++i)
        {
            GameObject icon = Instantiate(buttonIcons[m_currentPattern[i]]);
            icon.transform.position = iconStart.position + new Vector3(1, 0, 0) * i * 200;
            icon.transform.SetParent(bonusPanel.transform);
            m_currentIcons.Add(icon);
        }
    }

    bool GetPose(int currentPose, ref bool correct)
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            correct = currentPose == 0;
            return true;
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            correct = currentPose == 1;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            correct = currentPose == 2;
            return true;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            correct = currentPose == 3;
            return true;
        }

        return false;
    }
}
