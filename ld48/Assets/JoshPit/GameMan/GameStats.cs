using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    public static int deadBodies;
    public static float timeCompleted;
    public static int bonusPoints;
    public static int totalScore;
    public static int highScore = 0;
    public static float bestTime = Mathf.Infinity;

    public static Dictionary<int, int> timeScores;
    public static int bodyPenalty = -50;

    public static void Reset()
    {
        deadBodies = 0;
        bonusPoints = 0;
        timeCompleted = 0;
        totalScore = 0;

        timeScores = new Dictionary<int, int>();
        timeScores.Add(1, 5000);
        timeScores.Add(2, 4500);
        timeScores.Add(3, 4000);
        timeScores.Add(7, 2500);
        timeScores.Add(15, 2000);
        timeScores.Add(-1, 1000);
    }
}
