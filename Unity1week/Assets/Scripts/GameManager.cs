using System;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Stages;
    private int Index = 0;
    private GameObject NowStage;
    [SerializeField] private Player_Test Player;
    [SerializeField] private Image FadeImage;
    public static GameManager Instance;
    [SerializeField] private float FadeTime;

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
        FadeImage.DOFade(1, FadeTime)
            .OnComplete(() => 
            {
                if (index >= Stages.Length)
                {
                    Debug.LogError("ステージ選択範囲を超えています");
                    return;
                }

                if (NowStage != null)
                    Destroy(NowStage);

                GameObject stage = Instantiate(Stages[index]);
                NowStage = stage;
                Player.PlayerReset();
                FadeImage.DOFade(0, FadeTime);
            });
       
    }
    public void ResetGame()
    {
        SetStage(Index);
    }
}
