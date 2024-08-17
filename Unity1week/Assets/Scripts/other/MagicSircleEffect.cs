using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MagicSircleEffect : MonoBehaviour
{

    SpriteRenderer SR;

    public float DashPower = 10;

    float angle = 0;
    float time = 0;
    float Speed = 0;

    float Acolor = 1;
    float DellAcolor = 0;
    float DellSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Clamp01(Acolor - DellAcolor);
        SR.color = new Color(1f, 1f, 1f, alpha);
        DellAcolor += DellSpeed * Time.deltaTime;
        if (DellAcolor > 1)
        {
            Destroy(gameObject);
        }

        transform.Rotate(0, 0, 90 * DellAcolor * Time.deltaTime);
        transform.localScale = new Vector2(transform.localScale.x +2 *DellAcolor*Time.deltaTime, transform.localScale.y + 2 * DellAcolor * Time.deltaTime);
    }
}
