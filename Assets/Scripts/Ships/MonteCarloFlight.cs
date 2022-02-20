using System;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    public class MonteCarloFlight : OrbitalFlight
    {
        private SensorBehaviour sensor;
        private MonteCarloTreeSearch mtSearch;

        public override void Awake()
        {
            base.Awake();
            sensor = transform.Find("Sensor").GetComponent<SensorBehaviour>();
            mtSearch = new MonteCarloTreeSearch();
        }

        protected override void _setNewDirection()
        {
            var path = mtSearch.ComputePath(sensor.GetTreeNode(), new Vector3(0, 500, 200));
            if (path.Reached)
            {
                speed = 0;
                return;
            }
            var next = path.Nodes[0];
            var position = transform.position;
            var vertical = next.Position.z - position.z;
            var horizontal = next.Position.x - position.x;
            Direction = Math.Abs(vertical) > Math.Abs(horizontal) 
                ? new Position(0, vertical > 0 ? 1 : -1) 
                : new Position(horizontal > 0 ? 1 : -1, 0);
        }

    }
}
