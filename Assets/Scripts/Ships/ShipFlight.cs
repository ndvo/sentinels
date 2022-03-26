using System;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;
using Vector3 = UnityEngine.Vector3;

namespace Ships
{
    public class ShipFlight : OrbitalFlight
    {

        private float _drag;
        public float latitude = -90f;
        public float longitude = 0f;
        // aim is used to set the direction the ship is looking to.
        // the idea is to have an object slightly ahead that moves in the same manner (transform and rotate)
        // using the aim to guide the lookAt function allow us to get rid of bugs due to the orbital navigation.
        protected GameObject Aim;
        [SerializeField] private bool inertia;
        // The SpaceStationBuilder is responsible for creating the board as is capable of noticing if the ship has left the board.
        protected SpaceStationBuilder SpaceStationBuilder;
        protected bool OffBoard;
        public float warpMultiplier;
        protected bool HasSpaceStationBuilder = false;
        protected bool ShipReady = false;

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

        public override void FixedUpdate()
        {
            if (ShipReady)
            {
                PreviousDirection = CurrentDirection;
                OffBoard = HasSpaceStationBuilder && SpaceStationBuilder.IsOffBoard(transform.position);
                _orbitalFlight();
            }
            else
            {
                GoToStartPosition();
                ShipReady = true;
            }
            _drag = 0;
        }

        protected override void _orbitalFlight()
        {
            DeltaTimeSpeed = (speed - (speed * _drag)) * Time.fixedDeltaTime;
            if (OffBoard)
            {
                DeltaTimeSpeed *= warpMultiplier;
                if (!(AudioSource is null) && !AudioSource.isPlaying) AudioSource.Play();
            }
            else
            {
                _setNewDirection();
                if (!(AudioSource is null) && AudioSource.isPlaying) AudioSource.Stop();
            }
            _positionAim();
            LookAtDirection();
            _move(transform, DeltaTimeSpeed);
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

        public void GoToBoardCenter()
        {
            var pos = transform.position;
            pos.x = 0;
            pos.z = 0;
        }

        public virtual void GoToStartPosition(Transform t = null)
        {
            GoTo(Utils.Direction.North, latitude, t);
            GoTo(Utils.Direction.East, longitude, t);
        }

        public void GoTo(Position dir, float angle, Transform t = null)
        {
            t = t ? t: transform;
            GoToBoardCenter();
            var oldDirection = CurrentDirection;
            CurrentDirection = dir;
            _move(t, angle);
            CurrentDirection = oldDirection;
        }

        /// <summary>
        /// Sets a drag percentage for a single frame.
        ///
        /// This function needs to be called for as long as the drag persists.
        /// </summary>
        /// <param name="dragPercentage"></param>
        public void SetDrag(float dragPercentage)
        {
            _drag = Mathf.Clamp(dragPercentage, 0, 0.9f);
        }
    }
}
