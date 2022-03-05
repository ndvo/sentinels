using System;
using System.Collections.Generic;
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
        private LayerMask _sentinelLayer;
        private MeshRenderer _renderer;
        private float _renderTime = 0;
        private float _maxRenderTime = 0.5f;
    
        void Start()
        {
            _shipLayer = LayerMask.NameToLayer("Ship");
            _sentinelLayer = LayerMask.NameToLayer("Sentinel");
            transform.localScale = new Vector3(size, size, size);
            _renderer = GetComponent<MeshRenderer>();
            _renderer.enabled = false;
        }

        private void Update()
        {
            _renderTime += Time.deltaTime;
            if (_renderTime > 10) _renderTime = _maxRenderTime;
            //if (_renderTime >= _maxRenderTime) _renderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _shipLayer &&
                other.gameObject.layer != _sentinelLayer) return;
            _renderer.enabled = true;
            _renderTime = 0;
            detected.Add(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != _shipLayer &&
                other.gameObject.layer != _sentinelLayer) return;
            _renderer.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _shipLayer &&
                other.gameObject.layer != _sentinelLayer) return;
            _renderer.enabled = false;
        }

        public void ShowSensor()
        {
            _renderer.enabled = true;
        }

        public void HideSensor()
        {
            _renderer.enabled = false;
        }
    }
}
