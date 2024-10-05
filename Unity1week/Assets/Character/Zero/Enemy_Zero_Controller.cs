using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zero_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    private SoundManager soundManager => SoundManager.Instance;
    Vector2 Direction;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Laser;
    [SerializeField] GameObject LaserOBJ;
    [SerializeField] GameObject DieArea;

    [SerializeField] AudioClip Charge;
    [SerializeField] AudioClip Shot;
    bool hitFloor = false;
    float LookTimer = 0;
    bool ShotNow = false;
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {


        Direction = Player.transform.position - transform.position;
        LaserOBJ.transform.up = Direction;
        LaserOBJ.transform.Rotate(0, 0, 90);

        Vector2 origin = transform.position;
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, Player.transform.position);

        // レイを飛ばす
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

        

        // レイが何かに当たった場合の処理
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    
                    hitFloor = true;
                    break;
                }
                else
                {
                    hitFloor = false;
                    //Debug.Log("WAWA");
                }
            }
        }

        if (hitFloor)
        {
            LookTimer = 0;
            Laser.transform.localScale = new Vector3(40, 0.1f, 1);
            Laser.SetActive(false);
        }
        else 
        {
            if (LookTimer == 0) 
            {
                soundManager.PlaySe(Charge);
                ShotNow = false;
            }

            LookTimer += Time.deltaTime;
            Laser.transform.localScale = new Vector3(40,0.1f+(LookTimer/1.5f),1);
            Laser.SetActive(true);
            if (LookTimer > 1) 
            {
                if (!ShotNow)
                {
                    soundManager.PlaySe(Shot);
                }
                Laser.transform.localScale = new Vector3(40, 3, 1);
                GameObject CL_DieArea = Instantiate(DieArea, Player.transform.position, Quaternion.identity);
                Destroy(CL_DieArea,0.1f );
                ShotNow = true;
               
            }
        }

        // floorタグのオブジェクトに当たらなかった場合
        if (!hitFloor)
        {
            //Shot();
            
        }

        // レイを可視化（デバッグ用）
        Debug.DrawRay(origin, direction * 10, Color.red);
    }
}

