using UnityEngine;
using UnityEngine.Events;

public class GameEventsScript : MonoBehaviour
{
    public static GameEventsScript Instance;

    public UnityEvent<bool> OnChangeIsGameStarted;
    public UnityEvent OnScore;
    public UnityEvent OnHealthDown;
    public UnityEvent OnGameOver;
    public UnityEvent OnWinLevel;

    public UnityEvent<string> OnBoostPlayer;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
