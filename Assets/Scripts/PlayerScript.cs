using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float padding = 1f;
    public float paddleWidth = 3.5f; // Adjust based on sprite size
    public float speed = 20;

    private Vector2 moveDirection;

    private bool isColliding = false;
    private bool isGameOver = false;

    private bool isBoosting = false;


    private float invincibleTime = 1;
    private float timeSinceLastCollision = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the game start event
        if (GameEventsScript.Instance == null)
        {
            Debug.LogError("GameEvents instance is null. Ensure GameEvents is properly initialized.");
            return;
        }

        GameEventsScript.Instance.OnBoostPlayer.AddListener((string direction) => StartCoroutine(SlowDownAndBoost(direction)));
    }

    void Update()
    {
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
        }
    }

    void FixedUpdate()
    {
        if (isBoosting)
        {
            return;
        }

        // Cancel out velocity from collision with ball
        rb.linearVelocity = Vector2.zero;

        if (moveDirection == Vector2.zero)
        {
            return;
        }

        // Move the paddle
        Vector2 newPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        // Apply movement

        rb.MovePosition(newPosition);
    }

    IEnumerator SlowDownAndBoost(string direction)
    {
        Debug.Log("SlowDownAndBoost called");

        Vector2 directionVector = direction == "left" ? Vector2.left : Vector2.right;

        Vector2 startPosition = rb.position;

        float boostDuration = 0.2f; // Duration of the boost
        float originalTimeScale = Time.timeScale;

        isBoosting = true;

        // Time.timeScale = 0.01f;

        // Perform the boost action
        for (float t = 0; t < boostDuration; t += Time.unscaledDeltaTime)
        {
            float progress = t / boostDuration; // Calculate progress (0 to 1)
            float newX = Mathf.SmoothStep(0f, directionVector.x * 2, progress);

            Vector2 newPosition = startPosition + new Vector2(newX, 0);
            rb.MovePosition(newPosition); // Move to the interpolated position
            Physics2D.SyncTransforms();

            Debug.Log($"Progress: {progress}, New Position: {newPosition}");
            yield return null;
        }


        isBoosting = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("PlayerScript: OnTriggerEnter2D with " + collision.gameObject.tag);
        isColliding = true;

        if (timeSinceLastCollision < invincibleTime)
        {
            return;
        }

        timeSinceLastCollision = 0;
    }

    void OnCollisionExit2D()
    {

        isColliding = false;
    }
}
