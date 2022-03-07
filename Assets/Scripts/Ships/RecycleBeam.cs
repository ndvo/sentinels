using System;
using System.Linq;
using UnityEngine;

namespace Ships
{
    public class RecycleBeam : MonoBehaviour
    {
        private ShipDetector _detector;
        private GameObject _beam;
        private Transform _shipTransform;
        private GameObject _target;
        public GameObject detector;
        private bool _hasTarget;
        private bool _activateBeam = false;
        private OrbitalFlight _shipFlight;
        private OrbitalFlight _targetFlight;
        private SpaceShip _targetShip;
        public int power = 10;

        // Start is called before the first frame update
        void Start()
        {
            var parent = transform.parent;
            _shipTransform = parent.transform;
            _shipFlight = parent.gameObject.GetComponent<OrbitalFlight>();
            _detector = detector.GetComponent<ShipDetector>();
            _beam = GameObject.Find("Beam");
        }

        private void Update()
        {
            _hasTarget = !(_target is null);
            _activateBeam = _hasTarget && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            _beam.SetActive(_activateBeam);
            if (_activateBeam) Fire(_target);
            else FindTarget();
        }

        void FindTarget()
        {
            _target = _detector.Closest(transform.position);
            _hasTarget = !(_target is null);
            if (_hasTarget)
                Capture(_target);
            else
                Release();
        }

        private void Fire(GameObject target)
        {
            transform.LookAt(target.transform.position);
            _shipFlight.MoveWithMe(target.transform);
            _shipFlight.SetDrag(0.5f);
            _targetFlight.SetDrag(0.9f);
            var energyLeft = _targetShip.TakeDamage(power);
            if (energyLeft <= 0)
            {
                Release();
            }
        }

        private void Capture(GameObject target)
        {
            _targetShip = target.GetComponentInChildren<SpaceShip>();
            if (_targetShip is {alive: false})
            {
                Release();
                return;
            }
            _targetFlight = target.GetComponent<OrbitalFlight>();
        }

        private void Release()
        {
            _target = null;
            _targetFlight = null;
        }
    }
}
