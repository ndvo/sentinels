using Sky;
using UnityEngine;

namespace Ships
{
    public class AttackEarth : MonoBehaviour
    {
        public Transform _targetStation;
        private bool _active;
        private GameObject _attackRay;
        private EarthBehaviour _earth;
        private EnemyBehaviour _enemyBehaviour;
        private GameObject _capturedVFX;
        
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

        private void _attack()
        {
            _setRayPosition();
            var totalDamage = _enemyBehaviour.drainPower * Time.deltaTime;
            if (_earth is {}) _earth.TakeDamage(totalDamage);
        }

        public void SetTarget(Transform target)
        {
            _targetStation = target;
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
                _targetStation = null;
            }
        }

        public void StopAttack()
        {
            if (!_active) return;
            _active = false;
            _attackRay.SetActive(false);
            _capturedVFX.SetActive(false);
            _targetStation = null;
        }


        private void _setRayPosition()
        {
            if (_attackRay is null || _targetStation is null) return;
            _attackRay.transform.position = _targetStation.transform.position / 2;
            _attackRay.transform.LookAt(_targetStation);
        }
    }
}