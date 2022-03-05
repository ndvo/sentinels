using UnityEngine;
using Utils;

namespace Ships
{
    public class SimpleFlight : OrbitalFlight
    {
        private SimpleSensor _sensor;

        public override void Start()
        {
            CurrentDirection = Utils.Direction.North;
            var sensorObj = transform.Find("SimpleSensor");
            if (sensorObj != null)
            {
                _sensor = sensorObj.GetComponent<SimpleSensor>();
            }
        }

        protected override void _setNewDirection()
        {
            if (_sensor.Blocked)
            {
                CurrentDirection = CurrentDirection.x != 0 
                    ? new Position(0, Random.value > 0.5f ? 1 : -1) 
                    : new Position(Random.value > 0.5f ? 1 : -1, 0);
            }
        }
    }
}
