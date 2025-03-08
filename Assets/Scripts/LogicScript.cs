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
    public int playerScore = 0;
    public int health = 3;
    public float pitchModifier = 1;

    public AudioSource scoreAudioSource;
    public AudioSource collideAudioSource;
    public AudioSource gameOverAudioSource;

    public UnityEvent OnGameStart;

    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public GameObject pausedScreen;

    private float timeScale = 1;
    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isGameStarted = false;

    void updateText()
    {
        scoreText.text = $"Score : {playerScore} (Life: {health})";
    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        if (health == 0)
        {
            return;
        }

        playerScore += scoreToAdd;
        // scoreAudioSource.Play();
        updateText();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool collide()
    {
        health = health - 1;
        isGameOver = health == 0;
        collideAudioSource.Play();

        updateText();

        if (isGameOver)
        {
            // gameOverAudioSource.Play();
            gameOverScreen.SetActive(true);
        }

        return isGameOver;
    }

    public void toggleGamePaused()
    {
        if (isGameOver)
        {
            return;
        }

        Time.timeScale = isPaused ? timeScale : 0;
        pausedScreen.SetActive(!isPaused);
        isPaused = !isPaused;
    }

    void Update()
    {
        // if (!isPaused)
        // {
        //     timeScale += Time.deltaTime / 50;
        //     Time.timeScale = timeScale;
        // }

        // pitchModifier = (1 + (timeScale - 1) / 2) + 0.57F;

        // scoreAudioSource.pitch = pitchModifier;
        // collideAudioSource.pitch = pitchModifier;


        var logics = new List<(KeyCode, Action)>{
            (
                KeyCode.R,
                () => {
                    restartGame();
                }
            ),
            (
                KeyCode.Escape,
                () => {
                    toggleGamePaused();
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

                    GameEventsScript.Instance.OnGameStart?.Invoke();
                    isGameStarted = true;
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

    public bool IsGameStarted()
    {
        return isGameStarted;
    }
}
