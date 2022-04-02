using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

public class MoonBehaviour : OrbitalFlight
{
    
    public override void Start()
    {
        CurrentDirection = new Position(1, 0);
    }

    protected override void _orbitalFlight()
    {
        transform.LookAt(Vector3.zero);
        _move(transform, speed * Time.fixedDeltaTime);
    }

    protected override void _setNewDirection()
    {
    }
}
