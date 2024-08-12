using System;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] Stages;
    private int Index = 0;
    private GameObject NowStage;
    [SerializeField] private Player_Test Player;
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        SetStage(Index);
    }
    public void NextStage()
    {
        Index++;
        SetStage(Index);
    }
    public void SetStage(int index)
    {
        if(index >= Stages.Length) 
        {
            Debug.LogError("ステージ選択範囲を超えています");
            return;
        }

        if(NowStage != null)
            Destroy(NowStage);

        GameObject stage = Instantiate(Stages[index]);
        NowStage = stage;
        Player.PlayerReset();
    }
    public void ResetGame()
    {
        if (NowStage != null)
            Destroy(NowStage);

        SetStage(Index);
    }
}
