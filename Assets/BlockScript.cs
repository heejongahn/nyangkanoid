using System;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public Rigidbody2D rb;

    private float fallVelocity = 1;

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
}
