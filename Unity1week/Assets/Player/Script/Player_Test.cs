using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.LowLevel;
using System;
using UnityEngine.SocialPlatforms;
using unityroom.Api;
using UnityEngine.XR;

public class Player_Test : MonoBehaviour
{
    [SerializeField] private PlayerInput MoveAction;
    private Vector2 InputMove = Vector2.zero;
    [SerializeField] private float Speed=3;
    [SerializeField] private float JampForce = 200;
    [SerializeField] private float MaxSpeed;
    public float StandardDeadX { get; private set; }
    public float StandardDeadY { get; private set; }
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
    [SerializeField] private AudioClip GoalSound;

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
    public GravityMode gravityMode;
    private MoveMode moveMode = MoveMode.Right;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction.actions["Move"].performed += OnMove;
        MoveAction.actions["Move"].canceled += OnMove;
        MoveAction.actions["Jump"].started += Jump;
        MoveAction.actions["GravityChange"].started += GravityChange;
        MoveAction.actions["Restart"].performed += Restart;
        MoveAction.actions["Title"].started += Backtitke;
        MoveAction.actions["Finish"].started += FinishGame;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
      
    }
    private void Start()
    {
        gravityMode = GravityMode.Ceiling;
        StandardDeadX = DeadX;
        StandardDeadY = DeadY;
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
            rb.velocity = new Vector2(rb.velocity.x, 0);
            soundManager.PlaySe(JumpSound);
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
                transform.position += new Vector3(0, 0.093f, 0);
            }
                
            else
            {
                PlayerAnimation.SetBool("Down", false);
                gravityMode = GravityMode.Floor;
                transform.position -= new Vector3(0, 0.093f, 0);
            }

            Instantiate(ChangeGravityEffect, transform.position, Quaternion.identity);
            soundManager.PlaySe(ChangeGravitySound);
            rb.gravityScale = (float)gravityMode;
            isJump = false;
            InpossibleGravityChange();
        }
       

    }

    public void PlayerReset(GameObject Start)
    {
        PlayerAnimation.SetInteger("Anim", 0);
        gameObject.SetActive(true);
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
        if(gameManager != null)
        {
            rb.velocity = Vector2.zero;
            if (gravityMode == GravityMode.Floor)
                rb.AddForce(transform.up * -JampForce + (transform.right* JampForce*-1 ));

            else
                rb.AddForce(transform.up * JampForce + (transform.right * JampForce* -1 ));
            CameraShaker();
            PlayerAnimation.SetInteger("Anim", 10);
            PlayerStop = true;
            soundManager.PlaySe(DeathSound);
            gameManager.ContinueGame();
        }
       
       
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
            if(gameManager!=null)
            {
                GoalController goal = collision.gameObject.GetComponent<GoalController>();
                goal.SetGoal();
                CameraShaker();
                gameObject.SetActive(false);
                soundManager.PlaySe(GoalSound);
                PlayerStop = true;
                gameManager.NextStage();
            }
           
        }
        else if (collision.gameObject.tag == "RestartGoal")
        {
            GoalController goal = collision.gameObject.GetComponent<GoalController>();
            goal.SetGoal();
            CameraShaker();
            gameObject.SetActive(false);
            soundManager.PlaySe(GoalSound);
            PlayerStop = true;
            gameManager.RestartGame();
        }
        else if(collision.gameObject.tag == "tweet")
        {
            TweetController tweet = collision.gameObject.GetComponent<TweetController>();
            tweet.SetTweet();
            CameraShaker();
            gameObject.SetActive(false);
            soundManager.PlaySe(GoalSound);
            PlayerStop = true;
        }
        else if( collision.gameObject.tag =="Extra"|| collision.gameObject.tag == "Normal")
        {
            GoalController goal = collision.gameObject.GetComponent<GoalController>();
            goal.SetGoal();
            CameraShaker();
            gameObject.SetActive(false);
            soundManager.PlaySe(GoalSound);
            PlayerStop = true;
            gameManager.ChangeScene(collision.gameObject.tag);
        }
    }

    private void Landing()
    {
        if(rb.velocity.y >= 5||rb.velocity.y <= -5)
        {
            soundManager.PlaySe(LandingSound);
            GameObject effect = Instantiate(LandingEffect, LandingEffectPos.transform.position, Quaternion.identity);
            if (gravityMode == GravityMode.Floor)
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, -effect.transform.localScale.y, 0);

        }
        PlayerAnimation.SetInteger("Anim", 0);
        isJump = true;
        if(!isGravityChange)
        {
            PossibleGravityChange();
        }
      

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
            if (!ReGround||ReGround&&!isJump)
            {
                Landing();
               
            }

        }
        else
        {
            SetAirAnimation();
            isJump = false;
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
        MagicSircle.transform.DOComplete();
        isGravityChange = true;
        MagicSircle.SetActive(true);
        MagicSircle.transform.localScale = Vector3.zero;
        MagicSircle.transform.DOScale(Vector3.one*2,0.2f);

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
      
        return Physics2D.Linecast(StartGroundPos.position - transform.right * 0.1f, FinishGroundPos.position, FloorLayer) ||
               Physics2D.Linecast(StartGroundPos.position + transform.right * 0.1f, FinishGroundPos.position, FloorLayer);
    }

    private void Restart(InputAction.CallbackContext context)
    {
        PlayerStop = true;
        gameManager.TimeStop = true;
        gameManager.RestartGame();
    }

    private void Backtitke(InputAction.CallbackContext context)
    {
        PlayerStop = true;
        gameManager.TimeStop = true;
        gameManager.Backtitle();
    }

    private void FinishGame(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
