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
   
    private bool isFirstPush = true;
    private SoundManager soundManager => SoundManager.Instance;

   
    private void Update()
    {
        if(Gamepad.current!=null&&isFirstPush)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                isFirstPush = false;
                soundManager.PlaySe(UIClip);
                FadeImage.DOFade(1, FadeTime)
               .OnComplete(() =>
               {
                   soundManager.BGMText.color = new Color32(94, 38, 193, 255);
                   soundManager.SEText.color = new Color32(94, 38, 193, 255);
                   SceneManager.LoadScene("MainGame");
               });
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isFirstPush)
            {
                isFirstPush = false;
                soundManager.PlaySe(UIClip);
                FadeImage.DOFade(1, FadeTime)
               .OnComplete(() =>
               {
                   soundManager.BGMText.color = new Color32(94, 38, 193, 255);
                   soundManager.SEText.color = new Color32(94, 38, 193, 255);
                   SceneManager.LoadScene("MainGame");
               });
            }
        }
      
    }
}
