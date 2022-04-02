using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleSensor : MonoBehaviour
{

    public LayerMask layer;
    public Vector3 sensorScale = Vector3.one;
    public bool blocked = false;
    public GameObject blocking;

    private void Update()
    {
        transform.localScale = sensorScale;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layer) == 0) return;
        blocked = true;
        blocking = other.gameObject;
    }

    public void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & layer) == 0) return;
        blocked = true;
        blocking = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & layer) == 0) return;
        blocked = false;
        blocking = null;
    }
}
