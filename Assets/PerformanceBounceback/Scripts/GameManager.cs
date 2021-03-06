﻿using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Self;
    private void Awake()
    {
        if (Self == null)
        {
            Self = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [SerializeField] private Score[] scoreboards;

    [SerializeField] private int score;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            foreach (Score s in scoreboards)
                if (s != null)
                    s.UpdateScore(score);
            CheckScoreAndUpdateGoalAndTimer();
            if (audioSource != null)
                audioSource.Play();
        }
    }

    /*
	 *  ====================== Optional Part ======================
	 *  1_) Add a timer and a target number of points for players. For example, can players get 20 points in 1 minute?
	 *  2_) Create a button that resets the score.
	 *  3_) Add a "score multiplier" that floats in the scene somewhere. When hit with a ball, it disappears all points scored within a short time are multiplied.
	 * */

    #region (1)
    public static bool isGameRunning;

    private int timerInSec = 60;
    private int goalPerTimer = 20;

    private AudioSource audioSource;
    private WaitForSeconds delay1Sec = new WaitForSeconds(1f);

    private void Start()
    {
        isGameRunning = true;
        StartCoroutine(updateTimer());
        audioSource = GetComponent<AudioSource>();
        foreach (Score s in scoreboards)
            s.UpdateGoal(goalPerTimer);
    }

    private bool CheckScoreAndUpdateGoalAndTimer()
    {
        if (score < goalPerTimer) return false;
        timerInSec += 60;
        goalPerTimer += (int)(1.25f * goalPerTimer);
        foreach (Score s in scoreboards)
        {
            s.UpdateGoal(goalPerTimer);
            s.UpdateTimer(timerInSec);
        }
        timerInSecChanged = true;
        return true;
    }

    private bool timerInSecChanged;
    IEnumerator updateTimer()
    {
        int currentTime = timerInSec;
        while (currentTime > 0)
        {
            if (timerInSecChanged)
            {
                timerInSecChanged = false;
                currentTime = timerInSec;
            }
            currentTime--;
            foreach (Score s in scoreboards)
            {
                s.UpdateTimer(currentTime);
            }
            yield return delay1Sec;
        }
        if (!CheckScoreAndUpdateGoalAndTimer())
            GameOver();
    }

    private void GameOver()
    {
        isGameRunning = false;
    }
    #endregion

    #region (2)
    public void GameReset()
    {
        score = 0;
        timerInSec = 60;
        goalPerTimer = 20;
    }
    #endregion

    #region (3)

    #endregion





}
