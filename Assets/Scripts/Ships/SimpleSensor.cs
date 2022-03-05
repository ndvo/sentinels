using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSensor : MonoBehaviour
{

    public bool Blocked = false;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            Blocked = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            Blocked = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SpaceStation"))
        {
            Blocked = false;
        }
    }
}
