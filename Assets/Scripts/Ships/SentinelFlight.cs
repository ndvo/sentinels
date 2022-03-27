using System;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    public class SentinelFlight : ShipFlight
    {

        protected override void _setNewDirection()
        {
            PreviousDirection = new Position(CurrentDirection.x, CurrentDirection.y);
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            horizontal = horizontal > 0 ? 1 : horizontal < 0 ? -1 : 0;
            vertical = vertical > 0 ? -1 : vertical < 0 ? 1 : 0;
            if (horizontal != 0|| vertical != 0) CurrentDirection = new Position((int) horizontal, (int) vertical);
        }
        
    }
}
