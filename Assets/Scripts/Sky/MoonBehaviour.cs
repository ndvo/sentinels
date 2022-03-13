using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class MoonBehaviour : OrbitalFlight
{
    public override void Start()
    {
        latitude = Random.Range(0, 180);
        longitude = 90;
    }

    protected override void _orbitalFlight()
    {
        transform.LookAt(Vector3.zero);
        _move(transform, 0.3f * Time.fixedDeltaTime);
    }
    
}
