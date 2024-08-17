using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflySpawn_Controller : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject Butterfly;
    [SerializeField] GameObject Mahoujin;



     float SpawnTime = 0.001f;
     float DieTime = 0.1f;
    float Timer = 0;
    float SpawnTimer = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        SpawnTimer += Time.deltaTime;

        GameObject CL_Mahoujin = Instantiate(Mahoujin,transform.position,Quaternion.identity);
    }
    private void FixedUpdate()
    {
        
        if (SpawnTimer > SpawnTime) 
        {
            SpawnTimer = 0;
            GameObject CL_BT = Instantiate(Butterfly,transform.position,Quaternion.identity);
        }
        if (DieTime < Timer) 
        {
            Destroy(gameObject);
        }
    }
}
