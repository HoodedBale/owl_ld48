using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    public Text message;
    public ResetFade resetFade;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TabulateScore");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TabulateScore()
    {
        string timeScore = "Time:\t\t";
        string bodiesScore = "Bodies:\t";
        string bonusScore = "Bonus:\t\t";
        string totalScore = "Total:\t\t";

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);

        yield return new WaitForSeconds(1.0f);

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);

        yield return new WaitForSeconds(1.0f);

        int minute = (int)(GameStats.timeCompleted / 60.0f) + 1;
        int score = GameStats.timeScores.ContainsKey(minute) ? GameStats.timeScores[minute] : GameStats.timeScores[-1];
        GameStats.totalScore += score;
        timeScore += score.ToString();

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);

        yield return new WaitForSeconds(1.0f);

        score = GameStats.bodyPenalty * GameStats.deadBodies;
        bodiesScore += score;
        GameStats.totalScore += score;

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);

        yield return new WaitForSeconds(1.0f);

        bonusScore += GameStats.bonusPoints;
        GameStats.totalScore += GameStats.bonusPoints;

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);

        yield return new WaitForSeconds(1.0f);

        totalScore += GameStats.totalScore;

        message.text = BuildMessage(timeScore, bodiesScore, bonusScore, totalScore);
    }

    string BuildMessage(string time, string bodies, string bonus, string total)
    {
        string message = time + '\n';
        message += bodies + '\n';
        message += bonus + "\n\n";
        message += total;
        return message;
    }

    public void Restart()
    {
        StartCoroutine(RestartFade());
    }

    IEnumerator RestartFade()
    {
        resetFade.StartFade();

        while(!resetFade.reset)
        {
            yield return null;
        }

        SceneManager.LoadScene("Game");
    }
}
