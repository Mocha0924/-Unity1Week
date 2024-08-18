using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.LowLevel;

public class TitlePlayer_Test : MonoBehaviour
{
    [SerializeField] private PlayerInput MoveAction;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private GameObject LandingEffect;
    [SerializeField] private GameObject LandingEffectPos;
    [SerializeField] private LayerMask FloorLayer;
    [SerializeField] private Transform StartGroundPos;
    [SerializeField] private Transform FinishGroundPos;
    private bool ReGround = true;
    [SerializeField] private GameObject ChangeGravityEffect;
    Rigidbody2D rb;
    public bool isGravityChange = true;
    private SoundManager soundManager => SoundManager.Instance;
    [SerializeField] private AudioClip ChangeGravitySound;
    [SerializeField] private AudioClip LandingSound;
 
    [SerializeField] private Animator PlayerAnimation;
  
    public GameObject MagicSircle;

    [SerializeField] private float MaxTime;
    private float GameTime;


    public enum GravityMode 
    {

        Ceiling = 1,
        Floor = -1
     
    }

    

    public GravityMode gravityMode;
    // Start is called before the first frame update
    private void OnEnable()
    {
        MoveAction.actions["GravityChange"].started += GravityChange;
    }

    private void OnDisable()
    {
        MoveAction.actions["GravityChange"].started -= GravityChange;
    }


    private void Start()
    {
        gravityMode = GravityMode.Ceiling;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = (float)gravityMode;
    }

    private void TimeGravityChange()
    {
        if (isGravityChange)
        {
            if (gravityMode == GravityMode.Floor)
            {
                PlayerAnimation.SetBool("Down", true);
                gravityMode = GravityMode.Ceiling;
            }

            else
            {
                PlayerAnimation.SetBool("Down", false);
                gravityMode = GravityMode.Floor;
            }

            Instantiate(ChangeGravityEffect, transform.position, Quaternion.identity);
            soundManager.PlaySe(ChangeGravitySound);
            rb.gravityScale = (float)gravityMode;

            InpossibleGravityChange();
        }
    }
    private void GravityChange(InputAction.CallbackContext context)
    {
        if(isGravityChange)
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
            soundManager.PlaySe(ChangeGravitySound);
            rb.gravityScale = (float)gravityMode;
            
            InpossibleGravityChange();
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
        PossibleGravityChange();

    }


    // Update is called once per frame
    void Update()
    {
        if (isGravityChange)
            GameTime+=Time.deltaTime;

        else
            GameTime = 0;

        if (GameTime >= MaxTime)
        {
            TimeGravityChange();
            GameTime = 0;
        }


        if (gravityMode == GravityMode.Floor)
        {
            transform.localScale = new Vector3(-1, -1, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, transform.localScale.z);
          
        }

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
