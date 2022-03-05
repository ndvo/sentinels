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
            PreviousDirection = new Position(CurrentDirection.x, CurrentDirection.y);
            if (Input.GetKey("left"))
                CurrentDirection = Utils.Direction.West;
            if (Input.GetKey("right"))
                CurrentDirection = Utils.Direction.East;
            if (Input.GetKey("down"))
                CurrentDirection = Utils.Direction.South;
            if (Input.GetKey("up"))
                CurrentDirection = Utils.Direction.North;
        }
        
    }
}
