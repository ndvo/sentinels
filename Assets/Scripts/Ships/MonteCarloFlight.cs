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
            if (directionTime >= 0.1f) {
                directionTime = 0.0f;
                var path = mtSearch.ComputePath(sensor.GetTreeNode(), target.transform.position);
                if (path.Nodes == null || path.Nodes.Count <= 1) return;
                sensor.ShowIfInList(path.Nodes);
                var nodes = path.Nodes;
                var next = nodes[1];
                var position = transform.position;
                var nextPosition = next.Position;
                var vertical = nextPosition.z - position.z;
                var horizontal = nextPosition.x - position.x;
                var absV = Math.Abs(vertical);
                var absH = Math.Abs(horizontal);
                Direction = absV > absH
                    ? new Position(0, (int) (vertical/absV))
                    : new Position((int) (horizontal/absH), 0);
            }
        }

    }
}
