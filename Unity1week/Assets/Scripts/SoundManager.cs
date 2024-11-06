using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] GameObject SoundPlayObj;

    public static SoundManager Instance;

    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] float BGMMasterVol;
    [SerializeField] private float BGMVol;
    [SerializeField] private float SEVol;

    private AudioSource BGMAudio;
    [SerializeField] private AudioClip GameBGM;
    public TextMeshProUGUI BGMText;
    public TextMeshProUGUI SEText;

    // Start is called before the first frame update
    void Awake()
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
       
    }

    private void Start()
    {
        BGMAudio = GetComponent<AudioSource>();
        BGMSlider.value = BGMVol;
        SESlider.value = SEVol;
        BGMAudio.volume = BGMVol * BGMMasterVol;
        SetGameBGM();
    }
    public void BGMSetVol()
    {
        BGMVol = BGMSlider.value;
        BGMAudio.volume = BGMVol * BGMMasterVol;
    }
   

    public void SESetvol()
    {
        SEVol = SESlider.value;
    }
    public void SetGameBGM()
    {
        BGMAudio.Stop();
        BGMAudio.clip = GameBGM;
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
