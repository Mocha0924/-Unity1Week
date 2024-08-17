using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SocialPlatforms;

public class GoalController : MonoBehaviour
{
   public void SetGoal()
    {
        gameObject.transform.DOScale(gameObject.transform.localScale*2,0.2f).OnComplete(() =>
        {
            gameObject.transform.DOScale(gameObject.transform.localScale / 2, 0.2f);
        });
    }
}
