using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;
using Utils;

public class ProjectileFlight : OrbitalFlight
{
    private SimpleSensor _sensor;
    public GameObject target;
    public override void Awake()
    {
        HasSpaceStationBuilder = false;
        AudioSource = GetComponent<AudioSource>();
    }

    public override void Start()
    {
        var sensorObj = transform.Find("SimpleSensor");
        if (sensorObj != null)
        {
            _sensor = sensorObj.GetComponent<SimpleSensor>();
        }
    }

    protected override void _setNewDirection()
    {
        if (!(target is { }))
        {
            CurrentDirection = new Position(0, 0);
        }
        else
        {
            var direction = Vector3.Normalize(target.transform.position - transform.position));
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
