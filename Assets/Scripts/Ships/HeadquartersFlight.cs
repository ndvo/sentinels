using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Time = UnityEngine.Time;

public class HeadquartersFlight : ShipFlight
{
    public override void Update()
    {
    }

    protected override void _orbitalFlight()
    {
        OffBoard = HasSpaceStationBuilder && SpaceStationBuilder.IsOffBoard(transform.position);
        DeltaTimeSpeed = speed * Time.fixedDeltaTime;
        if (Random.value < 0.001f || !OffBoard) _setNewDirection();
        _positionAim();
        LookAtDirection();
        _move(transform, DeltaTimeSpeed);
    }

    protected override void _setNewDirection()
    {
        Debug.Log("Whats happening");
        Debug.Log(OffBoard);
        CurrentDirection = CurrentDirection.x != 0 
            ? new Position(0, Random.value > 0.5f ? 1 : -1) 
            : new Position(Random.value > 0.5f ? 1 : -1, 0);
    }

}
