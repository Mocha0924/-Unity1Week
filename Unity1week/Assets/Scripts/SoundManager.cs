using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] GameObject SoundPlayObj;

    public static SoundManager Instance;

    [SerializeField] float MasterVol =1f;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance = null)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySe(AudioClip Clip ,float SEvol) 
    {
        SoundPlay soundPlay;
        
        GameObject SoundObj = Instantiate(SoundPlayObj);
        soundPlay = SoundObj.GetComponent<SoundPlay>();

        soundPlay.PlaySE(Clip,SEvol,MasterVol);

    }

}
