﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlossRushManager : MonoBehaviour
{
    public static FlossRushManager instance;

    public Floss floss;
    public FoodDropper foodDropper;

    private bool isOnFinishState;
    private bool isGameStarted;

    public GameObject winScreen;
    public GameObject startScreen;

    public WinBar winBar;

    public AudioSource collectAudioSource;

    [Range(1, 25)]
    public float timeLimit;
    public Text timerText;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Application.targetFrameRate = -1;
        startScreen.SetActive(true);
        timerText.text = timeLimit.ToString("F2");
    }

    public bool GetIsGameStarted()
    {
        return isGameStarted;
    }

    void Update()
    {
        if (!isOnFinishState && !isGameStarted && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            isGameStarted = true;
            startScreen.SetActive(false);
            floss.SetCanStart(true);
            foodDropper.StartDropping();
            winBar.SetMaxBar(Mathf.RoundToInt(timeLimit));
        }
        if (!isOnFinishState && isGameStarted)
        {
            timeLimit -= Time.deltaTime;
            timerText.text = timeLimit.ToString("F2");
            if (timeLimit <= 0)
            {
                timerText.text = "0:00";
                StopGame();
                ShowWinScreen();
            }
        }
    }

    public void IncreasePoint()
    {
        winBar.IncrementBar();
        collectAudioSource.Play();
        collectAudioSource.pitch += 0.0015f;
    }

    public void StopGame()
    {
        isOnFinishState = true;
        floss.SetCanStart(false);
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            RestartLevel();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
