using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ChangeFontController : MonoBehaviour
{
    private TextMeshProUGUI ExplanationText;
    [TextArea(1,3)][SerializeField] private string NoControllerText;
    [TextArea(1,3)][SerializeField] private string ControllerText;
  
    private void Start()
    {
        ExplanationText = GetComponent<TextMeshProUGUI>();
        var devices = InputSystem.devices;
   
        foreach (var device in devices)
        {
            if (device is Gamepad)
            {
                ExplanationText.text = ControllerText;
                return;
            }
        }
        ExplanationText.text = NoControllerText;
    }
    private void OnEnable()
    {
        //デバイスの接続/切断のイベントに処理登録
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        //デバイスの接続/切断のイベントの処理解除
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    ///デバイスの変更があった
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {//デバイスがゲームパッド(コントローラー)の時だけ処理
            switch (change)
            {
                case InputDeviceChange.Added:
                    ExplanationText.text = ControllerText;
                    break;
                case InputDeviceChange.Removed:
                    ExplanationText.text = NoControllerText;
                    break;
            }
        }
    }
}
