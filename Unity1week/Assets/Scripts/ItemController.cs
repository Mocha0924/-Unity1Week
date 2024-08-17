using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemController : MonoBehaviour
{
    [SerializeField] private float MaxTime;
    private float time = 0;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnDisable()
    {
        gameObject.transform.DOKill();
    }
    private void Update()
    {
        if(spriteRenderer.enabled == false)
        {
            time += Time.deltaTime;
        }
        if(time >= MaxTime)
        {
            time = 0;
            gameObject.transform.localScale = Vector3.zero;
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
            gameObject.transform.DOScale(Vector3.one,0.2f);
        }
    }
    public void TouchItem()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
