using UnityEngine;
using UnityEngine.Events;

public class GameEventsScript : MonoBehaviour
{
    public static GameEventsScript Instance;

    public UnityEvent<bool> OnChangeIsGameStarted;
    public UnityEvent OnHealthDown;
    public UnityEvent OnGameOver;
    public UnityEvent OnWinLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
