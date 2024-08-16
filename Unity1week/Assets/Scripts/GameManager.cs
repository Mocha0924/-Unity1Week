using System;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Stages;
    [SerializeField] private GameObject ClearStage;
    private int Index = 0;
    private GameObject NowStage;
    [SerializeField] private Player_Test Player;
    [SerializeField] private Image FadeImage;
    public static GameManager Instance;
    [SerializeField] private float FadeTime;
    public GameObject StartPoint;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        FadeImage.color = Color.black;
        SetStage(Index);
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
                    stage = Instantiate(ClearStage);

                else
                    stage = Instantiate(Stages[index]);

                if (NowStage != null)
                    Destroy(NowStage);

                StartPoint = stage.transform.Find("StartPoint").gameObject;
                NowStage = stage;
                Player.PlayerReset(StartPoint);
                FadeImage.DOFade(0, FadeTime);
            });
       
    }
    public void ContinueGame()
    {
        SetStage(Index);
    }

    public void RestartGame()
    {
        Index = 0;
        SetStage(Index);
    }
}
