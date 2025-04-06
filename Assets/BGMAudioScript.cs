using UnityEngine;

public class BGMAudioScript : MonoBehaviour
{
    private static BGMAudioScript instance;

    void Awake()
    {
        // Singleton 패턴으로 AudioManager가 중복 생성되지 않도록 설정
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}