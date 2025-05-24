using System;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public Rigidbody2D rb;

    private float fallVelocity = 0.5f;

    private bool isGameStarted = false;

    void Start()
    {
        GameEventsScript.Instance.OnChangeIsGameStarted.AddListener(HandleGameStart);
    }

    void OnDestroy()
    {
        GameEventsScript.Instance.OnChangeIsGameStarted.RemoveListener(HandleGameStart);
    }

    public void HandleGameStart(bool _isGameStarted)
    {
        isGameStarted = _isGameStarted;
        rb.linearVelocity = isGameStarted ? new Vector2(0, -fallVelocity) : Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("BlockScript: OnCollisionEnter2D with " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "Player":
                {
                    GameEventsScript.Instance.OnHealthDown?.Invoke();
                    break;
                }
            default:
                break;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("BlockScript: OnTriggerEnter2D with " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "Finish":
                {
                    GameEventsScript.Instance.OnHealthDown?.Invoke();
                    break;
                }
            default:
                break;
        }
    }
}
