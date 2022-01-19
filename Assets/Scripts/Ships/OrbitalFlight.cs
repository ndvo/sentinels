using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalFlight : MonoBehaviour
{
    
    public float speed;

    private void FixedUpdate()
    {
        if (Input.GetKey("up"))
            transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.left, speed);
        if (Input.GetKey("down"))
            transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.left, speed * -1);
        if (Input.GetKey("right"))
            transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.forward,  speed);
        if (Input.GetKey("left"))
            transform.RotateAround(new Vector3(0f, 0f, 0f), Vector3.forward, speed * -1);
    }
}
