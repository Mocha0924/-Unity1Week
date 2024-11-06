using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestTimeTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BestTimeText;
    [SerializeField] private TextMeshProUGUI BestText;
    private GameManager gameManager => GameManager.Instance;
    private void Start()
    {
        if(gameManager.isExtra)
            BestTimeText.text = "�x�X�g�^�C��\n"+PlayerPrefs.GetFloat("ExtraTime", -1).ToString("000.00")+"�т傤";
        else
            BestTimeText.text = "�x�X�g�^�C��\n" + PlayerPrefs.GetFloat("Time", -1).ToString("000.00") + "�т傤";
        if (gameManager.isBest)
            BestText.gameObject.SetActive(true);
        else
            BestText.gameObject.SetActive(false);
    }
}
