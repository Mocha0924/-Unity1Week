using System;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using unityroom.Api;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Stages;
    [SerializeField] private GameObject ClearStage;
    private int Index = 0;
    [SerializeField] private GameObject NowStage;
    [SerializeField] private Player_Test Player;
    [SerializeField] private Image FadeImage;
    public static GameManager Instance;
    [SerializeField] private float FadeTime;
    public GameObject StartPoint;
    public float GameTime { get; private set; } = 0;
    [SerializeField] private TextMeshProUGUI TimeText;
    [NonSerialized]public bool TimeStop = true;
    private bool isClear = false;
    private float BestTime;
    public bool isBest = false;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        BestTime = PlayerPrefs.GetFloat("Time", -1);
        FadeImage.color = Color.black;
        SetStage(Index);
    }

    private void Update()
    {
        if (!TimeStop)
        {
            GameTime += Time.deltaTime;
            TimeText.text = "Time:"+GameTime.ToString("000.00");
        }
          
    }
    public void NextStage()
    {
        Index++;
        SetStage(Index);
    }
    public void SetStage(int index)
    {
        if (Stages.Length <= 0)
        {
            FadeImage.DOFade(0, FadeTime);
            return;
        }
           
        FadeImage.DOFade(1, FadeTime)
            .OnComplete(() => 
            {
                GameObject stage;
                if (index >= Stages.Length)
                {
                    TimeStop = true;
                    isClear = true;
                    TimeText.text = "Time:" + GameTime.ToString("000.00");
                    if(BestTime <=-1||BestTime>GameTime)
                    {
                        isBest = true;
                        BestTime = GameTime;
                        PlayerPrefs.SetFloat("Time", GameTime);
                        PlayerPrefs.Save();
                        UnityroomApiClient.Instance.SendScore(1, BestTime, ScoreboardWriteMode.HighScoreAsc);
                    }
                    stage = Instantiate(ClearStage);
                }
                    

                else
                    stage = Instantiate(Stages[index]);

                if (NowStage != null)
                    Destroy(NowStage);

                StartPoint = stage.transform.Find("StartPoint").gameObject;
                StartController start = StartPoint.GetComponent<StartController>();
                if (isClear)
                    start.text.text = "Clear";
                else
                    start.text.text = (index + 1).ToString() + "/"+ Stages.Length;
                
                    
                NowStage = stage;
                Player.PlayerReset(StartPoint);
                FadeImage.DOFade(0, FadeTime).OnComplete(() =>
                { 
                    if(!isClear)
                        TimeStop = false;
                });
            });
       
    }
    public void ContinueGame()
    {
        SetStage(Index);
    }

    public void RestartGame()
    {
        isBest = false;
        isClear = false;
        Index = 0;
        GameTime = 0;
        TimeText.text = "Time:" + GameTime.ToString("000.00");
        SetStage(Index);
    }
}
