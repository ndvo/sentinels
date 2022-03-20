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
        public NaturalResources attackType = NaturalResources.Mineral;
        private EnemyBehaviour _enemyBehaviour;

        void Start()
        {
            _attackRay = transform.Find("AttackRay").gameObject;
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
            _earth = GameObject.Find("/Earth").GetComponent<EarthBehaviour>();
        }

        void Update()
        {
            if (_active) _attack();
        }

        private void _attack()
        {
            _setRayPosition();
            _earth.LooseResources(_enemyBehaviour.firePower * Time.deltaTime, attackType);
        }

        public void SetTarget(Transform target)
        {
            _targetStation = target;
            if (_attackRay is { })
            {
                _attackRay.gameObject.SetActive(true);
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