using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Time = UnityEngine.Time;

public class HeadquartersFlight : ShipFlight
{
    protected override void _orbitalFlight()
    {
        DeltaTimeSpeed = speed * Time.deltaTime;
        if (Random.value < 0.0001f || !OffBoard) _setDirection();
        _positionAim();
        LookAtDirection();
        _move(transform, DeltaTimeSpeed);
    }
    
    private void _setDirection()
    {
        CurrentDirection = CurrentDirection.x != 0 
            ? new Position(0, Random.value > 0.5f ? 1 : -1) 
            : new Position(Random.value > 0.5f ? 1 : -1, 0);
    }

}
