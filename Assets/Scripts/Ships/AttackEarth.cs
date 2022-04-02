using Sky;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ships
{
    /// <summary>
    /// Implements the behaviour for attacking Earth.
    /// 
    /// Enemy vessels cannot attack Earth directly. They act somewhat like a virus that need to take control of a cell
    /// (a space station in this case) to use the station resources to attack Earth.
    ///
    /// When the target station is under control of the ship, it can attack.
    /// 
    /// </summary>
    public class AttackEarth : MonoBehaviour
    {
        public Transform targetStation;
        private bool _active;
        private GameObject _attackRay;
        private EarthBehaviour _earth;
        private EnemyBehaviour _enemyBehaviour;
        private GameObject _capturedVFX;
        public float inflicted;
        
        void Start()
        {
            _capturedVFX = transform.Find("CapturedVFX").gameObject;
            _attackRay = transform.Find("AttackRay").gameObject;
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            var earthObject = GameObject.Find("/Earth");
            if (earthObject is {}) _earth = earthObject.GetComponent<EarthBehaviour>();
            else Debug.LogError("Could not find Earth");
        }

        void Update()
        {
            if (_active) _attack();
        }

        /// <summary>
        /// Deals damage to earth and sets the ray position.
        /// </summary>
        private void _attack()
        {
            _setRayPosition();
            var totalDamage = _enemyBehaviour.drainPower * Time.deltaTime;
            if (_earth is { })
            {
                inflicted += _earth.TakeDamage(totalDamage);
            }
        }

        /// <summary>
        /// Sets a target to be controlled to attack Earth.
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target)
        {
            targetStation = target;
            if (_attackRay is { })
            {
                _attackRay.gameObject.SetActive(true);
                _capturedVFX.transform.position = target.position;
                _capturedVFX.SetActive(true);
                _active = true;
            }
            else
            {
                // Do not set target station and attack ray if target has no attack ray.
                targetStation = null;
            }
        }

        /// <summary>
        /// Stop showing the ray and set _active to false.
        /// </summary>
        public void StopAttack()
        {
            if (!_active) return;
            _active = false;
            _attackRay.SetActive(false);
            _capturedVFX.SetActive(false);
            targetStation = null;
        }


        /// <summary>
        /// Ray position must start at the target station and reach Earth.
        /// </summary>
        private void _setRayPosition()
        {
            if (_attackRay is null || targetStation is null) return;
            // Position the center of the object halfway between the two points.
            _attackRay.transform.position = targetStation.transform.position / 2;
            // turn the ray to the station.
            // The goal is to give the impression that something is being drained from Earth to the station.
            // - selecting the perfect asset for this impression has not been an easy task.
            _attackRay.transform.LookAt(targetStation);
        }
    }
}