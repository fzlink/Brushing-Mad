using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public ToothBrush toothBrush;

    [Range(0f,1f)]
    public float winThresholdPercentage;
    public WinBar winBar;
    public Text levelNoText;

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject startScreen;
    public Text winPercentage;
    public Text losePercentage;

    public AudioSource collectAudioSource;
    private bool isGameStarted;
    private bool isOnFinishState;


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = -1;
        startScreen.SetActive(true);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        levelNoText.text += SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length-1];
    }


    private void Update()
    {
        if(!isOnFinishState && !isGameStarted && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            isGameStarted = true;
            startScreen.SetActive(false);
            toothBrush.SetCanStart(true);
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
        toothBrush.SetCanStart(false);
    }

    public void ShowWinScreen(int percentage)
    {
        winPercentage.text = percentage + "%";
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        losePercentage.text = "";
        loseScreen.SetActive(true);
    }

    public void ShowLoseScreen(int percentage)
    {
        losePercentage.text = percentage + "%";
        loseScreen.SetActive(true);
    }


    public void DecideWinLoseState(bool isOnFinish)
    {
        if (isOnFinish)
        {
            int percentage = Mathf.RoundToInt(winBar.GetNormalizedValue() * 100);
            if(winBar.GetNormalizedValue() >= winThresholdPercentage)
            {
                ShowWinScreen(percentage);
            }
            else
            {
                ShowLoseScreen(percentage);
            }
        }
        else
        {
            ShowLoseScreen();
        }
    }

    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
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
