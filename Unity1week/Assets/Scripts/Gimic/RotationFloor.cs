using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFloor : MonoBehaviour
{

    public float Speed = 10;

    public enum Mode 
    { 
    RightRotation,
    LeftRotation,
    Stop

    }
    public Mode mode = Mode.RightRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        switch (mode) 
        { 
            case Mode.RightRotation:
                isRotate(-1);
                break;

            case Mode.LeftRotation:
                isRotate(1);
                break;
            case Mode.Stop:

                break;
        }
    }

    void isRotate(int num) 
    {

        transform.Rotate(0, 0, Speed *num);
    
    }
}
