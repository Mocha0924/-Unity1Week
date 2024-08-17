using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SocialPlatforms;

public class TweetController : MonoBehaviour
{
    private GameManager gameManager => GameManager.Instance;
   public void SetTweet()
    {
        gameObject.transform.DOScale(gameObject.transform.localScale*2,0.2f).OnComplete(() =>
        {
            gameObject.transform.DOScale(gameObject.transform.localScale / 2, 0.2f);
            naichilab.UnityRoomTweet.Tweet("switchwitch", "���̃x�X�g�^�C����"+ PlayerPrefs.GetFloat("Time", -1).ToString("000.00")+ "�b�ł�\n#unity1week\n#SwitchWitch");
            gameManager.ContinueGame();
        });
    }
}
