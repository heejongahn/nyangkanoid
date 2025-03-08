using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    public Rigidbody2D rb;

    public float speed = 15f;
    public Vector2 moveDirection = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f)).normalized;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                float paddleWidth = collision.collider.bounds.size.x;
                float offsetFromPaddleCenter = collision.transform.position.x - transform.position.x;

                // -1: Left Edge of the paddle
                //  0: Center of the paddle
                //  1: Right Edge of the paddle
                float hitFactor = offsetFromPaddleCenter / (paddleWidth / 2);
                float angle = -1 * hitFactor * Mathf.PI / 4; // Using PI/4 (45 degrees) as the max angle for the bounce

                // Convert angle to direction vector
                moveDirection.x = Mathf.Sin(angle); // X component
                moveDirection.y = Mathf.Cos(angle); // Y component (always positive)

                // Adjust the angle based on the hit factor
                moveDirection = moveDirection.normalized;

                break;
            case "Wall":
                moveDirection.x *= -1;
                break;
            case "TopWall":
                moveDirection.y *= -1;
                break;
            case "Block":
                Vector2 hitNormal = collision.contacts[0].normal;
                if (Mathf.Abs(hitNormal.x) > Mathf.Abs(hitNormal.y))
                {
                    moveDirection.x *= -1;
                }
                else
                {
                    moveDirection.y *= -1;
                }

                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }
}
