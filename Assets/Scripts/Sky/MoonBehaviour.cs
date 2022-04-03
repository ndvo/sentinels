using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

/// <summary>
/// Controls the behaviour of the Moon
///
/// Moon's role in the game is to provide an additional feature for immersion.
/// It does not try to be realistic, but to mimic some easily noticeable Moon behaviour:
/// - rotating around the Earth
/// - keeping the same face towards Earth at all times.
///
/// No effort is done to ensure fidelity in the orbit on with regards to witch side faces Earth.
/// </summary>
public class MoonBehaviour : OrbitalFlight
{
    
    public override void Start()
    {
        CurrentDirection = new Position(1, 0);
    }

    protected override void _orbitalFlight()
    {
        // Moon should keep the same side facing Earth at all times.
        transform.LookAt(Vector3.zero);
        _move(transform, speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Moon does not change direction
    /// </summary>
    protected override void _setNewDirection()
    {
    }
}
