using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Test : MonoBehaviour
{
    [SerializeField] private PlayerInput MoveAction;
    private InputAction moveAction;
    private Vector2 InputMove = Vector2.zero;
    public float Speed=3;
    public float JampForce = 200;

    Rigidbody2D rb;

    public enum MoveMode 
    {

        Ceiling = 1,
        Floor = -1
     
    }

    public MoveMode moveMode = MoveMode.Floor;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction.actions["Move"].performed += OnMove;
        MoveAction.actions["Move"].canceled += OnMove;
        MoveAction.actions["Jump"].started += Jump;
        MoveAction.actions["GravityChange"].started += GravityChange;
        moveAction = MoveAction.actions["Move"];
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
      
    }

    private void Move()
    {
        if (InputMove.x >= 0.1f)
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        if (InputMove.x <= -0.1f)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
        }
    }
    private void Stop()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (moveMode == MoveMode.Floor)
            rb.AddForce(transform.up * -JampForce);

        else
            rb.AddForce(transform.up * JampForce);

       
    }

    private void GravityChange(InputAction.CallbackContext context)
    {
        if (moveMode == MoveMode.Floor)
            moveMode = MoveMode.Ceiling;
        else
            moveMode = MoveMode.Floor;

        rb.gravityScale = (float)moveMode;

    }
    // Update is called once per frame
    void Update()
    {

        if (moveMode == MoveMode.Floor)
        {
            transform.localScale = new Vector3(transform.localScale.x, -2, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 2, transform.localScale.z);
          
        }

        if (InputMove != Vector2.zero)
            Move();
        
        else
            Stop();
    }
   
}
