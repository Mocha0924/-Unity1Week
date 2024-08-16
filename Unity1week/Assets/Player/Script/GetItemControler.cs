using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_Test;

public class GetItemControler : MonoBehaviour
{
    [SerializeField] private Player_Test player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Item")
        {
            player.PossibleGravityChange();
            Destroy(collision.gameObject);
        }
    }
}
