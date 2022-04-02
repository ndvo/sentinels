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

        /// <summary>
        /// Detect vessels when they enter the sensor.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            detected.Add(other.gameObject);
        }

        /// <summary>
        /// Remove vessels from detected list when they leave sensor
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != _shipLayer) return;
            detected.Remove(other.gameObject);
        }

        /// <summary>
        /// Clean up detected list.
        ///
        /// GameObjects in this list may have been destroyed and referencing destroyed objects can cause unexpected
        /// behaviour.
        /// </summary>
        private void Update()
        {
            detected = detected.Where(i => !(i is null) && i.activeSelf).ToList();
        }

        /// <summary>
        /// Returns the detected object that is closest to the provided target.
        /// </summary>
        /// <param name="targetPosition">a position one wants the closest object from.</param>
        /// <returns>the closest game object</returns>
        public GameObject Closest(Vector3 targetPosition)
        {
            detected = detected
                .OrderBy(i => Vector3.Distance(targetPosition, i.transform.position)).ToList();
            return detected.FirstOrDefault();
        }
    }
}
