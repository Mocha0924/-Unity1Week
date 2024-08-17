using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Image FadeImage;
    [SerializeField] private float FadeTime;
    [SerializeField] private AudioClip UIClip;
    private InputAction _pressAnyKeyAction =
                new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>", interactions: "Press");

    private void OnEnable() => _pressAnyKeyAction.Enable();
    private void OnDisable() => _pressAnyKeyAction.Disable();
    private bool isFirstPush = true;
    private SoundManager soundManager => SoundManager.Instance;
    
    private void Update()
    {
        if (_pressAnyKeyAction.triggered)
        {
            isFirstPush = false;
            soundManager.PlaySe(UIClip,1);
            FadeImage.DOFade(1, FadeTime)
           .OnComplete(() =>
           {
               SceneManager.LoadScene("MainGame");
           });
        }
    }
}
