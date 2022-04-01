using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ShipContainerBehaviour : MonoBehaviour
{
    public int maxShips = 0;

    void Update()
    {
        foreach (Transform t in transform)
        {
            if (!t.gameObject.activeSelf)
                Destroy(t.gameObject);
        }
        
    }
}
