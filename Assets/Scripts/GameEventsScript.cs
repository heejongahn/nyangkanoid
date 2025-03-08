using UnityEngine;
using UnityEngine.Events;

public class GameEventsScript : MonoBehaviour
{
    public static GameEventsScript Instance;

    public UnityEvent OnGameStart;

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
