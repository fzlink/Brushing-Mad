using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TongueRushManager : MonoBehaviour
{
    public static TongueRushManager instance;

    public WinBar winBar;
    public ToothBrushTongueRush toothBrush;

    
    [Range(1,10)]
    public float timeLimit;
    public GameObject winScreen;
    public GameObject startScreen;
    public AudioSource collectAudioSource;

    public Text timerText;
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
        timerText.text = timeLimit.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnFinishState && !isGameStarted && (Input.touchCount > 0 || Input.GetMouseButtonDown(0)))
        {
            isGameStarted = true;
            startScreen.SetActive(false);
            toothBrush.SetCanStart(true);
        }
        if (!isOnFinishState && isGameStarted)
        {
            timeLimit -= Time.deltaTime;
            timerText.text = timeLimit.ToString("F2");
            if(timeLimit <= 0)
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

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void StopGame()
    {
        isOnFinishState = true;
        toothBrush.SetCanStart(false);
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
