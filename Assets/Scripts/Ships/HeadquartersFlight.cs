using Ships;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

/// <summary>
///     Controls the flight of the Headquarters
///     The purpose of the Headquarters flight is twofold:
///     - distribute spawned ships in a less predictable way
///     - make it harder for players to destroy the headquarter (this feature is not implemented yet and will not be
///     implemented before the final project deadline)
/// </summary>
public class HeadquartersFlight : ShipFlight
{
    public override void Update()
    {
        // remove base behavior in ShipFlight update method
    }

    /// <summary>
    ///     Override the orbital flight to change directions in a random fashion and to fly only off board.
    /// </summary>
    protected override void _orbitalFlight()
    {
        OffBoard = HasSpaceStationBuilder && SpaceStationBuilder.IsOffBoard(transform.position);
        DeltaTimeSpeed = speed * Time.fixedDeltaTime;
        if (Random.value < 0.0005f || !OffBoard) _setNewDirection();
        _positionAim();
        LookAtDirection();
        _move(transform, DeltaTimeSpeed);
    }

    protected override void _setNewDirection()
    {
        CurrentDirection = CurrentDirection.x != 0
            ? new Position(0, Random.value > 0.5f ? 1 : -1)
            : new Position(Random.value > 0.5f ? 1 : -1, 0);
    }
}