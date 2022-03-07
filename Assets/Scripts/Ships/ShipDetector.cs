using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ships
{
    /// <summary>
    /// Class <c>ShipDetector</c> provides a scalable sensor that detects colliding ships.
    ///
    /// It does no other effort to filter the list of colliding ships other than checking the Ship and Sentinel layers.
    /// It provides a public list of detected ships.
    /// </summary>
    public class ShipDetector : MonoBehaviour
    {
        public int size = 1;
        public List<GameObject> detected;
        private LayerMask _shipLayer;
        private MeshRenderer _renderer;
    
        void Start()
        {
            _shipLayer = LayerMask.NameToLayer("Ship");
            transform.localScale = new Vector3(size, size, size);
            _renderer = GetComponent<MeshRenderer>();
            _renderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            detected.Add(other.gameObject);
        }

        private void FixedUpdate()
        {
            detected.RemoveAll(i => i is null);
            _renderer.enabled = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            if (!other.gameObject.activeSelf)
            {
                detected.Remove(other.gameObject);
                return;
            }
            _renderer.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            _renderer.enabled = false;
            detected.Remove(other.gameObject);
        }

        public void ShowSensor()
        {
            _renderer.enabled = true;
        }

        public void HideSensor()
        {
            _renderer.enabled = false;
        }

        public GameObject[] Detect()
        {
            return detected.Where(i => !(i is null)).ToArray();
        }

        public GameObject Closest(Vector3 targetPosition)
        {
            detected = detected
                .Where(i => !(i is null) && i.activeSelf)
                .OrderBy(i => Vector3.Distance(targetPosition, i.transform.position)).ToList();
            return detected.FirstOrDefault();
        }
    }
}
