using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] GameObject AAAAAAAA;
    private SoundManager soundManager => SoundManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            soundManager.PlaySe(clip);

            GameObject CL_A = Instantiate(AAAAAAAA,transform.position,Quaternion.identity);
        }
    }
}
