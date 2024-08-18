using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(transform.localScale*1.1f, 20).SetLoops(-1, LoopType.Yoyo); 
    }
}
