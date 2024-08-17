using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly_Effect : MonoBehaviour
{
    public float DashPower = 10;

    float angle = 0;
    float time = 0;
    float Speed = 0;

    float Acolor = 1;
    float DellAcolor = 0;
    float DellSpeed = 0.9f;

    bool Shot = false;

    Rigidbody2D rb;
    SpriteRenderer sr;
    //[SerializeField] GameObject Player;
    [SerializeField] AudioClip Clip;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Clamp01(Acolor - DellAcolor);
        sr.color = new Color(1f, 1f, 1f, alpha);
        DellAcolor += DellSpeed * Time.deltaTime;
        if (DellAcolor > 1) 
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        isShot();
        
        time += Time.deltaTime;
        if (time > 0.05f && time < 1.4)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y * 0.9f);
            
        }
        else if (time > 1.4)
        {
            /*
            Vector2 Dir = Player.transform.position - transform.position;
            Speed = time * 8 - 16;
            rb.velocity = Dir.normalized * Speed;
            */
        }
    }
    void isShot()
    {
        if (!Shot)
        {
            Shot = true;
            angle = Random.Range(0, 361);
            DashPower = Random.Range(5, 11);
            float AngleRange = angle * Mathf.Deg2Rad;

            Vector2 force = new Vector2(Mathf.Cos(AngleRange), Mathf.Sin(AngleRange)) * DashPower;

            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
    
}
