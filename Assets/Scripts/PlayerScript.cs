using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatsScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float padding = 1f;
    public float paddleWidth = 3.5f; // Adjust based on sprite size
    public float speed = 20;

    private float minX, maxX;
    private Vector2 moveDirection;

    private bool isColliding = false;
    private bool isGameOver = false;
    private float invincibleTime = 1;
    private float timeSinceLastCollision = 0;

    // Start is called before the first frame update
    void Start()
    {

        float halfPaddle = paddleWidth / 2f;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        // Set paddle movement boundaries within the visible area
        minX = leftEdge.x + halfPaddle;
        maxX = rightEdge.x - halfPaddle;


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
        if (moveDirection == Vector2.zero)
        {
            return;
        }

        // Move the paddle
        Vector2 newPosition = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        // Clamp position within screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Apply movement
        rb.MovePosition(newPosition);
    }



    void OnCollisionEnter2D()
    {
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
