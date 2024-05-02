using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCharacter : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = 0.0f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 move = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Arrow keys to move the character around
        move.x = Input.GetAxis("Horizontal") * speed;
        move.y = Input.GetAxis("Vertical") * speed;

        // E key to interact with objects
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Interacting with objects");
        }


        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);

    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        // Apply gravity
    }



}
