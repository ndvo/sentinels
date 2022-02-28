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
        public GameObject target;
        private float directionTime;

        public override void Awake()
        {
            directionTime = 0;
            base.Awake();
            sensor = transform.Find("Sensor").GetComponent<SensorBehaviour>();
            mtSearch = new MonteCarloTreeSearch();
            Direction = new Position(0, 0);
        }

        protected override void _setNewDirection()
        {
            directionTime += Time.deltaTime;
            if (directionTime >= 0.2f) {
                directionTime = 0.0f;
                var path = mtSearch.ComputePath(sensor.GetTreeNode(), target.transform.position);
                if (path.Nodes != null && path.Nodes.Count > 0)
                {
                    sensor.ShowIfInList(path.Nodes);
                    var next = path.Nodes[1];
                    var position = transform.position;
                    var vertical = next.Position.z - position.z;
                    var horizontal = next.Position.x - position.x;
                    Direction = Math.Abs(vertical) > Math.Abs(horizontal) 
                        ? new Position(0, vertical > 0 ? 1 : -1) 
                        : new Position(horizontal > 0 ? 1 : -1, 0);
                }
            }
        }

    }
}
