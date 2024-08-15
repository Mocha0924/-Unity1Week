using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class DamageEffectController : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    private Player_Test PlayerController;
    private Image image;
    [SerializeField] private float Limit;

    private void Start()
    {
        image = GetComponent<Image>();
        PlayerController = Player.GetComponent<Player_Test>();
    }

    private void Update()
    {
        if (PlayerController.PlayerStop)
            return;

        float PlusX = (PlayerController.DeadX - Player.transform.position.x) / PlayerController.DeadX;
        float MinusX = (-PlayerController.DeadX - Player.transform.position.x) / PlayerController.DeadX * -1;
        float PlusY = (PlayerController.DeadY - Player.transform.position.y) / PlayerController.DeadY;
        float MinusY = (-PlayerController.DeadY - Player.transform.position.y) / PlayerController.DeadY * -1;

        float[] Distance = new float[4];
        Distance[0] = PlusX;
        Distance[1] = MinusX;
        Distance[2] = PlusY;
        Distance[3] = MinusY;

        float min = Mathf.Min(Distance);

        if (min > Limit)
            image.color = new Color32(255, 0, 0,0);
        else
            image.color = new Color32(255, 0, 0, (byte)(38 -(38 * (min+1-Limit))));


       

    }

    
}
