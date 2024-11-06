using System;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{

    [SerializeField] private Stage[] Stages;
    [SerializeField] private Stage ClearStage;
    private int Index = 0;
    [SerializeField] private GameObject NowStage;
    [SerializeField] private Player_Test Player;
    [SerializeField] private Image FadeImage;
    public static GameManager Instance;
    [SerializeField] private float FadeTime;
    public GameObject StartPoint;
    public float GameTime { get; private set; } = 0;
    [SerializeField] private TextMeshProUGUI TimeText;
    [NonSerialized] public bool TimeStop = true;
    [SerializeField] private Camera MainCamera;
    private bool isClear = false;
    private float BestTime;
    public bool isBest = false;
    public bool isExtra;

    SoundManager soundManager => SoundManager.Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        if (isExtra)
            BestTime = PlayerPrefs.GetFloat("ExtraTime", -1);
        else
            BestTime = PlayerPrefs.GetFloat("Time", -1);
        FadeImage.color = Color.black;
        SetStage(Index);
    }

    private void Update()
    {
        if (!TimeStop)
        {
            GameTime += Time.deltaTime;
            TimeText.text = "Time:" + GameTime.ToString("000.00");
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
                    if (BestTime <= -1 || BestTime > GameTime)
                    {
                        isBest = true;
                        BestTime = GameTime;
                        if(isExtra)
                            PlayerPrefs.SetFloat("ExtraTime", GameTime);
                        else
                            PlayerPrefs.SetFloat("Time", GameTime);
                        PlayerPrefs.Save();
                       
                    }
                    stage = Instantiate(ClearStage.StageOb);
                    MainCamera.orthographicSize = ClearStage.CameraSize;
                    Player.DeadX = Player.StandardDeadX * (ClearStage.CameraSize / 6.0f);
                    Player.DeadY = Player.StandardDeadY * (ClearStage.CameraSize / 6.0f);
                }


                else
                {
                    stage = Instantiate(Stages[index].StageOb);
                    MainCamera.orthographicSize = Stages[index].CameraSize;
                    Player.DeadX = Player.StandardDeadX * (Stages[index].CameraSize / 6.0f);
                    Player.DeadY = Player.StandardDeadY * (Stages[index].CameraSize / 6.0f);
                }

                if (NowStage != null)
                    Destroy(NowStage);

                StartPoint = stage.transform.Find("StartPoint").gameObject;
                StartController start = StartPoint.GetComponent<StartController>();
                if (isClear)
                    start.text.text = "Clear";
                else
                    start.text.text = (index + 1).ToString() + "/" + Stages.Length;


                NowStage = stage;
                Player.PlayerReset(StartPoint);
                FadeImage.DOFade(0, FadeTime).OnComplete(() =>
                {
                    if (!isClear)
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

    public void ChangeScene(string StageName)
    {
        FadeImage.DOFade(1, FadeTime)
           .OnComplete(() =>
           {
               if (StageName == "Normal")
               {
                   SceneManager.LoadScene("MainGame");
                   soundManager.BGMText.color = new Color32(94,38,193,255);
                   soundManager.SEText.color = new Color32(94, 38, 193, 255);
               }
               else if(StageName =="Extra")
               {
                   SceneManager.LoadScene("ExtraGame");
                   soundManager.BGMText.color = Color.white;
                   soundManager.SEText.color = Color.white;
               }
           });
    }

    public void Backtitle()
    {
        SceneManager.LoadScene("Title");
    }
}