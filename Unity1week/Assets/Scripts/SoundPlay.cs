using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!audioSource.isPlaying) 
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(AudioClip Clip,float SEvol) 
    { 
    audioSource.clip = Clip;
        if (SEvol > 1) 
        {
            SEvol = 1;
        }
        
        audioSource.volume = SEvol;
        audioSource.Play();
    }
}
