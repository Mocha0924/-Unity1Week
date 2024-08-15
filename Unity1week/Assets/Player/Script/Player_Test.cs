using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player_Test : MonoBehaviour
{
    [SerializeField] private PlayerInput MoveAction;
    private Vector2 InputMove = Vector2.zero;
    [SerializeField] private float Speed=3;
    [SerializeField] private float JampForce = 200;
    [SerializeField] private float MaxSpeed;
    public float DeadX;
    public float DeadY;
    [SerializeField] private GameObject LandingEffect;
    [SerializeField] private GameObject LandingEffectPos;
    private GameObject StartPoint;
    Rigidbody2D rb;
    private GameManager gameManager => GameManager.Instance;
    private bool isGround;
    public bool isGravityChange = true;
    public bool PlayerStop = false;
    private SoundManager soundManager => SoundManager.Instance;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip ChangeGravitySound;
    [SerializeField] private AudioClip DeathSound;
    [SerializeField] private AudioClip LandingSound;

    [SerializeField] private Animator PlayerAnimation;
    [SerializeField] private Transform cam;

    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    [SerializeField]private float shakeDuration;


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
        if(PlayerAnimation.GetInteger("Anim")<=0)
            PlayerAnimation.SetInteger("Anim",1);
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
        if (PlayerAnimation.GetInteger("Anim") <= 1)
            PlayerAnimation.SetInteger("Anim", 0);
        rb.velocity = new Vector2(0,rb.velocity.y);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if(isGround&&!PlayerStop)
        {
            soundManager.PlaySe(JumpSound, 1);
            PlayerAnimation.SetInteger("Anim", 3);
            if (moveMode == MoveMode.Floor)
                rb.AddForce(transform.up * -JampForce);

            else
                rb.AddForce(transform.up * JampForce);

            isGround = false;
        }
      

    }

    private void GravityChange(InputAction.CallbackContext context)
    {
        if(isGravityChange && !PlayerStop)
        {
            if (moveMode == MoveMode.Floor)
            {
                PlayerAnimation.SetBool("Down",false);
                moveMode = MoveMode.Ceiling;
            }
                
            else
            {
                PlayerAnimation.SetBool("Down", true);
                moveMode = MoveMode.Floor;
            }

            soundManager.PlaySe(ChangeGravitySound, 1);
            rb.gravityScale = (float)moveMode;
            isGround = false;
            isGravityChange = false;
        }
       

    }

    public void PlayerReset()
    {
        PlayerStop = false;
        moveMode = MoveMode.Ceiling;
        rb.gravityScale = (float)moveMode;
        StartPoint = GameObject.Find("StartPoint");
        rb.velocity = Vector2.zero;
        transform.position = StartPoint.transform.position;
    }

    public void Damage()
    {
        CameraShaker();
        PlayerStop = true;
        soundManager.PlaySe(DeathSound,1);
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
        if(rb.velocity.y >= 5||rb.velocity.y <= -5)
        {
            soundManager.PlaySe(LandingSound, 1);
            GameObject effect = Instantiate(LandingEffect, LandingEffectPos.transform.position, Quaternion.identity);
            if (moveMode == MoveMode.Floor)
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, -effect.transform.localScale.y, 0);

        }

      //  PlayerAnimation.SetInteger("Anim", 0);
        isGround = true;
        isGravityChange = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerStop)
            return;
        if(transform.position.x >= DeadX||
            transform.position.y >= DeadY||
            transform.position.x <= -DeadX||
            transform.position.y <= -DeadY)
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

        if(rb.velocity.y < 0&& moveMode == MoveMode.Ceiling|| rb.velocity.y > 0 && moveMode == MoveMode.Floor)
            PlayerAnimation.SetInteger("Anim", 2);
        else if(PlayerAnimation.GetInteger("Anim") == 2)
            PlayerAnimation.SetInteger("Anim", 0);

       
    }
    private void CameraShaker()
    {
        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }

}
