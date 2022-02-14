using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace Ships
{
    public class MonteCarmeloSensors: MonoBehaviour
    {
        public int level = 0;
        public GameObject sensorTemplate;
        
        public void Start()
        {
            var position = transform.position;
            var sensor = Object.Instantiate(sensorTemplate, transform);
            CreateChildren(sensor, level - 1);
        }

        private void CreateChildren(GameObject parent, int childrenLevel)
        {
            if (childrenLevel < 0) return;
            var forward = Object.Instantiate(sensorTemplate, parent.transform);
            var left = Object.Instantiate(sensorTemplate, parent.transform);
            var right = Object.Instantiate(sensorTemplate, parent.transform);
            _move(forward.transform, 0, 1);
            _move(left.transform, 1, 0);
            _move(right.transform, 1, 0);
            if (childrenLevel == 0)
            {
                forward.GetComponent<SensorBehaviour>().SetLeaf();
                left.GetComponent<SensorBehaviour>().SetLeaf();
                right.GetComponent<SensorBehaviour>().SetLeaf();
            }
            CreateChildren(forward, childrenLevel - 1);
            CreateChildren(left, childrenLevel - 1);
            CreateChildren(right, childrenLevel - 1);
        }

        private void _move(Transform t, int directionY, int directionZ)
        {
            t.RotateAround(
                Vector3.zero, 
                Vector3.left,
                directionY
            );
            t.RotateAround(
                Vector3.zero, 
                Vector3.forward,
                directionZ
            );
        }
    }
}