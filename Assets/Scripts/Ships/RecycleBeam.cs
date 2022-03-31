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
        private ShipFlight _shipFlight;
        private ShipFlight _targetFlight;
        private SpaceShip _targetShip;
        public int power = 10;
        private AudioSource _audioSource;
        private SentinelShip _sentinelShip;
        public GameObject help;
        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            var parent = transform.parent;
            _shipTransform = parent.transform;
            _shipFlight = parent.gameObject.GetComponent<ShipFlight>();
            _detector = detector.GetComponent<ShipDetector>();
            _beam = GameObject.Find("Beam");
            _audioSource = GetComponent<AudioSource>();
            _sentinelShip = parent.transform.Find("CoreParts").GetComponent<SentinelShip>();
            _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
        }

        private void Update()
        {
            _hasTarget = !(_target is null);
            _activateBeam = _hasTarget && (Input.GetAxis("Fire1") > 0);
            _beam.SetActive(_activateBeam);
        }

        private void FixedUpdate()
        {
            if (_activateBeam) Fire(_target);
            else FindTarget();
        }

        void FindTarget()
        {
            if (_audioSource.isPlaying) _audioSource.Stop();
            _target = _detector.Closest(transform.position);
            _hasTarget = !(_target is null);
            if (_hasTarget)
                Capture(_target);
            else
                Release();
        }

        private void Fire(GameObject target)
        {
            if (!_audioSource.isPlaying) _audioSource.Play();
            transform.LookAt(target.transform.position);
            _shipFlight.MoveWithMe(target.transform);
            _shipFlight.SetDrag(0.6f);
            _targetFlight.SetDrag(0.99f);
            if (!(_target is null) && !(_targetShip is null))
            {
                var energyLeft = _targetShip.TakeDamage(power);
                _sentinelShip.RecoverEnergy(1);
                if (energyLeft <= 0)
                {
                    Release();
                }
            } else Release();
        }

        private void Capture(GameObject target)
        {
            _gameManager.ShowHelp(help);
            _targetShip = target.GetComponentInChildren<SpaceShip>();
            if (_targetShip is {alive: false})
            {
                Release();
                return;
            }
            _targetFlight = target.GetComponent<ShipFlight>();
        }

        private void Release()
        {
            help.SetActive(false);
            _target = null;
            _targetFlight = null;
            _audioSource.Stop();
        }
    }
}
