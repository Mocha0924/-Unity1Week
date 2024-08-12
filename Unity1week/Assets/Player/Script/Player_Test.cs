using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test : MonoBehaviour
{

    public float Speed=3;
    public float JampForce = 200;

    Rigidbody2D rb;

    public enum MoveMode 
    { 
    
        Floor,
        Ceiling

    }

    public MoveMode moveMode = MoveMode.Floor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (moveMode == MoveMode.Floor)
        {
            transform.localScale = new Vector3(transform.localScale.x,2, transform.localScale.z);

            rb.gravityScale = 1;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * JampForce);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                moveMode = MoveMode.Ceiling;
            }

        }
        else 
        {
            transform.localScale = new Vector3(transform.localScale.x, -2, transform.localScale.z);
            rb.gravityScale = -1;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(transform.up * -JampForce);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                moveMode = MoveMode.Floor;
            }
        }

        if (Input.GetKey(KeyCode.A)) 
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        
    }
    private void FixedUpdate()
    {
        
    }
}
