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

    public int remaingingBoost = 3;

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
    private float boostTimer = 0;

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

        timeScale += Time.deltaTime / 300;
        Time.timeScale = timeScale;
        timeScaleText.text = $"{timeScale:F2}";
    }

    void RestoreBoost()
    {
        if (!isGameStarted)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        boostTimer += Time.deltaTime;
        if (boostTimer > 5)
        {
            remaingingBoost = Math.Min(remaingingBoost + 1, 3);
            boostTimer = 0;
            return;
        }
    }

    void Update()
    {
        IncreaseTimeScaleIfAvailable();
        RestoreBoost();

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
            ),
            (
                KeyCode.Z,
                () => {
                    BoostPlayer("left");
                }
            ),
            (
                KeyCode.C,
                () => {
                    BoostPlayer("right");
                }
            ),
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
        scoreText.text = $"{playerScore:F2}";
    }

    void UpdateHealthText()
    {
        healthText.text = $"{health}";
    }

    void BoostPlayer(string direction)
    {
        if (remaingingBoost == 0)
        {
            return;
        }

        remaingingBoost -= 1;
        GameEventsScript.Instance.OnBoostPlayer?.Invoke(direction);
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
