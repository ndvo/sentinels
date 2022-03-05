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
        //private SpaceShip _targetShip;

        // Start is called before the first frame update
        void Start()
        {
            var parent = transform.parent;
            _shipTransform = parent.transform;
            _shipFlight = parent.gameObject.GetComponent<OrbitalFlight>();
            _detector = detector.GetComponent<ShipDetector>();
            _beam = GameObject.Find("Beam");
        }

        private void FixedUpdate()
        {
            _activateBeam = _hasTarget && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
            _beam.SetActive(_activateBeam);
            if (_activateBeam) Fire(_target);
            else FindTarget();
        }

        void FindTarget()
        {
            var found = _detector.detected
                .OrderBy(i => Vector3.Distance(i.transform.position, _shipTransform.position))
                .FirstOrDefault(i => i != _shipTransform.gameObject);
            _hasTarget = found is { };
            if (_hasTarget)
            {
                _target = found;
                if (_target is { }) _targetFlight = _target.gameObject.GetComponent<OrbitalFlight>();
            }
            else
            {
                _target = null;
                _targetFlight = null;
            }
        }

        void Fire(GameObject target)
        
        {
            transform.LookAt(target.transform.position);
            _shipFlight.MoveWithMe(target.transform);
            _shipFlight.SetDrag(0.3f);
            _targetFlight.SetDrag(0.9f);
        }

        void Capture(GameObject target)
        {
            var go = 
            _targetFlight = _target.GetComponent<OrbitalFlight>();
            //_targetShip = _target.GetComponent<SpaceShip>();
        }
    }
}
