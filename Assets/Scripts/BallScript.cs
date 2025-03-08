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
            moveDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0f, 1f));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartGame();
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
                moveDirection.y *= -1;
                break;
            case "Wall":
                moveDirection.x *= -1;
                break;
            case "TopWall":
                moveDirection.y *= -1;
                break;
            case "Block":
                Destroy(collision.gameObject);
                moveDirection.y *= -1;
                break;
            default:
                break;
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
