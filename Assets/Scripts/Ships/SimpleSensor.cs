using UnityEngine;

namespace Ships
{
    /// <summary>
    ///     Implements the behaviour of a simple sensor.
    ///     A sensor detects collisions with objects in a specific layer.
    ///     If the sensor detected something, it is blocked.
    ///     It stores the blocking game object in the blocking variable.
    /// </summary>
    public class SimpleSensor : MonoBehaviour
    {
        public LayerMask layer;
        public Vector3 sensorScale = Vector3.one;
        public bool blocked;
        public GameObject blocking;

        private void Update()
        {
            transform.localScale = sensorScale;
        }

        /// <summary>
        ///     Detects collisions when a target object enters the sensor
        /// </summary>
        /// <param name="other">the colliding object</param>
        public void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & layer) == 0) return;
            blocked = true;
            blocking = other.gameObject;
        }

        /// <summary>
        ///     When an object leaves the sensor, make it non blocked.
        ///     If there are other objects in the sensor, OnTriggerStay will make sure the sensor revert this change.
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & layer) == 0) return;
            blocked = false;
            blocking = null;
        }

        /// <summary>
        ///     Ensures the sensor status will be blocked while there is any object colliding.
        /// </summary>
        /// <param name="other">the colliding object</param>
        public void OnTriggerStay(Collider other)
        {
            if (((1 << other.gameObject.layer) & layer) == 0) return;
            blocked = true;
            blocking = other.gameObject;
        }
    }
}