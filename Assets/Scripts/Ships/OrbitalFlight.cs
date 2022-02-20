using UnityEngine;
using Utils;
using Time = UnityEngine.Time;
using Vector3 = UnityEngine.Vector3;

namespace Ships
{
    public class OrbitalFlight : MonoBehaviour
    {

        public float speed;
        protected Position Direction = new Position(0, 0);
        protected Position PreviousDirection = new Position(0, 0);

        protected float DeltaTimeSpeed = 0;
        // aim is used to set the direction the ship is looking to.
        // the idea is to have an object slightly ahead that moves in the same manner (transform and rotate)
        // using the aim to guide the lookAt function allow us to get rid of bugs due to the orbital navigation.
        protected GameObject Aim;
        [SerializeField] private bool inertia;
        // The SpaceStationBuilder is responsible for creating the board as is capable of noticing if the ship has left the board.
        protected SpaceStationBuilder SpaceStationBuilder;
        protected bool OffBoard;
        public float warpMultiplier;

        public virtual void Awake()
        {
            Aim = new GameObject();
            SpaceStationBuilder = GameObject.Find("SpaceStationBuilder").GetComponent<SpaceStationBuilder>();
        }

        public virtual void Start()
        {
        }


        public void FixedUpdate()
        {
            OffBoard = SpaceStationBuilder.IsOffBoard(transform.position);
            _orbitalFlight();
        }

        protected void _orbitalFlight()
        {
            DeltaTimeSpeed = speed * Time.deltaTime;
            if (OffBoard) DeltaTimeSpeed *= warpMultiplier;
            if (!OffBoard) _setNewDirection();
            _positionAim();
            LookAtDirection();
            _move(transform, DeltaTimeSpeed);
        }

        protected virtual void _move(Transform t, float moveSpeed)
        {
            t.RotateAround(
                Vector3.zero, 
                Vector3.left,
                moveSpeed * Direction.y
            );
            t.RotateAround(
                Vector3.zero, 
                Vector3.forward,
                moveSpeed * Direction.x
            );
        }

        protected virtual void _setNewDirection()
        {
            Direction = Utils.Direction.North;
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

        public void GoToStartPosition(Transform t = null)
        {
            GoTo(Utils.Direction.South, 90, t);
        }

        public void GoTo(Position dir, float angle, Transform t = null)
        {
            t = t ? t: transform;
            GoToBoardCenter();
            var oldDirection = Direction;
            Direction = dir;
            _move(t, angle);
            Direction = oldDirection;
        }
    }
}
