using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.Events;

public class LogicScript : MonoBehaviour
{
    public float playerScore = 0;
    public int health = 3;

    public AudioSource scoreAudioSource;
    public AudioSource collideAudioSource;
    public AudioSource gameOverAudioSource;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timeScaleText;


    public GameObject gameOverScreen;
    public GameObject pausedScreen;
    public GameObject victoryScreen;

    private float timeScale = 1;
    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isGameStarted = false;

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    void Start()
    {
        GameEventsScript.Instance.OnHealthDown.AddListener(OnHealthDown);
        GameEventsScript.Instance.OnWinLevel.AddListener(OnVictory);
        GameEventsScript.Instance.OnScore.AddListener(HandleScore);
    }

    void OnDestroy()
    {
        GameEventsScript.Instance.OnHealthDown.RemoveListener(OnHealthDown);
        GameEventsScript.Instance.OnWinLevel.RemoveListener(OnVictory);
        GameEventsScript.Instance.OnScore.RemoveListener(HandleScore);
    }

    void IncreaseTimeScaleIfAvailable()
    {
        if (!isGameStarted)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        timeScale += Time.deltaTime / 100;
        Time.timeScale = timeScale;
        timeScaleText.text = $"TimeScale: {timeScale:F2}";
    }

    void Update()
    {
        IncreaseTimeScaleIfAvailable();

        var logics = new List<(KeyCode, Action)>{
            (
                KeyCode.R,
                () => {
                    RestartGame();
                }
            ),
            (
                KeyCode.Escape,
                () => {
                    ToggleGamePaused();
                }
            ),
            (
                KeyCode.Space,
                () => {
                    if (isGameStarted) {
                        return;
                    }

                    if (GameEventsScript.Instance == null) {
                        Debug.LogError("GameEvents instance is null. Ensure GameEvents is properly initialized.");
                        return;
                    }

                    ToggleIsGameStarted();
                }
            )
        };

        foreach (var logic in logics)
        {
            (var keyCode, var func) = logic;
            if (Input.GetKeyDown(keyCode))
            {
                func();
            }
        }
    }


    void HandleScore()
    {
        playerScore += Time.timeScale;
        scoreText.text = $"Score : {playerScore:F2}";
    }

    void UpdateHealthText()
    {
        healthText.text = $"Health: {health}";
    }

    // [ContextMenu("Increase Score")]
    // public void addScore(int scoreToAdd)
    // {
    //     if (health == 0)
    //     {
    //         return;
    //     }

    //     playerScore += scoreToAdd;
    //     scoreAudioSource.Play();
    //     UpdateScoreText();
    // }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnHealthDown()
    {
        health = health - 1;
        isGameOver = health == 0;
        ToggleIsGameStarted();
        // collideAudioSource.Play();
        UpdateHealthText();

        if (isGameOver)
        {
            GameOver();
        }
    }

    void OnVictory()
    {
        victoryScreen.SetActive(true);
        ToggleIsGameStarted();
    }

    void GameOver()
    {
        // gameOverAudioSource.Play();
        ToggleIsGameStarted();
        gameOverScreen.SetActive(true);
        GameEventsScript.Instance.OnGameOver?.Invoke();
    }

    public void ToggleIsGameStarted()
    {
        if (isGameOver)
        {
            return;
        }

        isGameStarted = !isGameStarted;
        GameEventsScript.Instance.OnChangeIsGameStarted.Invoke(isGameStarted);
    }

    public void ToggleGamePaused()
    {
        if (isGameOver)
        {
            return;
        }

        Time.timeScale = isPaused ? timeScale : 0;
        pausedScreen.SetActive(!isPaused);
        isPaused = !isPaused;
    }


}
