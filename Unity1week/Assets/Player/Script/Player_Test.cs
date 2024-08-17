using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.LowLevel;

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
    [SerializeField] private LayerMask FloorLayer;
    [SerializeField] private Transform StartGroundPos;
    [SerializeField] private Transform FinishGroundPos;
    private bool ReGround = true;
    [SerializeField] private GameObject ChangeGravityEffect;
    private GameObject StartPoint;
    Rigidbody2D rb;
    private GameManager gameManager => GameManager.Instance;
    private bool isJump;
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
    public GameObject MagicSircle;

    public enum GravityMode 
    {

        Ceiling = 1,
        Floor = -1
     
    }
    private enum MoveMode
    {

        Right = 1,
        Left = -1

    }
    public GravityMode gravityMode = GravityMode.Floor;
    private MoveMode moveMode = MoveMode.Right;

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
            moveMode = MoveMode.Right;
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        if (InputMove.x <= -0.1f)
        {
            moveMode = MoveMode.Left;
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
        if(isJump&&!PlayerStop)
        {
            soundManager.PlaySe(JumpSound, 1);
            if (gravityMode == GravityMode.Floor)
                rb.AddForce(transform.up * -JampForce);

            else
                rb.AddForce(transform.up * JampForce);

            isJump = false;
            PlayerAnimation.SetInteger("Anim", 2);
        }
      

    }

    private void GravityChange(InputAction.CallbackContext context)
    {
        if(isGravityChange && !PlayerStop)
        {
            if (gravityMode == GravityMode.Floor)
            {
                PlayerAnimation.SetBool("Down",true);
                gravityMode = GravityMode.Ceiling;
            }
                
            else
            {
                PlayerAnimation.SetBool("Down", false);
                gravityMode = GravityMode.Floor;
            }

            Instantiate(ChangeGravityEffect, transform.position, Quaternion.identity);
            soundManager.PlaySe(ChangeGravitySound, 1);
            rb.gravityScale = (float)gravityMode;
            isJump = false;
            InpossibleGravityChange();
        }
       

    }

    public void PlayerReset(GameObject Start)
    {
        gravityMode = GravityMode.Ceiling;
        rb.gravityScale = (float)gravityMode;
        PlayerAnimation.SetBool("Down", true);
        StartPoint = Start;
        isJump = true;
        rb.velocity = Vector2.zero;
        transform.position = StartPoint.transform.position;
        PossibleGravityChange();
        PlayerStop = false;
    }

    public void Damage()
    {
        CameraShaker();
        PlayerStop = true;
        soundManager.PlaySe(DeathSound,1);
        gameManager.ContinueGame();
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PlayerStop)
            return;
        if(collision.gameObject.tag == "Obstacles")
        {
            Damage();
        }
        else if(collision.gameObject.tag == "Goal")
        {
            PlayerStop = true;
            gameManager.NextStage();
        }
      
    }

    private void Landing()
    {
        if(rb.velocity.y >= 5||rb.velocity.y <= -5)
        {
            soundManager.PlaySe(LandingSound, 1);
            GameObject effect = Instantiate(LandingEffect, LandingEffectPos.transform.position, Quaternion.identity);
            if (gravityMode == GravityMode.Floor)
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, -effect.transform.localScale.y, 0);

        }
        PlayerAnimation.SetInteger("Anim", 0);
        isJump = true;
        PossibleGravityChange();

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
   
        if (gravityMode == GravityMode.Floor)
        {
            transform.localScale = new Vector3((int)moveMode, -1, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3((int)moveMode, 1, transform.localScale.z);
          
        }

        if (InputMove != Vector2.zero)
            Move();
        
        else
            Stop();

       
        if(rb.velocity.y >= MaxSpeed)
            rb.velocity = new Vector2 (rb.velocity.x, MaxSpeed);
        if (rb.velocity.y <= -MaxSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -MaxSpeed);

        bool ground = IsGround();
       
        if (ground)
        {
            if (!ReGround)
            {
                Landing();
               
            }

        }
        else
        {
            SetAirAnimation();
        }
      

        ReGround = ground;
       
    }

   
    private void CameraShaker()
    {
        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }

    public void PossibleGravityChange()
    {
        isGravityChange = true;
        MagicSircle.SetActive(true);
    }

    public void InpossibleGravityChange()
    {
        isGravityChange = false;
        MagicSircle.SetActive(false);
    }

    public void SetAirAnimation()
    {
        if(gravityMode == GravityMode.Ceiling)
        {
            if(rb.velocity.y <=0)
                PlayerAnimation.SetInteger("Anim", 2);
            else
                PlayerAnimation.SetInteger("Anim", 3);
            
        }
        else
        {
            if (rb.velocity.y >= 0)
                PlayerAnimation.SetInteger("Anim", 2);
            else
                PlayerAnimation.SetInteger("Anim", 3);
          
        }
    }

    private bool IsGround()
    {
      
        return Physics2D.Linecast(StartGroundPos.position - transform.right * 0.2f, FinishGroundPos.position - transform.right * 0.2f, FloorLayer) ||
               Physics2D.Linecast(StartGroundPos.position + transform.right * 0.2f, FinishGroundPos.position + transform.right * 0.2f, FloorLayer);
    }
}
