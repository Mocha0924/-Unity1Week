using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!audio.isPlaying) 
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(AudioClip Clip,float SEvol,float MasterVol) 
    { 
    audio.clip = Clip;
        if (SEvol > 1) 
        {
            SEvol = 1;
        }
        
        audio.volume = SEvol*MasterVol;
        audio.Play();
    }
}
