using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    /// <summary>
    ///     ShipFlight provides a base class for ship orbital flight.
    ///     Ships need to do more in Orbital Flight than space bodies. They need to look at the direction they are going and
    ///     need to be arbitrarily positioned.
    ///     Ships also need to use warp drive when off the board.
    /// </summary>
    public class ShipFlight : OrbitalFlight
    {
        public float latitude = -90f;

        public float longitude;

        [SerializeField] private bool inertia;
        public float warpMultiplier;

        private float _drag;
        private float _multiplier = 1;

        // aim is used to set the direction the ship is looking to.
        // the idea is to have an object slightly ahead that moves in the same manner (transform and rotate)
        // using the aim to guide the lookAt function allow us to get rid of bugs due to the orbital navigation.
        protected GameObject Aim;
        protected bool HasSpaceStationBuilder;
        protected bool OffBoard;
        protected bool ShipReady = false;

        // The SpaceStationBuilder is responsible for creating the board as is capable of noticing if the ship has left the board.
        protected SpaceStationBuilder SpaceStationBuilder;

        /// <summary>
        ///     Creates a aim object to help pointing the ship in the right direction when moving.
        /// </summary>
        public virtual void Awake()
        {
            Aim = new GameObject("aim");
            Aim.transform.parent = transform;
            var station = GameObject.Find("SpaceStationBuilder");
            if (station != null)
            {
                SpaceStationBuilder = station.GetComponent<SpaceStationBuilder>();
                HasSpaceStationBuilder = true;
            }

            AudioSource = GetComponent<AudioSource>();
        }

        public override void Start()
        {
            base.Start();
            GoToStartPosition();
        }

        public override void Update()
        {
            if (!OffBoard) _setNewDirection();
        }

        /// <summary>
        ///     Override orbital flight to add
        /// </summary>
        protected override void _orbitalFlight()
        {
            OffBoard = HasSpaceStationBuilder && SpaceStationBuilder.IsOffBoard(transform.position);
            if (OffBoard)
            {
                _multiplier = warpMultiplier;
                if (!(AudioSource is null) && !AudioSource.isPlaying) AudioSource.Play();
            }
            else
            {
                _multiplier = 1;
                if (!(AudioSource is null) && AudioSource.isPlaying) AudioSource.Stop();
            }

            DeltaTimeSpeed = _multiplier * (speed - speed * _drag) * Time.fixedDeltaTime;
            _positionAim();
            LookAtDirection();
            _move(transform, DeltaTimeSpeed);
            _drag = 0;
        }

        protected virtual void _positionAim()
        {
            var position = transform.position;
            var rotation = transform.rotation;
            Aim.transform.position = new Vector3(position.x, position.y, position.z);
            Aim.transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            _move(Aim.transform, speed);
        }

        protected virtual void LookAtDirection()
        {
            transform.LookAt(Aim.transform.position);
        }

        /// <summary>
        ///     Move the ship to the center of the board.
        ///     This function assumes the distance from Earth is 500.
        /// </summary>
        public void GoToBoardCenter()
        {
            var pos = transform.position;
            pos.x = 0;
            pos.z = 0;
            pos.y = 500;
        }

        /// <summary>
        ///     Moves the ship to an initial position defined as latitude and longitude.
        /// </summary>
        /// <param name="t"></param>
        public virtual void GoToStartPosition(Transform t = null)
        {
            GoTo(Direction.North, latitude, t);
            GoTo(Direction.East, longitude, t);
        }

        /// <summary>
        ///     Move the ship to a specified position
        ///     The position is specified as an angle and a direction.
        ///     The distance of the ship from Vector3.zero will not change.
        /// </summary>
        /// <param name="dir">The direction the ship should move to.</param>
        /// <param name="angle">The angle to move.</param>
        /// <param name="t">The transform to be moved.</param>
        public void GoTo(Position dir, float angle, Transform t = null)
        {
            t = t ? t : transform;
            GoToBoardCenter();
            var oldDirection = CurrentDirection;
            CurrentDirection = dir;
            _move(t, angle);
            CurrentDirection = oldDirection;
        }

        /// <summary>
        ///     Sets a drag percentage for a single frame.
        ///     This function needs to be called for as long as the drag persists.
        /// </summary>
        /// <param name="dragPercentage"></param>
        public void SetDrag(float dragPercentage)
        {
            _drag = Mathf.Clamp(dragPercentage, 0, 0.9f);
        }
    }
}