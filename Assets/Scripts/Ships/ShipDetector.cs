using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
        void Start()
        {
            _shipLayer = LayerMask.NameToLayer("Ship");
            transform.localScale = new Vector3(size, size, size);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            detected.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            detected.Remove(other.gameObject);
        }

        private void Update()
        {
            detected = detected.Where(i => !(i is null) && i.activeSelf).ToList();
        }

        public GameObject Closest(Vector3 targetPosition)
        {
            detected = detected
                .OrderBy(i => Vector3.Distance(targetPosition, i.transform.position)).ToList();
            return detected.FirstOrDefault();
        }
    }
}
