using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ChangeFontController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ExplanationText;
    [SerializeField] private string NoControllerText;
    [SerializeField] private string ControllerText;
  
    private void Start()
    {
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
        //�f�o�C�X�̐ڑ�/�ؒf�̃C�x���g�ɏ����o�^
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        //�f�o�C�X�̐ڑ�/�ؒf�̃C�x���g�̏�������
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    ///�f�o�C�X�̕ύX��������
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {//�f�o�C�X���Q�[���p�b�h(�R���g���[���[)�̎���������
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
