using System;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    public class SentinelFlight : OrbitalFlight
    {

        protected override void _setNewDirection()
        {
            PreviousDirection = new Position(Direction.x, Direction.y);
            if (Input.GetKey("left"))
                Direction = Utils.Direction.West;
            if (Input.GetKey("right"))
                Direction = Utils.Direction.East;
            if (Input.GetKey("down"))
                Direction = Utils.Direction.South;
            if (Input.GetKey("up"))
                Direction = Utils.Direction.North;
        }
        
    }
}
