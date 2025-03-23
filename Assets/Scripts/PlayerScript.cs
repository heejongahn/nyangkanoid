using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatsScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float padding = 1f;
    public float paddleWidth = 3.5f; // Adjust based on sprite size
    public float speed = 20;

    private Vector2 moveDirection;

    private bool isColliding = false;
    private bool isGameOver = false;
    private float invincibleTime = 1;
    private float timeSinceLastCollision = 0;

    // Start is called before the first frame update
    void Start()
    {
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



    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("PlayerScript: OnTriggerEnter2D with " + collision.gameObject.tag);
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
