using Ships;
using UnityEngine;
using Utils;

/// <summary>
///     Controls the flight behaviour of projectiles.
/// </summary>
public class ProjectileFlight : OrbitalFlight
{
    private bool _hasTarget;
    private SimpleSensor _sensor;
    private GameObject _target;

    public override void Start()
    {
        var sensorObj = transform.Find("SimpleSensor");
        if (sensorObj != null) _sensor = sensorObj.GetComponent<SimpleSensor>();
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _hasTarget = true;
    }

    /// <summary>
    ///     The projectile follow the sentinel, avoiding stations.
    /// </summary>
    protected override void _setNewDirection()
    {
        if (!_hasTarget)
        {
            CurrentDirection = new Position(0, 0);
        }
        else
        {
            var direction = Vector3.Normalize(_target.transform.position - transform.position);
            var x = Mathf.Abs(direction.x);
            var z = Mathf.Abs(direction.z);
            if (x.Equals(z))
            {
                CurrentDirection = new Position(0, 0);
                return;
            }

            // Select the direction where the target is further (if target is further south than east, move south)
            CurrentDirection = x > z ? new Position(x > 0 ? 1 : -1, 0) : new Position(0, z > 0 ? 1 : -1);
            if (!_sensor.blocked || !PreviousDirection.Equals(CurrentDirection)) return;
            // If the path is blocked and it is the same as the previous frame, set direction using the closest direction
            CurrentDirection = x < z ? new Position(x > 0 ? 1 : -1, 0) : new Position(0, z > 0 ? 1 : -1);
        }
    }
}