using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] GameObject SoundPlayObj;

    public static SoundManager Instance;

    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] float BGMMasterVol;
    public float BGMVol = 1f;
    public float SEVol = 1f;

    private AudioSource BGMAudio;
    [SerializeField] private AudioClip TitleBGM;
    [SerializeField] private AudioClip GameBGM;
    [SerializeField] private AudioClip ClearBGM;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        BGMSlider.value = BGMVol;
        SESlider.value = SEVol;
        BGMAudio = GetComponent<AudioSource>();
        BGMAudio.volume = BGMVol*BGMMasterVol;
    }

    public void SetVol()
    {
        BGMVol = BGMSlider.value;
        SEVol = SESlider.value;
        BGMAudio.volume = BGMVol * BGMMasterVol;
    }
    public void SetTitleBGM()
    {
        BGMAudio.Stop();
        BGMAudio.clip = TitleBGM;
        BGMAudio.Play();
    }
    public void SetGameBGM()
    {
        BGMAudio.Stop();
        BGMAudio.clip = GameBGM;
        BGMAudio.Play();
    }

    public void SetClearBGM()
    {
        BGMAudio.Stop();
        BGMAudio.clip = ClearBGM;
        BGMAudio.Play();
    }

    public void PlaySe(AudioClip Clip) 
    {
        SoundPlay soundPlay;
        
        GameObject SoundObj = Instantiate(SoundPlayObj);
        soundPlay = SoundObj.GetComponent<SoundPlay>();

        soundPlay.PlaySE(Clip,SEVol);

    }

}
