using System;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public Rigidbody2D rb;
    private float fallVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fallVelocity = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + new Vector2(0, -fallVelocity));
    }
}
