using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Test : MonoBehaviour
{
    [SerializeField] private PlayerInput MoveAction;
    private Vector2 InputMove = Vector2.zero;
    [SerializeField] private float Speed=3;
    [SerializeField] private float JampForce = 200;
    [SerializeField] private float MaxSpeed;
    private GameObject StartPoint;
    Rigidbody2D rb;
    private GameManager gameManager => GameManager.Instance;
    private bool isGround;
    private bool isGravityChange = true;

    public enum MoveMode 
    {

        Ceiling = 1,
        Floor = -1
     
    }

    public MoveMode moveMode = MoveMode.Floor;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction.actions["Move"].performed += OnMove;
        MoveAction.actions["Move"].canceled += OnMove;
        MoveAction.actions["Jump"].started += Jump;
        MoveAction.actions["GravityChange"].started += GravityChange;
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
        if(isGround)
        {
            if (moveMode == MoveMode.Floor)
                rb.AddForce(transform.up * -JampForce);

            else
                rb.AddForce(transform.up * JampForce);

            isGround = false;
        }
      

    }

    private void GravityChange(InputAction.CallbackContext context)
    {
        if(isGravityChange)
        {
            if (moveMode == MoveMode.Floor)
                moveMode = MoveMode.Ceiling;
            else
                moveMode = MoveMode.Floor;

            rb.gravityScale = (float)moveMode;
            isGround = false;
            isGravityChange = false;
        }
       

    }

    public void PlayerReset()
    {
        moveMode = MoveMode.Ceiling;
        rb.gravityScale = (float)moveMode;
        StartPoint = GameObject.Find("StartPoint");
        rb.velocity = Vector2.zero;
        transform.position = StartPoint.transform.position;
    }

    public void Damage()
    {
        gameManager.ResetGame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacles")
        {
            Damage();
        }
        else if(collision.gameObject.tag == "Goal")
        {
            gameManager.NextStage();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = true;
        isGravityChange = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= 15||
            transform.position.y >= 8||
            transform.position.x <= -15||
            transform.position.y <= -8)
        {
            Damage(); 
        }

        if (moveMode == MoveMode.Floor)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
          
        }

        if (InputMove != Vector2.zero)
            Move();
        
        else
            Stop();

       
        if(rb.velocity.y >= MaxSpeed)
            rb.velocity = new Vector2 (rb.velocity.x, MaxSpeed);
        if (rb.velocity.y <= -MaxSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -MaxSpeed);
    }
   
}
